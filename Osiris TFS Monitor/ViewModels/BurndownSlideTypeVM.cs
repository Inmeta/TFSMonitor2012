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

	public class BurndownSlideTypeVM : SlideTypeVM
	{
		public override string DisplayName { get { return "Burndown Graph"; } }

		public override SlidePropVM CreateSlidePropVM(ISlidePropView view)
		{
			return new BurndownSlidePropVM(view);
		}
	}
}