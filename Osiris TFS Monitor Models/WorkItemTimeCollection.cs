﻿namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Diagnostics;

	#endregion // Usings

	internal class WorkItemTimeCollection : Dictionary<int, WorkItemTime>
	{
		#region Fields

		string _estimateFieldName;
		string _remainingFieldName;

		#endregion // Fields

		#region Constructors

		public WorkItemTimeCollection(string estimateFieldName, string remainingFieldName)
		{
			_estimateFieldName = estimateFieldName;
			_remainingFieldName = remainingFieldName;
		}

		#endregion // Constructors

		#region Methods

		public void Add(Revision rev)
		{
			// Set/join workitem
			WorkItemTime wiTime;
			if (!this.TryGetValue(rev.WorkItem.Id, out wiTime))
			{
				this.Add(rev.WorkItem.Id, new WorkItemTime(rev));
			}
			else
			{
				wiTime.Join(rev);
			}
		}

		public void Filter(int iterationId)
		{
			// Build up exclude list
			var excluded = new List<int>();
			foreach (WorkItemTime wiTime in this.Values)
			{

				if (wiTime.Revision.Fields[CoreField.IterationId].Value.ToString() != iterationId.ToString())
				{
					excluded.Add(wiTime.Revision.WorkItem.Id);
				}
			}

			// Remove items
			foreach (int id in excluded)
			{
				this.Remove(id);
			}
		}


		public decimal TotalRemaining()
		{
			decimal result = 0;
			foreach (var wiTime in this.Values)
			{
				result += wiTime.DecimalValue(_remainingFieldName);
			}
			return result;
		}

		public decimal TotalEstimate()
		{
			decimal result = 0;
			foreach (var wiTime in this.Values)
			{
				result += wiTime.DecimalValue(_estimateFieldName);
			}
			return result;
		}

		/// <summary>
		/// Calculate total remaining "tasks" for this timeframe.
		/// </summary>
		/// <returns></returns>
		public decimal TotalRemainingTasks()
		{
			decimal result = 0;
			foreach (var wiTime in this.Values)
			{
				result++;
			}
			return result;
		}

		#endregion // Methods
	}
}
