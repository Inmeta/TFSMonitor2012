namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;

	#endregion // Using
	
	public abstract class SlideshowMonitor
	{
		#region Properties

		public abstract int Code { get; }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Factory generating settings-code to class.
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static SlideshowMonitor ClassFromCode(int code, string selectedScreen)
		{
			if (code == new SlideshowMonitorSingle(selectedScreen).Code)
			{
				return new SlideshowMonitorSingle(selectedScreen);
			}
			if (code == new SlideshowMonitorDifferent().Code)
			{
				return new SlideshowMonitorDifferent();
			}

			// Default
			return new SlideshowMonitorSame();
		}

		#endregion // Methods
	}

	public class SlideshowMonitorSingle : SlideshowMonitor 
	{
		public override int Code { get { return 0; } }

		public string DeviceName { get; private set; }

		public SlideshowMonitorSingle(string deviceName)
		{
			this.DeviceName = deviceName;
		}

		public bool IsScreen(Screen s)
		{
			return this.DeviceName == s.DeviceName;
		}
	}

	public class SlideshowMonitorSame : SlideshowMonitor
	{
		public override int Code { get { return 1; } }
	}

	public class SlideshowMonitorDifferent : SlideshowMonitor
	{
		public override int Code { get { return 2; } }
	}
}
