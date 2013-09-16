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

	public interface IBuildMonitorMenuView
	{
         
	}

	public class BuildMonitorMenuVm : ViewModelBase
	{
		#region Fields

		IBuildMonitorMenuView _view; 
		bool _disposed = false;
		BuildMonitor _model = null;
		bool _isTeamProjectsEnabled = false;

		#endregion // Fields

		#region Properties

		public IBuildMonitorMenuView View
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
						QueryManager.Instance.UpdatePollingInterval<BuildQuery>(q => q.SourceId == _model.Id, value.Value);
					}
				}
			}
		}

		public bool IsTeamProjectsEnabled
		{
			get { return _isTeamProjectsEnabled; }
			set
			{
				_isTeamProjectsEnabled = value;
				RaisePropertyChanged(() => IsTeamProjectsEnabled);
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

		public ObservableNodeList TeamProjects { get; private set; }

		public ObservableNodeList BuildStatus { get; private set; }

		#endregion // Properties

		#region Constructors

		public BuildMonitorMenuVm(BuildMonitor model)
		{
			_model = model;

			this.TeamProjects = new ObservableNodeList();

			// Build status
			this.BuildStatus = new ObservableNodeList();
			this.BuildStatus.Add(new BuildStatusNode(this, BuildFilterStatus.Failed, (_model.Status & BuildFilterStatus.Failed) != 0, "Failed"));
			this.BuildStatus.Add(new BuildStatusNode(this, BuildFilterStatus.PartiallySucceeded, (_model.Status & BuildFilterStatus.PartiallySucceeded) != 0, "Partially succeeded"));
			this.BuildStatus.Add(new BuildStatusNode(this, BuildFilterStatus.InProgress, (_model.Status & BuildFilterStatus.InProgress) != 0, "In progress"));
			this.BuildStatus.Add(new BuildStatusNode(this, BuildFilterStatus.Succeeded, (_model.Status & BuildFilterStatus.Succeeded) != 0, "Succeeded"));

			var qry = new TeamProjectQuery(_model.Id, null, OnTeamProjectsQueryCompleted);
			qry.Query(false);
		}

		#endregion // Constructors

		#region Methods

		private void Init()
		{
		}

		private void OnTeamProjectsQueryCompleted(TeamProjectQuery qry)
		{
			if (_disposed)
			{
				return;
			}

			this.TeamProjects.Clear();
	
			if (qry.Exception != null)
			{
				this.TeamProjects.Add(new TeamProjectNode(this, true, "Request failed. Check the connection string, press Osiris icon on top left, choose Application options"));
			}
			else
			{
				foreach (var tp in qry.Results)
				{
					this.TeamProjects.Add(new TeamProjectNode(this, _model.TeamProjects.Exists(p => p == tp.Name), tp.Name));
				}
			}

			this.IsTeamProjectsEnabled = (qry.Exception == null);
            
		}

		public void UpdateModels(bool updateQuery)
		{
			if (!updateQuery)
			{
				ViewModelEvents.Instance.SlideElementUpdated.Publish(_model);
			}
			else
			{
				var qry = new BuildQuery(_model.Id, null, _model.BuildFilter);
				qry.Query(true);
			}
		}

		public void TeamProjectsChanged()
		{
			_model.TeamProjects.Clear();
			_model.TeamProjects.AddRange(this.TeamProjects.Where(tp => tp.IsSelected).Select(tp => tp.Title));
            
			UpdateModels(true);

            if (_model.TeamProjects.Count == 1) // Single team project selected
            {
                if (_model.Title == "Untitled") // means it has not been set at all
                    _model.Title = TeamProjects.First().Title;
            }
            UpdateModels(false);
		}

		public void BuildStatusChanged()
		{
			_model.Status = 0;
		
			foreach (BuildStatusNode bs in this.BuildStatus)
			{
				if (bs.IsSelected)
				{
					_model.Status |= bs.Status;
				}
			}

			UpdateModels(true);
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			_disposed = true;
		}

		#endregion // IDisposable Members
	}

	public class TeamProjectNode : ComboWithCheckBoxNode
	{
		#region Fields

		BuildMonitorMenuVm _parent;

		#endregion // Fields

		#region Constructors+

		public TeamProjectNode(BuildMonitorMenuVm parent, bool selected, string name)
			: base(name)
		{
			_parent = parent;
			this.IsSelected = selected;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Refactor to use ICommand binding instead...
		/// </summary>
		public override void Changed()
		{
			_parent.TeamProjectsChanged();
		}

		#endregion // Methods
	}

	public class BuildStatusNode : ComboWithCheckBoxNode
	{
		#region Fields

		BuildMonitorMenuVm _parent;

		#endregion // Fields

		#region Properties

		public BuildFilterStatus Status { get; private set; }

		#endregion // Properties

		#region Constructors+

		public BuildStatusNode(BuildMonitorMenuVm parent, BuildFilterStatus status, bool selected, string name)
			: base(name)
		{
			this.IsSelected = selected;
			_parent = parent;
			this.Status = status;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Refactor to use ICommand binding instead...
		/// </summary>
		public override void Changed()
		{
			_parent.BuildStatusChanged();
		}

		#endregion // Methods
	}

}
