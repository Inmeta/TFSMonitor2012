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
	/// Creates a slide with no slide elements.
	/// </summary>
	public class BlankSlideTemplateVm : SlideTemplateVm
	{
		#region Properties

		public override string DisplayName { get { return "Blank Slide"; } }

		#endregion // Properties

		#region Methods

		public override Slide CreateSlide()
		{
			return new Slide();
		}

		#endregion // Methods
	}

}