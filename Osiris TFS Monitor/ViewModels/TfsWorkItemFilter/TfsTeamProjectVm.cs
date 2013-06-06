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

	#endregion // Using

	public class TeamProjectItem
	{
		public string DisplayName { get; private set; }
		public string TeamProject { get; private set; }

		public TeamProjectItem(string dispName, string tp)
		{
			this.DisplayName = dispName;
			this.TeamProject = tp;
		}
	}

	public class TfsTeamProjectVm : ViewModelBase
	{
		private readonly TfsWorkItemFilterVm _filter;
		TeamProjectItem _selectedTeamProject;
		bool _isTeamProjectsEnabled = false;
		bool _isLoaded = true;
		SlideElementCollection _elements;
		List<string> _teamProjects;

		#region Properties

		public ObservableCollection<TeamProjectItem> TeamProjects { get; private set; }

		public bool IsTeamProjectsEnabled
		{
			get { return _isTeamProjectsEnabled; }
			set
			{
				_isTeamProjectsEnabled = value;
				RaisePropertyChanged(() => IsTeamProjectsEnabled);
			}
		}

		public TeamProjectItem SelectedTeamProject
		{
			get { return _selectedTeamProject; }
			set
			{
				_selectedTeamProject = value;
				if (value != null && value.TeamProject != null)
				{
					UpdateModels();
				}
				RaisePropertyChanged(() => SelectedTeamProject);
			}
		}

		#endregion // Properties

		#region Constructors

		public TfsTeamProjectVm(TfsWorkItemFilterVm filter)
		{
			_filter = filter;
			this.TeamProjects = new ObservableCollection<TeamProjectItem>();
			ViewModelEvents.Instance.TeamProjectChanged.Subscribe(OnTeamProjectChanged);
		}

		#endregion // Constructors

		#region Methods

		private void OnTeamProjectChanged(SlideElement elem)
		{
			if (this.TeamProjects == null || _elements == null)
			{
				return;
			}

			if (elem.Id != _elements.First().Id)
			{
				return;
			}

			var selected = this.TeamProjects.FirstOrDefault(tp => tp.TeamProject == elem.TfsWorkItemFilter.TeamProjectName);
			this.SelectedTeamProject = selected;
		}

		public void UpdateModels()
		{
			bool modified = false;
			foreach (var elem in _elements)
			{
				// Team Projected changed?
				if (elem.TfsWorkItemFilter.TeamProjectName != _selectedTeamProject.TeamProject)
				{
					modified = true;
					elem.TfsWorkItemFilter.TeamProjectName = _selectedTeamProject.TeamProject;
				}
			}

			if (modified)
			{
				ViewModelEvents.Instance.TeamProjectChanged.Publish(_elements.First());
			}
		}

		private void OnTeamProjectsQueryCompleted(TeamProjectQuery qry)
		{
			if (!_isLoaded || _elements == null)
			{
				return;
			}

			this.TeamProjects.Clear();

			if (qry.Exception != null)
			{
				_teamProjects = null;
				this.TeamProjects.Add(new TeamProjectItem("Request failed", null));
			}
			else
			{
				_teamProjects = new List<string>();
				_teamProjects.AddRange(qry.Results.Select(tp => tp.Name));
				this.TeamProjects.Clear();
				TeamProjectItem selected = null;
				var filter = _elements.First().TfsWorkItemFilter;
				foreach (string tp in _teamProjects)
				{
					var tp2 = new TeamProjectItem(tp, tp);
					this.TeamProjects.Add(tp2);
					if (filter != null && filter.TeamProjectName != null)
					{
						if (tp2.TeamProject == filter.TeamProjectName)
						{
							selected = tp2;
						}
					}
				}

				if (this.TeamProjects.Count <= 0)
				{
					this.TeamProjects.Add(new TeamProjectItem("No teamprojects found", null));
				}
				RaisePropertyChanged("TeamProjects");

				this.SelectedTeamProject = (selected == null) ? this.TeamProjects.First() : selected;
			}

			this.IsTeamProjectsEnabled = (qry.Exception == null);
			UpdateFilters();
		}

		private void UpdateFilters()
		{
			if (_filter != null)
			{
				_filter.TeamProjectSelected(this.SelectedTeamProject != null ? this.SelectedTeamProject.TeamProject : null);
			}
		}

		public void Invalidate(SlideElementCollection elements)
		{
			_elements = elements;

			// Clear
			this.TeamProjects.Clear();
			this.IsTeamProjectsEnabled = false;

			// No model return
			if (_elements == null)
			{
				return;
			}

			this.TeamProjects.Add(new TeamProjectItem("Loading...", null));
			this.SelectedTeamProject = this.TeamProjects.First();

			// Query TFS
			var qry = new TeamProjectQuery(0, null, OnTeamProjectsQueryCompleted);
			qry.Query(false);
		}
		
		private string GetTeamProject(string tpName)
		{
			if (string.IsNullOrEmpty(tpName))
			{
				return null;
			}

			return _teamProjects.First(tp => tp == tpName);
		}

		#endregion // Methods

		#region IDisposable Members

		public void Unload()
		{
			ViewModelEvents.Instance.TeamProjectChanged.Unsubscribe(OnTeamProjectChanged);
			_teamProjects = null;
			_isLoaded = false;
		}

		#endregion // IDisposable Members
	}
}
