namespace Osiris.TFS.Monitor.Common
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	public static class TimeSpanExstension
	{
		public static TimeSpan SubtractEx(this DateTime dt, DateTime value, bool excludeWeekEnds)
		{
			if (!excludeWeekEnds)
			{
				return (dt - value);
			}

			TimeSpan tsResult = new TimeSpan();
			TimeSpan tsTemplate;
			if (value.DayOfWeek != DayOfWeek.Saturday && value.DayOfWeek != DayOfWeek.Sunday)
			{
				// Add the TimeSpan of first date.  
				tsTemplate = new TimeSpan(0, value.Hour, value.Minute, value.Second);
				tsResult = tsResult.Add(tsTemplate);
			}
			if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
			{
				// Add the TimeSpan of last date.  
				tsTemplate = new TimeSpan(0, dt.Hour, dt.Minute, dt.Second);
				tsResult = tsResult.Add(tsTemplate);
			}

			value = new DateTime(value.Year, value.Month, value.Day);
			value.AddDays(1);
			dt = new DateTime(dt.Year, dt.Month, dt.Day);

			while (value < dt)
			{
				if (value.DayOfWeek != DayOfWeek.Saturday && value.DayOfWeek != DayOfWeek.Sunday)
				{
					tsTemplate = new TimeSpan(1, 0, 0, 0);
					tsResult = tsResult.Add(tsTemplate);
				}
				value = value.AddDays(1);
			}

			return tsResult;
		}
	}
}
