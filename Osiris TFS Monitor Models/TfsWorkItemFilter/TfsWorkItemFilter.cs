namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Osiris.Tfs.Report;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Xml.Serialization;
	using Osiris.TFS.Monitor.Common;
	using System.Diagnostics;
using Microsoft.TeamFoundation.Server;

	#endregion // Using

	public interface IWorkItemFilter
	{
		bool IsWithin(Revision rev);
	}

	public class TfsWorkItemFilter
	{
		#region Properties

		public string TeamProjectName { get; set; }

		public TfsAreaFilter AreaFilter { get; set; }

		public TfsIterationFilter IterationFilter { get; set; }

		public TfsWiTypeFilter WiTypeFilter { get; set; }

		public TfsWiStateFilter WiStateFilter { get; set; }

		public TfsAssignedToFilter AssignedToFilter { get; set; }

		[XmlIgnore]
		public int Id { get; private set; }

		#endregion // Properties

		#region Constructor

		public TfsWorkItemFilter() 
		{
			// Defaults
			this.Id = AutoIdGenerator.GenerateId();

			this.AreaFilter = new TfsAreaFilter();
			this.IterationFilter = new TfsIterationFilter();
			this.WiTypeFilter = new TfsWiTypeFilter();
			this.WiStateFilter = new TfsWiStateFilter();
			this.AssignedToFilter = new TfsAssignedToFilter();
		}

		private IEnumerable<IWorkItemFilter> Filters()
		{
			return new List<IWorkItemFilter>()
			{
			    this.AreaFilter,
			    this.IterationFilter,
			    this.AssignedToFilter,
			    this.WiTypeFilter,
			    this.WiStateFilter
			};
		}

		/// <summary>
		/// Copy construct
		/// </summary>
		public TfsWorkItemFilter(TfsWorkItemFilter other) 
		{
			Copy(other);
		}

		#endregion // Constructor

		#region Methods

		public void Load(string teamProject)
		{
			using (var tfs = TfsService.Instance.Connect())
			{
				var gss = tfs.GetService<IGroupSecurityService>();
				var wis = tfs.GetService<WorkItemStore>();
				foreach (Project p in wis.Projects)
				{
					if (p.Name == teamProject)
					{
						var sw = new Stopwatch();
						sw.Start();

						Load(wis, p, gss);

						sw.Stop();
						Debug.WriteLine("TfsWorkItemFilter.Load: " + sw.ElapsedMilliseconds.ToString() + "ms");

						return;
					}
				}
			}
		}

		private void Load(WorkItemStore wis, Project p, IGroupSecurityService gss)
		{
			this.AreaFilter = new TfsAreaFilter(wis, p);
			this.IterationFilter = new TfsIterationFilter(wis, p);
			var col = WorkItemTypeExCollection.GetTypes(wis, p);
			this.WiTypeFilter = new TfsWiTypeFilter(col);
			this.WiStateFilter = new TfsWiStateFilter(col);
			this.AssignedToFilter = new TfsAssignedToFilter(wis);
		}

		/// <summary>
		/// Compare if alike
		/// </summary>
		/// <returns></returns>
		public bool Compare(TfsWorkItemFilter wif)
		{
			return (this.TeamProjectName == wif.TeamProjectName);
		}

		/// <summary>
		/// Copy 
		/// </summary>
		public void Copy(TfsWorkItemFilter other)
		{
			this.Id = other.Id;
			this.TeamProjectName = other.TeamProjectName;
			this.IterationFilter = new TfsIterationFilter(other.IterationFilter);
			this.AreaFilter = new TfsAreaFilter(other.AreaFilter);
			this.WiStateFilter = new TfsWiStateFilter(other.WiStateFilter);
			this.WiTypeFilter = new TfsWiTypeFilter(other.WiTypeFilter);
			this.AssignedToFilter = new TfsAssignedToFilter(other.AssignedToFilter);
		}

		#endregion // Methods

		public bool IsWithIn(Revision rev)
		{
			return Filters().All(filter => filter.IsWithin(rev));
		}
	}
}
