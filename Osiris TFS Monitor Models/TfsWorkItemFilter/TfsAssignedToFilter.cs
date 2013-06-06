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
	using Microsoft.TeamFoundation.Server;

	#endregion // Using

	public class TfsAssignedToFilter : IWorkItemFilter
	{
		public TfsIdentityCollection Users { get; set; }

		public TfsAssignedToFilter()
		{
			this.Users = new TfsIdentityCollection();
		}

		public TfsAssignedToFilter(TfsAssignedToFilter other) : this()
		{
			Copy(other);
		}

		private void Copy(TfsAssignedToFilter other)
		{
			this.Users.Clear();
			other.Users.ForEach(u => this.Users.Add(u));
		}

		public TfsAssignedToFilter(WorkItemStore wis) : this()
		{
			var sw = new Stopwatch();
			sw.Start();

			this.Users.Load(wis);

			sw.Stop();
			Debug.WriteLine("AssignedTos's load: " + sw.ElapsedMilliseconds.ToString() + "ms");
		}

		public bool IsWithin(Revision rev)
		{
			var assignedTo = rev.Fields[CoreField.AssignedTo].Value as string;
			if (this.Users.Count() >= 1 && !this.Users.Any(u => u == assignedTo))
			{
				return false;
			}
			return true;
		}
	}
}
