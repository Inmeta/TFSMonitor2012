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
	using System.Runtime.InteropServices;
	using Osiris.TFS.Monitor.Common;

	#endregion // Using

	public class SlidesRibbonGroupVm : ViewModelBase
	{
		#region Fields

		//Slide _slide;

		#endregion // Fields

		#region Properties

		public ObservableCollection<SlideTemplateVm> SlideTemplates { get; private set; }

		#endregion // Properties

		#region Constructors

		public SlidesRibbonGroupVm()
		{
			// Add slide templates
			this.SlideTemplates = new ObservableCollection<SlideTemplateVm>();
			this.SlideTemplates.Add(new BurndownChartTemplateVm());
			this.SlideTemplates.Add(new BuildMonitorTemplateVm());
            this.SlideTemplates.Add(new WebPageTemplateVm());

			ViewModelEvents.Instance.SlideSelected.Subscribe(OnSlideSelected);
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Called when zero or more SlideElements have been selected and we need
		/// to reflect this in UI.
		/// </summary>
		/// <param name="elements"></param>
		private void OnSlideSelected(Slide slide)
		{
			//_slide = slide;
		}

		#endregion // Methods
	}
}
