namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Windows;
	using System.Windows.Data;

	#endregion // Using

	/// <summary>
	/// Invartes boolean value.
	/// </summary>
	public class InvertBoolConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool? boolValue = (bool?)value;
			if (!boolValue.HasValue)
			{
				return null;
			}
			return !boolValue.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return this.Convert(value, targetType, parameter, culture);
		}

		#endregion // IValueConverter Members
	}
}
