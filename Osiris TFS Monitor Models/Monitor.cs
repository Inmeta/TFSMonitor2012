using System.Collections;
using System.Diagnostics.Contracts;
using Osiris.TFS.Monitor.Common;

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
	using System.Xml.Serialization;
	using System.Runtime.InteropServices;

	#endregion // Using

	[Serializable()]
	public class Monitor
	{
		#region Properties

		public int Number { get; set; }

		[XmlIgnore]
		public string Name { get; private set; }

		[XmlIgnore]
		public string MonitorInfo { get; private set; }

		#endregion // Properties

		[Obsolete("For use with serialization only")]
		public Monitor() { }

		public Monitor(int number)
		{
			this.Number = number;
			InitName();
			InitInfo();
		}

		private void InitName()
		{
			// Name
			if (this.Number == 0)
			{
				this.Name = "Primary";
			}
			else if (this.Number == 1)
			{
				this.Name = "Secondary";
			}
			else
			{
				this.Name = "Monitor " + (this.Number + 1).ToString();
			}
		}

		private void InitInfo()
		{
			var screens = Screen.AllScreens;
			if (screens.Count() > this.Number)
			{
				this.MonitorInfo = screens[this.Number].MonitorName();
			}
		}
	}

	public class MonitorCollection : IEnumerable<Monitor>
	{
		private List<Monitor> _monitors = new List<Monitor>();

		public MonitorCollection()
		{
		}

		public MonitorCollection(IEnumerable<Monitor> monitors)
		{
			Contract.Requires(monitors != null);

			_monitors.AddRange(monitors.Select(m => new Monitor(m.Number)));
		}

		public IEnumerator<Monitor> GetEnumerator()
		{
			return _monitors.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void LoadAvailable()
		{
			_monitors.Clear();
	
			var screens = Screen.AllScreens;
			int number = 0;
			foreach (var screen in screens)
			{
				_monitors.Add(new Monitor(number++));
			}
		}

		public static MonitorCollection Available()
		{
			var result = new MonitorCollection();
			result.LoadAvailable();
			return result;
		}
	}
}
