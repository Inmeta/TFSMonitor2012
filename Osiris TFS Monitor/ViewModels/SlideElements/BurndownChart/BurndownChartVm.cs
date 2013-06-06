namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.Windows.Input;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Report;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using System.Net;
	using Microsoft.TeamFoundation.Client;
	using Osiris.Tfs.Monitor.Properties;
	using System.IO;
	using System.Collections.ObjectModel;
	using Microsoft.Practices.Composite.Events;
	using System.Windows;
	using Osiris.TFS.Monitor.Common;
	using Microsoft.Practices.Composite.Presentation.Events;
	using System.Threading;
	using System.Windows.Threading;

	#endregion // Using

	public interface ISlideElementVm
	{
	}

	public interface IBurndownChartView
	{
		void UpdateChart(SprintBurndown data);
		void UpdateDataContext();
	}

	public class BurndownChartVm : SlideElementVm, ISlideElementVm
	{
		#region Fields

		BurndownChart _model;
		Scale2d _scale2d = new Scale2d(2);
		IBurndownChartView _view;
		string _infoText;
		bool _enablePolling;
		BurndownQuery _burndownQuery = null;
		bool _pollingStarted = false;
		private bool _isLoaded = false;
		private readonly bool _autoUnload;

		#endregion // Fields

		#region Properties

		public IBurndownChartView View 
		{
			get { return _view; }
			set
			{
				_view = value;

				// View established? --> Update chart
				if (_view != null)
				{
					Load();
					UpdateView();
				}
				// View disconnected? --> Dispose vm
				else
				{
					if (_autoUnload)
					{
						Unload();
					}
				}
			}
		}

		public Scale2d Scale2d { get { return _scale2d; } }

		public string Title
		{
			get
			{
				if (_model.TfsIteration == null)
				{
					return "";
				}

				var itName = _model.TfsIteration.IterationName;

				if (string.IsNullOrEmpty(itName))
				{
					return "Unspecified iteration";
				}

				return itName;
			}
		}

		public string InfoText 
		{
			get { return _infoText; }
			set
			{
				_infoText = value;
				RaisePropertyChanged(() => InfoText);
				RaisePropertyChanged(() => ShowInfoText);
			}
		}

		public bool ShowInfoText
		{
			get { return (!string.IsNullOrEmpty(_infoText)); }
		}

		public bool ExcludeWeekEnds
		{
			get { return _model.ExcludeWeekEnds; }
		}

		#endregion // Properties

		#region Constructors

		public BurndownChartVm(BurndownChart model, bool enablePolling, bool autoUnload)
		{
			_autoUnload = autoUnload;
			_model = model;
			_enablePolling = enablePolling;
		}

		#endregion // Constructors

		#region Methods

		private void Load()
		{
			if (_isLoaded)
			{
				return;
			}

			_isLoaded = true;
			if (_burndownQuery != null)
			{
				ViewModelEvents.Instance.BurndownQueryCompleted.Unsubscribe(OnBurndownQueryCompleted);
			}

			_burndownQuery = new BurndownQuery(_model, _enablePolling ? _model.UpdateInterval : (int?)null, OnBurndownQueryCompleted);
			if (_model.TfsIteration.IterationId != null)
			{
				_burndownQuery.Query(false);
			}
			
			ViewModelEvents.Instance.SlideElementUpdated.Subscribe(OnSlideElementUpdated, ThreadOption.UIThread, false, s => s.Id == _model.Id);
		}

		public override void Refresh()
		{
			_burndownQuery.Query(true);
		}


		private void OnSlideElementUpdated(SlideElement model)
		{
			if (_view == null || !_isLoaded)
			{
				return;
			}

			_view.UpdateDataContext();
		}

		private void OnBurndownQueryCompleted(BurndownQuery qry)
		{
			if (_view == null || !_isLoaded)
			{
				return;
			}

			if (_enablePolling && !_pollingStarted)
			{
				_pollingStarted = true;
				_burndownQuery.Enque();
			}

			_queryResult = qry;
			UpdateView();
		}

		private BurndownQuery _queryResult;

		private void UpdateView()
		{
			if (_queryResult == null)
			{
				return;
			}

			if (_queryResult.Exception == null)
			{
				this.InfoText = "";
				_view.UpdateChart(_queryResult.Results);
			}
			else
			{
				this.InfoText = "Requesting TFS data failed: '" + _queryResult.Exception.Message + "'";
			}
			
		}

		public void SizeChanged(double width, double height)
		{
			_scale2d.SetSize(width, height);
			RaisePropertyChanged(() => Scale2d);
		}

		#endregion // Methods

		#region IDisposable Members

		public override void Unload()
		{
			if (!_isLoaded)
			{
				return;
			}

			Debug.WriteLine("Unloading...");

			if (_burndownQuery != null)
			{
				_burndownQuery.Remove();
				ViewModelEvents.Instance.BurndownQueryCompleted.Unsubscribe(OnBurndownQueryCompleted);
			}

			ViewModelEvents.Instance.SlideElementUpdated.Unsubscribe(OnSlideElementUpdated);
			_isLoaded = false;
		}

		#endregion // IDisposable Members
	}
}
