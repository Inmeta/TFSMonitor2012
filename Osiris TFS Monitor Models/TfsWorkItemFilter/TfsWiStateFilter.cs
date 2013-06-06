namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Xml.Serialization;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using Osiris.Tfs.Report;
	using System.Diagnostics;

	#endregion // Using

	public class TfsWiStateFilter : IWorkItemFilter
	{
		public List<string> States { get; set; }

		public TfsWiStateFilter()
		{
			this.States = new List<string>();
		}

		public TfsWiStateFilter(TfsWiStateFilter other) : this()
		{
			Copy(other);
		}

		private void Copy(TfsWiStateFilter other)
		{
			this.States.Clear();
			other.States.ForEach(s => this.States.Add(s));
		}

		public TfsWiStateFilter(IEnumerable<WorkItemType> types) : this()
		{
			var sw = new Stopwatch();
			sw.Start();

			var states = WorkItemTypeExCollection.GetStates(types);
			this.States = new List<string>();
			states.ToList().ForEach(s => this.States.Add(s));

			sw.Stop();
			Debug.WriteLine("WiStates's load: " + sw.ElapsedMilliseconds.ToString() + "ms");

		}

		public bool IsWithin(Revision rev)
		{
			var state = rev.Fields[CoreField.State].Value as string;
			if (this.States.Count() >= 1 && !this.States.Any(s => s == state))
			{
				return false;
			}
			return true;
		}
	}
}
