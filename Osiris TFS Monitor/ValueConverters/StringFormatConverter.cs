namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Windows.Data;

	#endregion // Using

	/// <summary>
	/// Formats string as in String.Format().
	/// </summary>
	public class StringFormatConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.Format(parameter.ToString(), value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion // IValueConverter Members
	}
}
