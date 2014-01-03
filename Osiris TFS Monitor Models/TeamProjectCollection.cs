using Microsoft.TeamFoundation.Server;

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
    using Microsoft.TeamFoundation.Framework.Client;
    using Microsoft.TeamFoundation.Framework.Common;

	#endregion // Usings
	
	public class TeamProjectCollection : List<ProjectInfo>
	{
        

		#region Constructors

		public TeamProjectCollection()
		{
		}

		#endregion // Constructors

		#region Methods

		public void Load(TfsTeamProjectCollection tfsProjects)
		{
            //var versionControlServer = tfsProjects.GetService<VersionControlServer>();
            //TeamProject[] teamProjects = versionControlServer.GetAllTeamProjects(true);
            // Get team projects from TFS
            var structService = tfsProjects.GetService<Microsoft.TeamFoundation.Server.ICommonStructureService>();
            var teamProjects = structService.ListAllProjects().Select(a => a);
            //var pc = GetCurrentProjectCollection();
            //_projectCollectionSrc = new ProjectCollectionSource(getCurrrentCollectionName);
            //var collection = ImpersonatedCollection.CreateImpersonatedCollection(new Uri(pc.CollectionUrl),
            //    HttpContext.Current.User.Identity.Name);


			// Add team project to collection
			foreach (var tp in teamProjects)
			{
				this.Add(tp);
			}
		}

		#endregion // Methods
	}

 

}
