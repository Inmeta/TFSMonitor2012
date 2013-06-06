namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	[Serializable()]
	public class WebPage : SlideElement
	{
		#region Properties

		public string Url { get; set; }

		public int RefreshInterval { get; set; }

		#endregion // Properties

		#region Constructor

		public WebPage() 
		{
			// Default values
			this.Url = @"http://www.osiris.no";
			this.RefreshInterval = 30;
		}

		#endregion // Constructor
	}
}
