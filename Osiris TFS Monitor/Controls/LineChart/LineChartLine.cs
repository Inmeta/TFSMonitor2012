namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Media;
using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Shapes;
	using System.Diagnostics;

	#endregion // Using
	
	/// <summary>
	/// Function-data and display settings for chart-line.
	/// </summary>
	public class LineChartLine : List<LineChartPoint>
	{
		#region Properties

		public Color Color { get; private set; }
		public string Legend { get; private set; }
		public double Thickness { get; private set; }
		public bool Filled { get; private set; }


		#endregion // Properties

		#region Constructors

		public LineChartLine(bool filled, string legend, Color color)
		{
			this.Legend = legend;
			this.Color = color;
			this.Thickness = 8;
			this.Filled = filled;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Draw chart line on canvas.
		/// </summary>
		/// <param name="chartData"></param>
		public void Draw(Canvas canvas, Point origo, Point factor, int xIncr)
		{
			if (this.Count <= 0)
			{
				return;
			}

			if (Filled)
			{
				DrawFilled(canvas, origo, factor);
			}

			var pl = new Polyline();
			pl.Stroke = new SolidColorBrush(this.Color);
			pl.StrokeThickness = this.Thickness;
			pl.Opacity = 0.75;

			

			pl.Points = GetPoints(origo, factor); 
			canvas.Children.Add(pl);

			// Point-circle
			for (int x = 0; x < this.Count() + xIncr - 1; x += xIncr)
			{
				int x2 = Math.Min(this.Count() - 1, x);
				var p = this[x2];
				if (p.Highlight && p.Enable)
				{
					var e = new Ellipse();
					e.Opacity = 1;
					e.Width = 16;
					e.Height = 16;
					e.Stroke = new SolidColorBrush(this.Color);
					e.StrokeThickness = 4;
					e.Fill = new SolidColorBrush(p.HighlightFill);
					e.Margin = new Thickness(origo.X + p.X * factor.X - 8, origo.Y - p.Y * factor.Y - 8, 0, 0);
					canvas.Children.Add(e);
				}
			}
		}

		private void DrawFilled(Canvas canvas, Point origo, Point factor)
		{
			var line = new Polygon();
			PointCollection pts = GetPoints(origo, factor);

			if (pts.Count > 0)
			{
				pts.Add(new Point(pts.Last().X, origo.Y - 1));
				pts.Add(new Point(pts.First().X, origo.Y - 1));
				line.Points = pts;
				line.Opacity = 0.50;
				line.Fill = new SolidColorBrush(this.Color);
				canvas.Children.Add(line);
			}
		}

		private PointCollection GetPoints(Point origo, Point factor)
		{
			var pts = new PointCollection();
			for (int x = 1; x < this.Count(); x++)
			{
				var a = this[x - 1];
				var b = this[x];

				if (b.Enable)
				{
					pts.Add(new Point(origo.X + a.X * factor.X, origo.Y - a.Y * factor.Y - this.Thickness*0 / 2));
					pts.Add(new Point(origo.X + b.X * factor.X, origo.Y - b.Y * factor.Y - this.Thickness*0 / 2));
				}
			}
			return pts;
		}

		#endregion // Methods
	}
}
