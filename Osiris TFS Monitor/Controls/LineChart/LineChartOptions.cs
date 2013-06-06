namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Media;
using System.Windows.Media.Effects;

	#endregion // Using

	public class LineChartOptions
	{
		#region Properties

		public Typeface TextTypeface { get; set; }
		public int FontSize { get; set; }
		public Size AxisLineThickness { get; set; }
		public SolidColorBrush AxisColor { get; set; }
		public SolidColorBrush BackgroundTicksBrush { get; set; }
		public SolidColorBrush TextColor { get; set; }
		public BitmapEffect LineEffect { get; set; }

		#endregion // Properties

		#region Constructors

		/// <summary>
		/// Default values
		/// </summary>
		public LineChartOptions()
		{
			this.FontSize = 14;
			this.TextTypeface = new Typeface("Arial");
			this.AxisLineThickness = new Size(1, 1);
			this.AxisColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
			this.BackgroundTicksBrush = new SolidColorBrush(Color.FromRgb(38, 38, 38));
			this.TextColor = Brushes.Black;
			var effect = new BevelBitmapEffect();
			effect.BevelWidth = 1;
			effect.Freeze();
			this.LineEffect = null; // effect;
		}

		#endregion // Constructors
	}
}
