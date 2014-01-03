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

	public class BuildMonitorTemplateVm : SlideTemplateVm
	{
		public override string DisplayName { get { return "Build Monitor"; } }

		public override Slide CreateSlide()
		{
			var elem = new BuildMonitor();
			var container = new SlideElementContainer(elem, new Position(0,0,1,1));
			var s = new Slide();
			s.SlideElements.Add(container);
			return s;
		}
	}

    public class WebPageTemplateVm : SlideTemplateVm
    {
        public override string DisplayName { get { return "Web Page"; } }

        public override Slide CreateSlide()
        {
            var elem = new WebPage();
            var container = new SlideElementContainer(elem, new Position(0, 0, 1, 1));
            var s = new Slide();
            s.SlideElements.Add(container);
            return s;
        }
    }

}