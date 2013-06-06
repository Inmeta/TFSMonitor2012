namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;

	#endregion // Using

	/// <summary>
	/// This class is a support class for 2d-scaling.
	/// </summary>
	public class Scale2d
	{
		#region Fields

		double _widthFactor = 1;
		double _heightFactor = 1;
		double _minFactor = 1;
		double _minWidth;
		double _minHeight;
		double _scalefactor = 2;
		bool _nan = false;

		#endregion // Fields

		#region Properties

		public double W4 { get { return _nan ? double.NaN : _widthFactor * 4; } }
		public double W5 { get { return _nan ? double.NaN : _widthFactor * 5; } }
		public double W10 { get { return _nan ? double.NaN : _widthFactor * 10; } }
		public double W20 { get { return _nan ? double.NaN : _widthFactor * 20; } }

		public double M2 { get { return _nan ? double.NaN : _minFactor * 2; } }
		public double M3 { get { return _nan ? double.NaN : _minFactor * 3; } }
		public double M4 { get { return _nan ? double.NaN : _minFactor * 4; } }
		public double M5 { get { return _nan ? double.NaN : _minFactor * 5; } }
		public double M7 { get { return _nan ? double.NaN : _minFactor * 7; } }
		public double M8 { get { return _nan ? double.NaN : _minFactor * 8; } }
		public double M10 { get { return _nan ? double.NaN : _minFactor * 10; } }
		public double M14 { get { return _nan ? double.NaN : _minFactor * 14; } }
		public double M16 { get { return _nan ? double.NaN : _minFactor * 16; } }
		public double M20 { get { return _nan ? double.NaN : _minFactor * 20; } }
		public double M24 { get { return _nan ? double.NaN : _minFactor * 24; } }
		public double M30 { get { return _nan ? double.NaN : _minFactor * 30; } }

		#endregion // Properties

		#region Constructors

		public Scale2d() : this (1.0)
		{
		}

		public Scale2d(double scaleFactor) 
		{
			_scalefactor = scaleFactor;
			_minWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			_minHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Call when gui size has changed.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetSize(double width, double height)
		{
			if (double.IsNaN(width) || double.IsNaN(height))
			{
				Debug.WriteLine("NaN!");
				_nan = true;
				return;
			}

			_nan = false;
			_widthFactor = Math.Min(1 * _scalefactor, (width / _minWidth) * _scalefactor);
			_heightFactor = Math.Min(1 * _scalefactor, (height / _minHeight) * _scalefactor);
			_minFactor = Math.Min(_widthFactor, _heightFactor);

			//Debug.WriteLine("MinFactor: " + _minFactor.ToString() + ", WidthFactor: " + _widthFactor.ToString());
		}

		#endregion // Methods
	}
}
