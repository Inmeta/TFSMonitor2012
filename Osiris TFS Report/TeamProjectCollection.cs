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
	
	public class TeamProjectCollection : List<TeamProject>
	{
		#region Constructors

		public TeamProjectCollection()
		{
		}

		#endregion // Constructors

		#region Methods

		public void Load(TfsTeamProjectCollection tfsProjects)
		{
			var versionControlServer = tfsProjects.GetService<VersionControlServer>();
			TeamProject[] teamProjects = versionControlServer.GetAllTeamProjects(true);

			// Add team project to collection
			foreach (TeamProject tp in teamProjects)
			{
				this.Add(tp);
			}
		}

		#endregion // Methods
	}
}
