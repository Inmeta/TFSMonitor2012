namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	public class BurndownSlideType : SlideType
	{
		#region Constrcutors

		public BurndownSlideType() : base("Burndown chart", "Displays burndown chart.") 
		{
		}

		#endregion // Constrcutors

		#region Methods

		public override Slide CreateSlide(string name)
		{
			return new BurndownSlide(name);
		}

		#endregion // Methods

	}
}
