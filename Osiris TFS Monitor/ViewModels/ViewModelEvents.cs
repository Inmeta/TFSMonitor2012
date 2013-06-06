namespace Osiris.Tfs.Monitor
{
	#region Using
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Practices.Unity;
	using Microsoft.Practices.Composite.Presentation.Events;
	using Osiris.Tfs.Monitor.Models;
using System.Windows;
	using System.Windows.Media;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using Osiris.Tfs.Report;

	#endregion // Using

	public class SlideContentDisplayedArgs
	{
		public Slide Slide { get; private set; }
		public Visual Elem { get; private set; }


		public SlideContentDisplayedArgs(Slide slide, Visual elem)
		{
			this.Slide = slide;
			this.Elem = elem;
		}
	}

	public class ViewModelEvents
	{
		#region Fields

		static ViewModelEvents _instance;

		#endregion // Fields

		#region Properties

		public static ViewModelEvents Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ViewModelEvents();
				}
				return _instance;
			}
		}

		public CompositePresentationEvent<Slide> NewSlide { get; private set; }
		public CompositePresentationEvent<Slide> NewSlideAdded { get; private set; }
		public CompositePresentationEvent<TfsMonitorDocument> NewDocument { get; private set; }
		public CompositePresentationEvent<Slide> SlideSelected { get; private set; }
		public CompositePresentationEvent<SlideElement> SlideElementUpdated { get; private set; }
		public CompositePresentationEvent<Slide> SlideUpdated { get; private set; }
		public CompositePresentationEvent<SlideElementCollection> SlideElementSelected { get; private set; }
		public CompositePresentationEvent<SlideContentDisplayedArgs> SlideContentDisplayed { get; private set; }
		public CompositePresentationEvent<Slide> DeleteSlide { get; private set; }
		public CompositePresentationEvent<Slide> SlideDeleted { get; private set; }
		public CompositePresentationEvent<TfsMonitorSettings> SettingsUpdated { get; private set; }
		public CompositePresentationEvent<SlideElement> TeamProjectChanged { get; private set; }
		public CompositePresentationEvent<SlideElement> TfsIterationChanged { get; private set; }
		public CompositePresentationEvent<IQuery> QueryStarted { get; private set; }
		public CompositePresentationEvent<BuildQuery> BuildQueryCompleted { get; private set; }
		public CompositePresentationEvent<TwitterQuery> TwitterQueryCompleted { get; private set; }
		public CompositePresentationEvent<BurndownQuery> BurndownQueryCompleted { get; private set; }
		public CompositePresentationEvent<TeamProjectQuery> TeamProjectsQueryCompleted { get; private set; }
		public CompositePresentationEvent<WorkItemFilterQuery> WorkItemFilterQueryCompleted { get; private set; }
		public CompositePresentationEvent<WorkItemFieldQuery> WorkItemFieldQueryCompleted { get; private set; }
		public CompositePresentationEvent<IterationWiQuery> IterationWiQueryCompleted { get; private set; }
		public CompositePresentationEvent<SlideElement> UpdateIntervalChanged { get; private set; }
		public CompositePresentationEvent<string> FileOpen { get; private set; }
		public CompositePresentationEvent<string> RecentFilesChanged { get; private set; }
		public CompositePresentationEvent<Slide> RefreshSlide { get; private set; }

		#endregion // Properties

		#region Constructors

		private ViewModelEvents()
		{
			this.NewSlide = new CompositePresentationEvent<Slide>();
			this.NewSlideAdded = new CompositePresentationEvent<Slide>();
			this.NewDocument = new CompositePresentationEvent<TfsMonitorDocument>();
			this.SlideSelected = new CompositePresentationEvent<Slide>();
			this.SlideElementUpdated = new CompositePresentationEvent<SlideElement>();
			this.SlideUpdated = new CompositePresentationEvent<Slide>();
			this.SlideElementSelected = new CompositePresentationEvent<SlideElementCollection>();
			this.SlideContentDisplayed = new CompositePresentationEvent<SlideContentDisplayedArgs>();
			this.DeleteSlide = new CompositePresentationEvent<Slide>();
			this.SlideDeleted = new CompositePresentationEvent<Slide>();
			this.SettingsUpdated = new CompositePresentationEvent<TfsMonitorSettings>();
			this.TeamProjectChanged = new CompositePresentationEvent<SlideElement>();
			this.QueryStarted = new CompositePresentationEvent<IQuery>();
			this.BuildQueryCompleted = new CompositePresentationEvent<BuildQuery>();
			this.TwitterQueryCompleted = new CompositePresentationEvent<TwitterQuery>();
			this.BurndownQueryCompleted = new CompositePresentationEvent<BurndownQuery>();
			this.TfsIterationChanged = new CompositePresentationEvent<SlideElement>();
			this.TeamProjectsQueryCompleted = new CompositePresentationEvent<TeamProjectQuery>();
			this.WorkItemFilterQueryCompleted = new CompositePresentationEvent<WorkItemFilterQuery>();
			this.WorkItemFieldQueryCompleted = new CompositePresentationEvent<WorkItemFieldQuery>();
			this.IterationWiQueryCompleted = new CompositePresentationEvent<IterationWiQuery>();
			this.UpdateIntervalChanged = new CompositePresentationEvent<SlideElement>();
			this.FileOpen = new CompositePresentationEvent<string>();
			this.RecentFilesChanged = new CompositePresentationEvent<string>();
			this.RefreshSlide = new CompositePresentationEvent<Slide>();
		}

		#endregion // Constructors
	}
}
