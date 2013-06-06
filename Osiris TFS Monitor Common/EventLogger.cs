namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
using System.Security;

	#endregion // Using

	public static class EventLogger
	{
		public static void Write(Exception ex)
		{
			// Refactor to get app-info from app-binary
			var evSource = "Osiris TFS Monitor";
			var evLog = "Application";
			var evEvent = ex.Message + "\r\n\r\n" + ex.StackTrace;

			try
			{
				if (!EventLog.SourceExists(evSource))
				{
					EventLog.CreateEventSource(evSource, evLog);
				}
				EventLog.WriteEntry(evSource, evEvent);
			}
			catch (SecurityException)
			{
				// What to do?
				Debug.WriteLine("Unable to write entry in windows event-log.");
			}
		}
	}
}
