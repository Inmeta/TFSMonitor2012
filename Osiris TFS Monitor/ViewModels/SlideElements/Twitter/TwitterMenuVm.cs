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
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Windows.Threading;
	using System.ComponentModel;
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Collections.Specialized;

	#endregion // Using

	public interface ITwitterMenuView
	{
	}

	public class TwitterMenuVm : ViewModelBase
	{
		ITwitterMenuView _view; 
		bool _disposed = false;
		Twitter _model = null;

		#region Properties

		public ITwitterMenuView View
		{
			get
			{
				return _view;
			}
			set
			{
				_view = value;
				if (_view == null)
				{
					Dispose();
				}
				else
				{
					Init();
				}
			}
		}

		public int? UpdateInterval
		{
			get
			{
				return _model.UpdateInterval;
			}
			set
			{
				if (value.HasValue)
				{
					// Error or 0?
					if (value.Value > 0)
					{
						_model.UpdateInterval = value.Value;
						QueryManager.Instance.UpdatePollingInterval<TwitterQuery>(q => q.SourceId == _model.Id, value.Value);
					}
				}
			}
		}

		public string Title
		{
			get
			{
				return _model.Title;
			}
			set
			{
				_model.Title = value;
				UpdateModels(false);
			}
		}

		public int Columns
		{
			get
			{
				return _model.Columns;
			}
			set
			{
				if (value > 0 && value < 10)
				{
					_model.Columns = value;
					UpdateModels(true);
				}
			}
		}

		public int Rows
		{
			get
			{
				return _model.Rows;
			}
			set
			{
				if (value > 0 && value < 100)
				{
					_model.Rows = value;
					UpdateModels(true);
				}
			}
		}

		public string Query
		{
			get
			{
				return _model.Query;
			}
			set
			{
				_model.Query = value;
				UpdateModels(true);
			}
		}

		#endregion // Properties

		#region Constructors

		public TwitterMenuVm(Twitter model)
		{
			_model = model;

			var qry = new TwitterQuery(_model.Id, null, _model, OnTwitterQueryCompleted);
			qry.Query(false);
		}

		#endregion // Constructors

		#region Methods

		private void Init()
		{
		}

		private void OnTwitterQueryCompleted(TwitterQuery qry)
		{
			if (_disposed)
			{
				return;
			}


			if (qry.Exception != null)
			{
			}
			else
			{
			}

			
		}

		public void UpdateModels(bool updateQuery)
		{
			/*if (!updateQuery)
			{
				ViewModelEvents.Instance.SlideElementUpdated.Publish(_model);
			}
			else
			{
				var qry = new TwitterQuery(_model.Id, null, _model.BuildFilter);
				qry.Query(true);
			}*/
		}

		#endregion // Methods

		public void Dispose()
		{
			_disposed = true;
		}
	}


}
