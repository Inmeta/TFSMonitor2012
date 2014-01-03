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

	public class TfsWorkItemFilterVm : ViewModelBase
	{
		#region Fields

		TfsWorkItemFilter _model;
		SlideElementCollection _elements;
		bool _disposed = false;
		WorkItemFilterQuery _wiFilterQry;

		#endregion // Fields

		#region Properties

		public TfsWorkItemFilter Model { get { return _model; } }

		public TfsTeamProjectVm TeamProjectVm { get; set; }

		public TfsAreaVm TfsAreaVm { get; set; }

		public TfsIteration2Vm TfsIterationVm { get; set; }

		public TfsAssignedToVm TfsAssignedToVm { get; set; }

		public TfsWiTypeVm TfsWiTypeVm { get; set; }

		public TfsWiStateVm TfsWiStateVm { get; set; }

		#endregion // Properties

		#region Constructors

		public TfsWorkItemFilterVm()
		{
			this.TeamProjectVm = new TfsTeamProjectVm(this);
			this.TfsAreaVm = new TfsAreaVm();
			this.TfsIterationVm = new TfsIteration2Vm(this);
			this.TfsAssignedToVm = new TfsAssignedToVm(this);
			this.TfsWiTypeVm = new TfsWiTypeVm(this);
			this.TfsWiStateVm = new TfsWiStateVm(this);

			ViewModelEvents.Instance.SlideElementSelected.Subscribe(OnSlideElementSelected);
			ViewModelEvents.Instance.TeamProjectChanged.Subscribe(OnTeamProjectChanged);
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Called when zero or more SlideElements have been selected and we need
		/// to reflect this in UI.
		/// </summary>
		/// <param name="elements"></param>
		private void OnSlideElementSelected(SlideElementCollection elements)
		{
			_model = elements.TfsWorkItemFilter();
			_elements = (_model == null) ? null : elements;
			Invalidate();
		}

		private void OnTeamProjectChanged(SlideElement elem)
		{
			if (_elements == null || _elements.Count() == 0  || _elements.First().Id != elem.Id)
			{
				return;
			}
			TeamProjectSelected(elem.TfsWorkItemFilter.TeamProjectName);
		}

		private void Invalidate()
		{
			this.TeamProjectVm.Invalidate(_elements);
		}

		public void TeamProjectSelected(string teamProject)
		{
			if (teamProject == null)
			{
				this.TfsAreaVm.Invalidate(null);
				this.TfsIterationVm.Invalidate(null);
				this.TfsWiTypeVm.Invalidate(null);
				this.TfsWiStateVm.Invalidate(null);
				this.TfsAssignedToVm.Invalidate(null);
				return;
			}

			this.TfsAreaVm.DisplayLoadingStatus();
			this.TfsIterationVm.DisplayLoadingStatus();
			this.TfsWiTypeVm.DisplayLoadingStatus();
			this.TfsWiStateVm.DisplayLoadingStatus();
			this.TfsAssignedToVm.DisplayLoadingStatus();

			if (_wiFilterQry != null)
			{
				ViewModelEvents.Instance.WorkItemFilterQueryCompleted.Unsubscribe(WorkItemFilterQueryCompleted);
			}

			// Query TFS
			var qryArg = new WorkItemFilterQueryArg(teamProject);
			_wiFilterQry = new WorkItemFilterQuery(_model.Id, null, qryArg, WorkItemFilterQueryCompleted);
			_wiFilterQry.Query(false);

			Model.TeamProjectName = teamProject;
			UpdateModels();
		}

		public void WorkItemFilterQueryCompleted(WorkItemFilterQuery qry)
		{
			if (_disposed)
			{
				return;
			}

			if (qry.Exception != null)
			{
				this.TfsAreaVm.Invalidate(null);
				this.TfsIterationVm.Invalidate(null);
				this.TfsWiTypeVm.Invalidate(null);
				this.TfsWiStateVm.Invalidate(null);
				this.TfsAssignedToVm.Invalidate(null);

				// Move to common...
				MessageBox.Show("Unable to query WorkItem Filters:\r\n\r\n" + qry.Exception.Message + "\r\n\r\nStackstrace:\r\n" + qry.Exception.StackTrace, "Osiris TFS Monitor", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			else
			{
				this.TfsAreaVm.Invalidate(qry.Results.AreaFilter);
				this.TfsIterationVm.Invalidate(qry.Results.IterationFilter);
				this.TfsWiTypeVm.Invalidate(qry.Results.WiTypeFilter);
				this.TfsWiStateVm.Invalidate(qry.Results.WiStateFilter);
				this.TfsAssignedToVm.Invalidate(qry.Results.AssignedToFilter);
			}
		}

		public void UpdateModels()
		{
			if (_elements == null)
			{
				return;
			}
			foreach (var elem in _elements)
			{
				elem.TfsWorkItemFilter.Copy(_model);

				// Iteration changed?
				if (elem.TfsWorkItemFilter.IterationFilter.IterationId != _model.IterationFilter.IterationId)
				{
					ViewModelEvents.Instance.TfsIterationChanged.Publish(elem);
				}
			}
		}

		#endregion // Methods
	}
}
