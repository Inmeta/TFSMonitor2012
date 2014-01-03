namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Usings

	/// <summary>
	/// Data for burndown.
	/// 
	/// - Time:			The time
	/// - Remaining:	Remaining 
	/// - Estimate:		Estimate
	/// 
	/// </summary>
	public class BurndownData
	{
		#region Properties

		public DateTime Time { get; private set; }
		public decimal Remaining { get; private set; }
		public decimal Estimate { get; private set; }

		#endregion // Properties

		#region Constructors

		public BurndownData(DateTime time, decimal remaining, decimal estimate)
		{
			this.Time = time;
			this.Remaining = remaining;
			this.Estimate = estimate;
		}

		#endregion // Constructors
	}
}
