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
	/// Function-data and display settings for chart-point in chart-line.
	/// </summary>
	public class LineChartPoint
	{
		#region Properties

		public double X { get; private set; }
		public double Y { get; private set; }
		public bool Enable { get; private set; }
		public bool Highlight { get; private set; }
		public Color HighlightFill { get; set; }

		#endregion // Properties

		#region Constructors

		/// <summary>
		/// Line-point
		/// </summary>
		/// <param name="value">Y-value</param>
		/// <param name="enable">Enable</param>
		/// <param name="highlight">Highlight this point with circle?</param>
		public LineChartPoint(double x, double y, bool enable, bool highlight, Color? highlightFill)
		{
			this.X = x;
			this.Y = y;
			this.Enable = enable;
			this.Highlight = highlight;
			this.HighlightFill = (highlightFill == null) ? Colors.White : highlightFill.Value;
		}

		#endregion // Constructors
	}
}
