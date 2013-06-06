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
	using Osiris.Tfs.Monitor.Properties;
	using System.Timers;

	#endregion // Using

	public interface IFullscreenSlideView
	{
		void SetContent(SlideVm slide);
	}

	public class FullscreenSlideVm : ViewModelBase
	{
		private SlideVm _slide;

		public IFullscreenSlideView View { get; set; }

		public SlideVm Slide
		{
			get { return _slide; }
			set
			{
				if (value != _slide)
				{
					_slide = value;
					this.View.SetContent(_slide);
				}
			}
		}

		public FullscreenSlideVm(SlideVm slide)
		{
			_slide = slide;
		}
	}
}
