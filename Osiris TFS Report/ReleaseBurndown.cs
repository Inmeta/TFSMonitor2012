namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;

	#endregion // Usings

	public class ReleaseBurndown : List<BurndownChartData<WorkItem>>
	{
	}
}
