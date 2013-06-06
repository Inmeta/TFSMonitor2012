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

	public class BurndownSlidePropVM : SlidePropVM
	{
		#region Fields

		ObservableCollection<NameDecorator<TeamProject>> _teamProjects;
		ObservableCollection<NameDecorator<WorkItem>> _iterations;
		NameDecorator<TeamProject> _selectedTeamProject;
		NameDecorator<WorkItem> _selectedIteration;
		bool _isTeamProjectsEnabled = false;
		bool _isIterationsEnabled = false;
		int _iterationsQueryCount = 0;
		BurndownSlide _slide;
		bool _disposed = false;

		#endregion // Fields

		#region Properties

		protected override Slide Slide { get { return _slide; } }

		public bool ChangeEnabled { get; set; }

		public ObservableCollection<NameDecorator<TeamProject>> TeamProjects
		{
			get
			{
				if (_teamProjects == null)
				{
					_teamProjects = new ObservableCollection<NameDecorator<TeamProject>>();
					Invalidate();
				}
				return _teamProjects;
			}
		}

		public bool IsTeamProjectsEnabled
		{
			get
			{
				return _isTeamProjectsEnabled;
			}
			set
			{
				_isTeamProjectsEnabled = value;
				RaisePropertyChanged(() => IsTeamProjectsEnabled);
			}
		}

		public bool IsIterationsEnabled
		{
			get
			{
				return _isIterationsEnabled;
			}
			set
			{
				_isIterationsEnabled = value;
				RaisePropertyChanged(() => IsIterationsEnabled);
			}
		}

		public ObservableCollection<NameDecorator<WorkItem>> Iterations
		{
			get
			{
				return _iterations;
			}
		}

		public NameDecorator<TeamProject> SelectedTeamProject
		{
			get
			{
				return _selectedTeamProject;
			}
			set
			{
				_selectedTeamProject = value;
				RaisePropertyChanged(() => SelectedTeamProject);
				UpdateIterations();
			}
		}

		public NameDecorator<WorkItem> SelectedIteration
		{
			get
			{
				return _selectedIteration;
			}
			set
			{
				_selectedIteration = value;
				RaisePropertyChanged(() => SelectedIteration);
				((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
			}
		}

		public int Manpower
		{
			get { return _slide.Manpower; }
			set
			{
				_slide.Manpower = value;
				RaisePropertyChanged(() => Manpower);
			}
		}

		public int UpdateInterval
		{
			get { return _slide.UpdateInterval; }
			set
			{
				_slide.UpdateInterval = value;
				RaisePropertyChanged(() => UpdateInterval);
			}
		}

		#endregion // Properties

		#region Events

		public delegate void CloseEvent();

		#endregion // Events

		#region Constructors

		public BurndownSlidePropVM(ISlidePropView view)
			: base(view)
		{
			// New slide
			//if (previousSlide == null)
			{
				_slide = new BurndownSlide("Untitled");
				this.ChangeEnabled = false;
			}
			/*else
			{
				_slide = new BurndownSlide(previousSlide);
				this.ChangeEnabled = true;
				_teamProjects = new ObservableCollection<NameDecorator<TeamProject>>();
				_teamProjects.Add(new NameDecorator<TeamProject>(previousSlide.TeamProject));
				this.SelectedTeamProject = _teamProjects.First();
				_iterations = new ObservableCollection<NameDecorator<WorkItem>>();
				_iterations.Add(new NameDecorator<WorkItem>(previousSlide.IterationPath));
				this.SelectedIteration = _iterations.First();

			}*/

			//UpdateCanSave();		
		}

		#endregion // Constructors

		#region Methods

		private void UpdateIterations()
		{
			if (this.SelectedTeamProject.Inner != null)
			{
				this.IsIterationsEnabled = false;
				TfsService.Instance.Query(new IterationsQuery(Application.Current.Dispatcher, this.SelectedTeamProject.Inner, this.IterationsQueryCompleted,
					this.IterationsQueryFailed));
				_iterationsQueryCount++;

				if (_iterations == null)
				{
					_iterations = new ObservableCollection<NameDecorator<WorkItem>>();
				}
				else
				{
					_iterations.Clear();
				}
				_iterations.Add(new NameDecorator<WorkItem>("Requesting iterations from TFS..."));
				RaisePropertyChanged("Iterations");
				this.SelectedIteration = this.Iterations.First();
			}
		}

		public void TeamProjectsQueryCompleted(TeamProjectCollection teamProjects)
		{
			if (_disposed)
			{
				return;
			}

			this.TeamProjects.Clear();
			foreach (TeamProject tp in teamProjects)
			{
				this.TeamProjects.Add(new NameDecorator<TeamProject>(tp, tp.Name));
			}
			if (this.TeamProjects.Count <= 0)
			{
				this.TeamProjects.Add(new NameDecorator<TeamProject>("No teamprojects found"));
			}
			RaisePropertyChanged("TeamProjects");
			this.SelectedTeamProject = this.TeamProjects.First();
			this.IsTeamProjectsEnabled = true;
		}

		public void TeamProjectsQueryFailed(Exception ex)
		{
			if (_disposed)
			{
				return;
			}

			this.TeamProjects.Clear();
			this.TeamProjects.Add(new NameDecorator<TeamProject>("Unable to query TFS"));
			RaisePropertyChanged("TeamProjects");
			this.SelectedTeamProject = this.TeamProjects.First();
			this.IsTeamProjectsEnabled = false;

			MessageBox.Show("Unable query TFS for TeamProjects.\r\n\r\nReason:\r\n" + ex.Message, "Osiris TFS Monitor",
				MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		private void IterationsQueryCompleted(IterationCollection iterations)
		{
			if (_disposed)
			{
				return;
			}

			_iterationsQueryCount--;
			Debug.Assert(_iterationsQueryCount >= 0);
			if (_iterationsQueryCount == 0)
			{
				this.Iterations.Clear();
				foreach (WorkItem wi in iterations)
				{
					// Remove TeamProject part of iterationpath
					string name = wi.IterationPath;
					if (wi.IterationPath.StartsWith(SelectedTeamProject.Name))
					{
						name = name.Remove(0, SelectedTeamProject.Name.Count());
						if (name.StartsWith(@"\"))
						{
							name = name.Remove(0, 1);
						}
						if (name.Length == 0)
						{
							name = "(Blank)";
						}
					}
					this.Iterations.Add(new NameDecorator<WorkItem>(wi, name));
					this.IsIterationsEnabled = (_iterationsQueryCount == 0) ? true : false;
				}
				if (this.Iterations.Count <= 0)
				{
					_iterations.Add(new NameDecorator<WorkItem>("No iterations found"));
				}
				this.SelectedIteration = this.Iterations.First();
			}
		}

		public void IterationsQueryFailed(Exception ex)
		{
			if (_disposed)
			{
				return;
			}

			this.Iterations.Clear();
			this.Iterations.Add(new NameDecorator<WorkItem>("Unable to query TFS"));
			RaisePropertyChanged("Iterations");
			this.SelectedIteration = this.Iterations.First();

			MessageBox.Show("Unable query TFS for TeamProjects.\r\n\r\nReason:\r\n" + ex.Message, "Osiris TFS Monitor",
				MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		protected override bool CanExecuteOk()
		{
			// Name filled out?
			if (!this.ChangeEnabled)
			{
				// Iteration selected?
				if (this.SelectedIteration == null)
				{
					return false;
				}
				if (this.SelectedIteration.Inner == null)
				{
					return false;
				}
			}

			// All requirements ok
			return true;
		}

		protected override void Save()
		{
			if (!this.ChangeEnabled)
			{
				_slide.TeamProject = this.SelectedTeamProject.Inner.Name;
				_slide.IterationId = this.SelectedIteration.Inner.Id;
				_slide.IterationPath = this.SelectedIteration.Inner.IterationPath;
			}

			//base.Save();
		}

		public void Change()
		{
			this.ChangeEnabled = false;
			Invalidate();
		}

		void Invalidate()
		{
			TfsService.Instance.Query(new TeamProjectsQuery(Application.Current.Dispatcher, this.TeamProjectsQueryCompleted,
				this.TeamProjectsQueryFailed));
			_teamProjects.Clear();
			_teamProjects.Add(new NameDecorator<TeamProject>("Requesting teamprojects from TFS..."));
			this.SelectedTeamProject = _teamProjects.First();
			if (_iterations != null)
			{
				_iterations.Clear();
			}

		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			_disposed = true;
		}

		#endregion // IDisposable Members
	}
}
