namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Build.Client;

	#endregion // Using
	
	public class BuildDefinition
    {
        #region Properties

        public string TeamProject { get; set; }
        public string BuildName { get; set; }
        public BuildStatus Status { get; set; }

        #endregion

        #region Constructors

        public BuildDefinition()
        {
        }

        public BuildDefinition(IBuildDefinition tfsBuildDefinition)
        {
            TeamProject = tfsBuildDefinition.TeamProject;
            BuildName = tfsBuildDefinition.Name;
        }

        /// <summary>
        /// Copy-contructor
        /// </summary>
        /// <param name="other"></param>
        public BuildDefinition(BuildDefinition other)
        {
            Copy(other);
        }

        #endregion

        #region Methods

        public bool Compare(BuildDefinition other)
        {
            return TeamProject == other.TeamProject && BuildName == other.BuildName;
        }

        private void Copy(BuildDefinition other)
        {
            TeamProject = other.TeamProject;
            BuildName = other.BuildName;
            //copy status here too? why not?
        }

        #endregion // Methods
    }
}
