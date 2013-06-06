namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using Osiris.Tfs.Report;

	#endregion // Using

	public class BurndownChart : SlideElement, IEquatable<BurndownChart>
	{
		//int _pollingInterval; // Seconds

		#region Properties

		/*public int UpdateInterval
		{
			get { return _pollingInterval; }
			set
			{
				_pollingInterval = value;
			}
		}*/

		public override TfsIteration TfsIteration
		{
			get
			{
				if (_tfsIteration == null)
				{
					_tfsIteration = new TfsIteration();
				}
				return _tfsIteration;
			}
		}

		public override TfsWorkItemFilter TfsWorkItemFilter
		{
			get
			{
				if (_tfsWorkItemFilter == null)
				{
					_tfsWorkItemFilter = new TfsWorkItemFilter();
				}
				return _tfsWorkItemFilter;
			}
		}

		public SprintBurndownType SprintBurndownType { get; set; }

		public bool ExcludeWeekEnds { get; set; }

		public string RemainingFieldName { get; set; }

		public string EstimateFieldName { get; set; }

		#endregion // Properties

		#region Constructor

		public BurndownChart()
		{
			// Default values
			this.SprintBurndownType = new SprintRemainingHoursBurndownType();
			this.RemainingFieldName = "Remaining Work";
			this.EstimateFieldName = "Original estimate";
			this.ExcludeWeekEnds = false;
			_updateInterval = 60 * 5;
		}

		#endregion // Constructor

		#region IEquatable<BurndownChart> Members

		public bool Equals(BurndownChart other)
		{
			return this.Id == other.Id;
		}

		#endregion
	}
}
