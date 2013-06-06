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

	/// <summary>
	/// Creates a slide with burndown chart.
	/// </summary>
	public class TaskManagerTemplateVm : SlideTemplateVm
	{
		#region Properties

		public override string DisplayName { get { return "Task Manager"; } }

		#endregion // Properties

		#region Methods

		public override Slide CreateSlide()
		{
			var elem = new TaskManager();
			var container = new SlideElementContainer(elem, new Position(0,0,1,1));
			var s = new Slide();
			s.SlideElements.Add(container);
			return s;
		}

		#endregion // Methods
	}

}