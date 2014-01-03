namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Client;
	using System.Diagnostics;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using Microsoft.TeamFoundation.Build.Client;

	#endregion // Usings
	
	public class ProjectFilter
	{
		#region Properties

		public Node Node { get; private set; }

		public AreaCollection Areas { get; private set; }

		public TfsWorkItemTypeCollection WorkItemTypes { get; private set; }


		#endregion // Properties

		#region Constructors

		public ProjectFilter()
		{
		}

		#endregion // Constructors

		#region Methods

		public void Load(TeamProject tp)
		{
			var wis = (WorkItemStore)tp.TeamFoundationServer.GetService(typeof(WorkItemStore));
			foreach (Project p in wis.Projects)
			{
				if (p.Name == tp.Name)
				{
					Load(p);
				}
			}
		}

		private void Load(Project p)
		{
			// Areas
			this.Areas = new AreaCollection();
			this.Areas.Load(p);
		}

		#endregion // Methods
	}

	public class TfsWorkItemType
	{
	}

	public class TfsWorkItemTypeCollection : List<TfsWorkItemType>
	{
		public TfsWorkItemTypeCollection()
		{
		}

		public void Load(Project p)
		{
		}
	}

	public class TfsWorkItemState
	{
	}

	public class TfsWorkItemStateCollection : List<TfsWorkItemState>
	{
		public TfsWorkItemStateCollection()
		{
		}

		public void Load(TfsWorkItemTypeCollection types)
		{
		}
	}
}
