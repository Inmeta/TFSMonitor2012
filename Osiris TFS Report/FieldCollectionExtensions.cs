namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;

	#endregion // Usings

	internal static class FieldCollectionExtension
	{
		public static DateTime? GetDateTimeValue(this FieldCollection fields, string fieldName)
		{
			if (fields.Contains(fieldName))
			{
				var value = fields[fieldName].Value;
				if (value != null)
				{
					if (!string.IsNullOrEmpty(value.ToString()))
					{
						DateTime date;
						if (DateTime.TryParse(value.ToString(), out date))
						{
							return date;
						}
					}
				}
			}

			// Not found or not correct date-format
			return null;
		}

		public static decimal? GetDecimalValue(this FieldCollection fields, string fieldName)
		{
			if (fields.Contains(fieldName))
			{
				var value = fields[fieldName].Value;
				if (value != null)
				{
					decimal decimalValue;
					if (decimal.TryParse(value.ToString(), out decimalValue))
					{
						return decimalValue;
					}
				}
				return 0;
			}

			// Not found or not correct decimal-format
			return null;
		}
	}
}
