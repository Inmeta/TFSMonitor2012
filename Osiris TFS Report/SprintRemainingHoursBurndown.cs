namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using Microsoft.TeamFoundation.Client;
	using System.Diagnostics;
	using Osiris.TFS.Report;
	using Osiris.TFS.Monitor.Common;

	#endregion // Usings

	public class SprintRemainingHoursBurndown : SprintBurndown
	{
		#region Fields

		SprintBurndownType _type = new SprintRemainingHoursBurndownType();

		#endregion // Fields

		#region Properties

		public override SprintBurndownType SprintBurndownType { get { return _type; } }
 
		#endregion // Properties

		#region Constructors

		public SprintRemainingHoursBurndown(string teamProject, int iterationWiId, 
			TimeSpan timeInterval, bool excludeWeekEnds, string estimateFieldName, string remainingFieldName)
			: base(teamProject, iterationWiId, timeInterval, excludeWeekEnds, estimateFieldName, remainingFieldName)
		{
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Create chart data for each interval between start- and end-date of burndown iteration.
		/// 
		/// Note: Also includes current time.
		/// </summary>
		/// <param name="time"></param>
		/// <param name="wis"></param>
		internal override void CreateChartData(DateTime time, WorkItemTimeCollection wis)
		{
			var totalHours = this.EndDate.SubtractEx(this.StartDate, _excludeWeekEnds).TotalHours;
			var elapsedHours = time.SubtractEx(this.StartDate, _excludeWeekEnds).TotalHours;
			var totalEstimate = wis.TotalEstimate();

			decimal estimate = totalEstimate -  ((decimal)(elapsedHours / totalHours)) * totalEstimate;
			_data.Add(new BurndownData(time, wis.TotalRemaining(), estimate));
		}

		#endregion // Methods
	}
}
