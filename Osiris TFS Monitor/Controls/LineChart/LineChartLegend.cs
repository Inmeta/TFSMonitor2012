namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Media;

	#endregion // Using
	
	/// <summary>
	/// Holds data for legend beeing displayed beside the graf.
	/// </summary>
	public class LineChartLegend
	{
		#region Properties

		public string Name { get; private set; }
		public string Color { get; private set; }

		#endregion // Properties

		#region Constructors

		public LineChartLegend(string name, Color color)
		{
			this.Name = name;
			this.Color = color.ToString();
		}

		#endregion // Constructors
	}
}
