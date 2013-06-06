using System.Collections.Specialized;

namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.ObjectModel;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Monitor.Properties;
	using System.Diagnostics;

	#endregion // Using

	/// <summary>
	/// Options for Application
	/// </summary>
	public class SlideshowOptionsVm : OptionVm
	{
		//NameDecorator<Screen> _selectedScreen;

		#region Properties

		//public TfsMonitorSettings Model { get { return _parent.Model; } }

		public ObservableNodeList Monitors { get; private set; }

		public bool TurnOffScreenSaver
		{
			get { return this.Parent.Model.SlideshowTurnOffScreenSaver; }
			set { this.Parent.Model.SlideshowTurnOffScreenSaver = value; }
		}


		#endregion // Properties

		#region Constructors

		public SlideshowOptionsVm(ApplicationOptionsVm parent)
			: base(parent, "Slideshow", "Settings for SlideShow.",
			@"pack://application:,,,/Resources/Icons/SlideShowPlay32x32.png")
		{
			this.Monitors = new ObservableNodeList();
			var monitors = MonitorCollection.Available();
			foreach (var m in monitors)
			{
				this.Monitors.Add(new MonitorItem(m, this.Parent.Model.SlideshowMonitors.SingleOrDefault(m2 => m2.Number == m.Number) != null, new MonitorItemChangedDelegate(MonitorChanged)));
			}
		}

		private void MonitorChanged()
		{
			this.Parent.Model.SlideshowMonitors = new MonitorCollection(this.Monitors.Where(m => m.IsSelected).Select(m => new Monitor(((MonitorItem)m).Number)));
		}

		#endregion // Constructors
	}


}
