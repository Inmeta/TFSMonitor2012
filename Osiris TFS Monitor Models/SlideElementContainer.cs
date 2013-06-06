namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	public class SlideElementContainer 
	{
		#region Properties

		// Position of element in slide
		public Position Position { get; set; }

		public SlideElement Element { get; set; }

		#endregion // Properties

		#region Constructors

		[Obsolete("For serialization only", true)]
		public SlideElementContainer() { }

		public SlideElementContainer(SlideElement elem, Position pos)
		{
			this.Element = elem;
			this.Position = pos;
		}

		#endregion // Constructors


	}
}
