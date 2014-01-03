using System.Reflection;

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
	using System.Collections.ObjectModel;

	#endregion // Using

	public interface IApplicationRibbonView
	{
		void SetCustomTab(object type);
	}

	public class ApplicationRibbonVm : ViewModelBase
	{
		ApplicationRibbonView _view;
		Slide _activeSlide;

		public string DocumentTitle { get { return "Osiris TFS Monitor"; } }

		public WebPageType WebPageType { get { return new WebPageType(); } }

		public TwitterType TwitterType { get { return new TwitterType(); } }

		public BurndownChartType BurndownChartType { get { return new BurndownChartType(); } }

		public BuildMonitorType BuildMonitorType { get { return new BuildMonitorType(); } }

		public TaskManagerType TaskManagerType { get { return new TaskManagerType(); } }

		public ObservableCollection<string> RecentFiles { get; private set; }

		public bool? DesignMode 
		{
			get
			{
				if (_activeSlide == null)
				{
					return null;
				}
				return _activeSlide.DesignMode;
			}
			set
			{
				if (_activeSlide == null)
				{
					return;
				}
				_activeSlide.DesignMode = value.Value;
			}
		}

        

        public object CustomTabVm { get; set; }

		public ICommand OpenRecentFile { get; set; }

		#region Constructors

		public ApplicationRibbonVm(ApplicationRibbonView view)
		{
			_view = view;

			this.RecentFiles = new ObservableCollection<string>();
			OnRecentFilesChanged("");
			this.OpenRecentFile = new DelegateCommand<string>(OnOpenRecentFile);
			ViewModelEvents.Instance.SlideSelected.Subscribe(OnSlideSelected);
			ViewModelEvents.Instance.SlideUpdated.Subscribe(OnSlideUpdated);
			ViewModelEvents.Instance.SlideElementSelected.Subscribe(OnSlideElementSelected);
			ViewModelEvents.Instance.RecentFilesChanged.Subscribe(OnRecentFilesChanged);
		}

		#endregion // Constructors

		#region Methods

		private void OnSlideSelected(Slide slide)
		{
			_activeSlide = slide;

			if (slide == null)
			{
				_view.SetCustomTab(null);
			}

			RaisePropertyChanged(() => DesignMode);
		}

		private void OnSlideUpdated(Slide slide)
		{
			RaisePropertyChanged(() => DesignMode);
		}

		private void OnSlideElementSelected(SlideElementCollection elements)
		{
			if (elements.Count() > 0)
			{
                this.CustomTabVm = SlideElementType.CreateMenuViewModel(elements.First());
                _view.SetCustomTab(this.CustomTabVm);
			}
			else
			{
				_view.SetCustomTab(null);
			}
		}
		
		private void OnOpenRecentFile(string fileName)
		{
			ViewModelEvents.Instance.FileOpen.Publish(fileName);
		}

		private void OnRecentFilesChanged(string fileName)
		{
			this.RecentFiles.Clear();
			var settings = new TfsMonitorSettings();
			settings.Load(Settings.Default);
			foreach (var recentFile in settings.RecentFiles)
			{
				this.RecentFiles.Add(recentFile);				
			}
		}

		#endregion // Methods
	}
}
