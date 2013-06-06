namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Controls;
	using System.Globalization;

	#endregion // Using

	/// <summary>
	/// Requires string as a parsable positive number supporting suffixes 
	/// second and minute.
	/// </summary>
	public class DurationValidationRule : ValidationRuleBase
	{
		#region Constructors
	
		public DurationValidationRule(string errorMsg) : base(errorMsg)
		{
		}

		#endregion // Constructors

		#region Methods

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (string.IsNullOrEmpty(value.ToString()))
			{
				return new ValidationResult(true, null);
			}

			int? parseResult = DurationValidationRule.Parse(value as string);
			bool valid = (parseResult.HasValue && parseResult.Value >= 0);
			return new ValidationResult(valid, valid ? null : this.ErrorMessage);
		}

		public static int? Parse(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}

			value = value.Trim();

			// Parse number
			var intValue = "";
			var pos = 0;
			while (pos < value.Length)
			{
				if (value[pos] < '0' || value[pos] > '9')
				{
					break;
				}
				intValue += value[pos++];
			}

			// Any int value?
			if (string.IsNullOrEmpty(intValue))
			{
				return -1;
			}

			// Is int?
			int result;
			if (!int.TryParse(intValue, out result))
			{
				return -1;
			}

			if (pos >= value.Length)
			{
				return result;
			}

			string suffix = value.Substring(pos);
			suffix = suffix.Trim();

			if (suffix == "s" || suffix == "sec" || suffix == "seconds" || suffix == "second")
			{
				return result;
			}

			if (suffix == "m" || suffix == "min" || suffix == "minutes" || suffix == "minute")
			{
				return result * 60;
			}

			return -1;
		}


		#endregion // Methods
	}
}
