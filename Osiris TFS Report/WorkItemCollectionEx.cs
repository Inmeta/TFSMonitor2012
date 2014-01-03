namespace Osiris.TFS.Report
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;

	#endregion // Using

	internal class WorkItemCollectionEx : List<WorkItem>
	{
		#region Fields

		WorkItemStore _wis;

		#endregion // Fields

		#region Constructors

		public WorkItemCollectionEx(WorkItemStore wis)
		{
			_wis = wis;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Load all workItems's in TeamProject and Iteration.
		/// </summary>
		/// <param name="teamProject"></param>
		/// <param name="iterationPath"></param>
		public void Load(string teamProject, string iterationPath)
		{
			string query = @"SELECT [System.Id], [System.WorkItemType] FROM WorkItems WHERE [System.TeamProject] = '{0}' AND [System.IterationPath] EVER '{1}'";
			var wicol = _wis.Query(string.Format(query, teamProject, iterationPath));
			foreach (WorkItem wi in wicol)
			{
				this.Add(wi);	 
			}
		}

		#endregion // Methods
	}
}
