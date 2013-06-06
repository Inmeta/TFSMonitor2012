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

	#endregion // Using

	public class NameDecorator<T>
	{
		#region Properties

		public string Name { get; private set; }
		public T Inner { get; set; }

		#endregion // Properties

		#region Constructors

		/// <summary>
		/// Create with inner null value
		/// </summary>
		/// <param name="msg"></nparam>
		public NameDecorator(string msg)
		{
			this.Name = msg;
		}

		/// <summary>
		/// Create with name and inner
		/// </summary>
		/// <param name="inner"></param>
		/// <param name="name"></param>
		public NameDecorator(T inner, string name)
		{
			this.Name = name;
			this.Inner = inner;
		}

		#endregion // Constructors
	}
}
