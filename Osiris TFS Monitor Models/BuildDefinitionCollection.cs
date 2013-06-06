namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Client;
	using Microsoft.TeamFoundation.Build.Client;
	using Microsoft.TeamFoundation.VersionControl.Client;

	#endregion // Using

	public class BuildDefinitionCollection : List<BuildDefinition>
    {
		#region Constructors

		public BuildDefinitionCollection()
		{ 
		}

		#endregion // Constructors

		#region Methods

		public void Load(TfsTeamProjectCollection tfsProjects)
        {
			// Get buildserver
			/*var vcs = tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;
			var bs = tfs.GetService(typeof(IBuildServer)) as IBuildServer;*/

			// Iterate team projects
			var vcs = tfsProjects.GetService<VersionControlServer>();
			var bs = tfsProjects.GetService<IBuildServer>();
			var teamProjects = vcs.GetAllTeamProjects(true);
            foreach(var tp in teamProjects)
            {
				// Add every build-definition
                var buildDefinitions = bs.QueryBuildDefinitions(tp.Name);
                foreach (var bd in buildDefinitions)
                {
					this.Add(new BuildDefinition(bd));
                }
            }
		}

		

		#endregion // Methods
	}
}
