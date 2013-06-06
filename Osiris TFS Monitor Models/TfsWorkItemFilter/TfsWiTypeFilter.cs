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

	public class TfsWiTypeFilter : IWorkItemFilter
	{
		public List<WorkItemTypeEx> WiTypes { get; set; }

		#region Constructor

		public TfsWiTypeFilter()
		{
			this.WiTypes = new List<WorkItemTypeEx>();
		}

		public TfsWiTypeFilter(IEnumerable<WorkItemType> col) : this()
		{
			var sw = new Stopwatch();
			sw.Start();

			this.WiTypes = new List<WorkItemTypeEx>();
			col.ToList().ForEach(t => this.WiTypes.Add(new WorkItemTypeEx(t.Name)));

			sw.Stop();
			Debug.WriteLine("WITypes's load: " + sw.ElapsedMilliseconds.ToString() + "ms");

		}

		public TfsWiTypeFilter(TfsWiTypeFilter other) : this()
		{
			Copy(other);
		}

		private void Copy(TfsWiTypeFilter other)
		{
			this.WiTypes.Clear();
			other.WiTypes.ForEach(t => this.WiTypes.Add(new WorkItemTypeEx(t.Name)));
		}

		#endregion // Constructor
		
		public bool IsWithin(Revision rev)
		{
			if (this.WiTypes.Count() >= 1 && !this.WiTypes.Any(t => t.Name == rev.WorkItem.Type.Name))
			{
				return false;
			}
			return true;
		}
	}
}
