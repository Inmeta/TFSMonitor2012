namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	public class BlankSlideTemplate : SlideTemplate
	{
		#region Constrcutors

		public BlankSlideTemplate()	: base("Blank Slide", "Blank Slide.") 
		{
		}

		#endregion // Constrcutors

		#region Methods

		public override Slide CreateSlide(string name)
		{
			return null; 
		}

		#endregion // Methods

	}
}
