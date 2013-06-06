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
	using Microsoft.Practices.Composite.Presentation.Events;

	#endregion // Using

	public interface IBurndownChartMenuView
	{
	}

	public class BurndownChartMenuVm : ViewModelBase
	{
		#region Fields

		bool _isLoaded = false;
		BurndownChart _model = null;
		bool _isIterationWisEnabled = false;
		bool _isWiFieldsEnabled = false;
		IBurndownChartMenuView _view;
		IterationWiQuery _iterationWiQuery;
		WorkItemFieldQuery _workItemFieldQuery;

		#endregion // Fields

		#region Properties

		public IBurndownChartMenuView View
		{
			get
			{
				return _view;
			}
			set
			{
				_view = value;
				if (value != null)
				{
					Load();
				}
				else
				{
					Unload();
				}
			}
		}

		public TfsTeamProjectVm TeamProjectVm { get; private set; }

		private void OnUpdateIntervalChanged(SlideElement model)
		{
			if (_view == null || !_isLoaded)
			{
				return;
			}

			QueryManager.Instance.UpdatePollingInterval<BurndownQuery>(q => q.SourceId == _model.Id, model.UpdateInterval);
		}


		public ObservableCollection<IterationNode> IterationWis { get; private set; }

		public bool IsIterationWisEnabled
		{
			get { return _isIterationWisEnabled; }
			set
			{
				_isIterationWisEnabled = value;
				RaisePropertyChanged(() => IsIterationWisEnabled);
			}
		}

		public bool IsWiFieldsEnabled
		{
			get { return _isWiFieldsEnabled; }
			set
			{
				_isWiFieldsEnabled = value;
				RaisePropertyChanged(() => IsWiFieldsEnabled);
			}
		}


		public ObservableCollection<string> EstimateFields { get; private set; }

		public ObservableCollection<string> RemainingFields { get; private set; }

		public string RemainingField 
		{
			get { return _model.RemainingFieldName; }
			set 
			{
				_model.RemainingFieldName = value;
				RaisePropertyChanged(() => RemainingField);
				UpdateModels(true);
			}
		}

		public string EstimateField
		{
			get { return _model.EstimateFieldName; }
			set
			{
				_model.EstimateFieldName = value;
				RaisePropertyChanged(() => EstimateField);
				UpdateModels(true);
			}
		}

		public ObservableCollection<SprintBurndownType> SprintBurndownTypes { get; private set; }

		public SprintBurndownType SelectedSprintBurndownType
		{
			get 
			{
				return (_model == null) ? null : _model.SprintBurndownType; 
			}
			set
			{
				if (_model != null && value != null)
				{
					if (value.GetType() != _model.SprintBurndownType.GetType())
					{
						_model.SprintBurndownType = value;
						UpdateModels(true);
					}
				}
			}
		}

		IterationWiNode _selectedIterationWi;

		public IterationWiNode SelectedIterationWi
		{
			get
			{
				return _selectedIterationWi; 
			}
			set
			{
				if (_model != null && value != null)
				{
					_model.TfsIteration.IterationPath = value.FullPath;
					_model.TfsIteration.IterationId = value.IterationId;
					UpdateModels(true);
					//RaisePropertyChanged(() => SelectedIterationWi);
				}
			}
		}

		public bool ExcludeWeekEnds
		{
			get { return _model.ExcludeWeekEnds; }
			set
			{
				_model.ExcludeWeekEnds = value;
				UpdateModels(true);
			}
		}

		#endregion // Properties

		public BurndownChartMenuVm(BurndownChart model)
		{
			_model = model;

			this.SprintBurndownTypes = new ObservableCollection<SprintBurndownType>();
			this.IterationWis = new ObservableCollection<IterationNode>();
			this.EstimateFields = new ObservableCollection<string>();
			this.RemainingFields = new ObservableCollection<string>();
			this.TeamProjectVm = new TfsTeamProjectVm(null);

			//ViewModelEvents.Instance.SlideElementSelected.Subscribe(OnSlideElementSelected);

		}

		private void OnSlideElementSelected(SlideElementCollection elements)
		{
			
			var model = elements.SingleOrDefault() as BurndownChart;
			if (_model == model)
			{
				return;
			}

			Unload();

			_model = model;
			if (model != null)
			{
				this.SprintBurndownTypes.Clear();
				this.IterationWis.Clear();
				this.EstimateFields.Clear();
				this.RemainingFields.Clear();
				//this.TeamProjectVm = new TfsTeamProjectVm(null);
				//RaisePropertyChanged(() => TeamProjectVm);
				Load();
			}
		}

		#region Methods

		private void Load()
		{
			if (_isLoaded)
			{
				return;
			}

			// Add chart-types
			var allTypes = new SprintBurndownTypeCollection();
			foreach (var sbType in allTypes)
			{

				if (sbType.GetType() == _model.SprintBurndownType.GetType())
				{
					this.SprintBurndownTypes.Add(_model.SprintBurndownType);
					this.SelectedSprintBurndownType = sbType;
				}
				else
				{
					this.SprintBurndownTypes.Add(sbType);
				}
			}

			this.EstimateFields.Add("Story points");
			this.EstimateFields.Add("Original estimate");
			this.EstimateFields.Add("Estimate");
			this.RemainingFields.Add("Remaining Work");
			AppCommands.BindCommand(AppCommands.ExcludeWeekEnds, ExcludeWeekEnds_Executed, ExcludeWeekEnds_CanExecute);
			ViewModelEvents.Instance.TeamProjectChanged.Subscribe(OnTeamProjectChanged);
			QueryWiFields();
			QueryIterationWi();
			ViewModelEvents.Instance.TfsIterationChanged.Subscribe(OnTfsIterationChanged, ThreadOption.UIThread, false, s => s.Id == _model.Id);
			ViewModelEvents.Instance.UpdateIntervalChanged.Subscribe(OnUpdateIntervalChanged, ThreadOption.UIThread, false, s => s.Id == _model.Id);

			var elements = new SlideElementCollection(_model);
			this.TeamProjectVm.Invalidate(elements);

			_isLoaded = true;
		}

		private void OnTfsIterationChanged(SlideElement model)
		{
			if (!_isLoaded)
			{
				return;
			}

			QueryIterationWi();
		}

		private void AddIterationWiNodes(IterationWiNode parent, IterationWiCollection iterations)
		{
			if (iterations == null)
			{
				return;
			}

			foreach (IterationWi it in iterations)
			{
				var itn = new IterationWiNode(parent, it.IterationWiId, it.Name, it.IterationWiId != null);
				if (parent == null)
				{
					this.IterationWis.Add(itn);
				}
				else
				{
					parent.Add(itn);
				}
				AddIterationWiNodes(itn, it.Children);

				// Select from model?
				if (itn.Selectable)
				{
					if (_model.TfsIteration.IterationId.HasValue && _model.TfsIteration.IterationId == itn.IterationId.Value)
					{
						Debug.WriteLine("Current: " + _model.TfsIteration.IterationName);
						_selectedIterationWi = itn;
						RaisePropertyChanged(() => SelectedIterationWi);
						//this.SelectedIterationWi = itn;
					}
				}
			}
		}		


		private void QueryWiFields()
		{
			if (_workItemFieldQuery != null)
			{
				ViewModelEvents.Instance.WorkItemFieldQueryCompleted.Unsubscribe(WorkItemFieldQueryCompleted);
			}

			this.IsWiFieldsEnabled = false;
			this.EstimateFields.Clear();
			this.RemainingFields.Clear();

			if (!string.IsNullOrEmpty(_model.TfsWorkItemFilter.TeamProjectName))
			{
				_workItemFieldQuery = new WorkItemFieldQuery(_model.Id, null, new WorkItemFieldQueryArg(_model.TfsWorkItemFilter.TeamProjectName),
					WorkItemFieldQueryCompleted);
				_workItemFieldQuery.Query(false);
			}
		}

		private void QueryIterationWi()
		{
			if (_iterationWiQuery != null)
			{
				ViewModelEvents.Instance.IterationWiQueryCompleted.Unsubscribe(IterationWiQueryCompleted);
			}

			this.IterationWis.Clear();
			this.IsIterationWisEnabled = false;
			_selectedIterationWi = new IterationWiNode(null, null, "Loading...", false);
			RaisePropertyChanged(() => SelectedIterationWi);


			if (!string.IsNullOrEmpty(_model.TfsWorkItemFilter.TeamProjectName))
			{
				_iterationWiQuery = new IterationWiQuery(_model.Id, null, new IterationWiQueryArg(_model.TfsWorkItemFilter.TeamProjectName), IterationWiQueryCompleted);
				_iterationWiQuery.Query(false);
			}
		}

		private void OnTeamProjectChanged(SlideElement elem)
		{
			if (elem.Id != _model.Id)
			{
				return;
			}

			_model.TfsIteration.TeamProject = elem.TfsWorkItemFilter.TeamProjectName;

			QueryWiFields();
			QueryIterationWi();
		}

		private void ExcludeWeekEnds_Executed(object sender, ExecutedRoutedEventArgs e)
		{
		}

		private void ExcludeWeekEnds_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void UpdateModels(bool updateQuery)
		{
			ViewModelEvents.Instance.SlideElementUpdated.Publish(_model);

			if (updateQuery)
			{
				if (_model.TfsIteration.IterationId != null)
				{
					var qry = new BurndownQuery(_model, null);
					qry.Query(true);
				}
			}
		}

		public void WorkItemFieldQueryCompleted(WorkItemFieldQuery qry)
		{
			this.SelectedIterationWi = null;
			RaisePropertyChanged(() => SelectedIterationWi);


			if (!_isLoaded)
			{
				return;
			}

			if (qry.Exception != null)
			{
				this.EstimateFields.Add("Error");
				this.RemainingFields.Add("Error");
				this.IsWiFieldsEnabled = false;
			}
			else
			{
				this.EstimateFields.Clear();
				this.RemainingFields.Clear();
				foreach (string f in qry.Results)
				{
					this.EstimateFields.Add(f);
					this.RemainingFields.Add(f);
				}
				this.IsWiFieldsEnabled = true;
			}
		}

		public void IterationWiQueryCompleted(IterationWiQuery qry)
		{
			if (!_isLoaded)
			{
				return;
			}

			this.IterationWis.Clear();
			_selectedIterationWi = null;
			RaisePropertyChanged(() => SelectedIterationWi);

			if (qry.Exception != null)
			{
				this.IterationWis.Add(new IterationNode(null, "Error"));
			}
			else
			{
				AddIterationWiNodes(null, qry.Results);
				RaisePropertyChanged(() => IterationWis);
				this.IsIterationWisEnabled = true;
			}
		}

		public void Unload()
		{
			if (_isLoaded)
			{
				_isLoaded = false;
				this.TeamProjectVm.Unload();
				ViewModelEvents.Instance.TeamProjectChanged.Unsubscribe(OnTeamProjectChanged);
				ViewModelEvents.Instance.IterationWiQueryCompleted.Unsubscribe(IterationWiQueryCompleted);
				ViewModelEvents.Instance.WorkItemFieldQueryCompleted.Unsubscribe(WorkItemFieldQueryCompleted);
			}
		}

		#endregion // Methods
	}
}
