namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Xml.Serialization;

	#endregion // Using

	[Serializable()]
	public class BurndownSlide : Slide
	{
		#region Properties

		public string TeamProject { get; set; }

		public int? IterationId { get; set; }

		public string IterationPath { get; set; }

		public int Manpower { get; set; }

		public int UpdateInterval { get; set; }

		[XmlIgnore]
		public override SlideType SlideType { get { return new BurndownSlideType(); } }

		#endregion // Properties

		#region Constructors

		public BurndownSlide() { }

		public BurndownSlide(string name) : base(name) 
		{ 
			// Default value
			this.UpdateInterval = 60; // 1 minute
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="slide"></param>
		public BurndownSlide(BurndownSlide slide) : base(slide) 
		{
			this.TeamProject = slide.TeamProject;
			this.IterationId = slide.IterationId;
			this.IterationPath = slide.IterationPath;
			this.Manpower = slide.Manpower;
			this.UpdateInterval = slide.UpdateInterval;
		}

		#endregion // Constructors
	}
}
