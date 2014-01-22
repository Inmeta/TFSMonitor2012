using System.Diagnostics.Contracts;
using Osiris.Tfs.Monitor.Models.Utilities;

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
	using System.Diagnostics;
	using Osiris.Tfs.Report;
	using System.Collections;

	#endregion // Using

	public enum BuildFilterStatus
	{
		InProgress = 1,
		Succeeded = 2,
		PartiallySucceeded = 4,
		Failed = 8,
		Stopped = 16,
		NotStarted = 32
	}

	public class BuildFilter : IEquatable<BuildFilter>
	{
		public readonly int MaxFetch;
		public readonly IEnumerable<string> TeamProjects;
		public readonly BuildFilterStatus Status; 

		public BuildFilter(int maxFetch, IEnumerable<string> teamProjects, BuildFilterStatus status)
		{
			Contract.Requires(teamProjects != null);
			Contract.Ensures(this.TeamProjects != null);

			this.MaxFetch = maxFetch;
			this.TeamProjects = teamProjects;
			this.Status = status;
		}

		public bool Equals(BuildFilter other)
		{
			Contract.Assume(this.TeamProjects != null);
			Contract.Assume(other.TeamProjects != null);

			return (this.Status == other.Status && this.TeamProjects.SequenceEqual(other.TeamProjects));
		}
	}

	public class Build : IEquatable<Build>, IComparable<Build>
	{
        public string TeamProject { get; set; }
        public string BuildName { get; set; }
		public BuildStatus Status { get; set ; }
		public DateTime StartTime { get; set; }
		public string RequestedBy { get; set; }
		public string RequestedFor { get; set; }
		public DateTime? FinishTime { get; set; }

		internal Build(IBuildDetail bd)
		{

            this.BuildName = BuildUtility.GetBuildNameWithBranchName(bd);
			this.TeamProject = bd.BuildDefinition.TeamProject;
			this.Status = bd.Status;
			this.StartTime = bd.StartTime;
			this.RequestedBy = bd.RequestedBy;
			this.RequestedFor = bd.RequestedFor;
			this.FinishTime = bd.BuildFinished ? bd.FinishTime : (DateTime?)null;
		}


		#region IEquatable<Build> Members

		public bool Equals(Build other)
		{
			return
				this.TeamProject == other.TeamProject &&
				this.BuildName == other.BuildName &&
				this.Status == other.Status &&
				this.StartTime == other.StartTime &&
				this.RequestedBy == other.RequestedBy &&
				this.RequestedFor == other.RequestedFor &&
				this.FinishTime == other.FinishTime;
		}

		#endregion

		#region IComparable<Build> Members

		public int CompareTo(Build other)
		{
			if (this.Status == BuildStatus.Failed && this.Status != other.Status)
			{
				return 1;
			}
			if (this.Status == BuildStatus.PartiallySucceeded && this.Status != other.Status)
			{
				if (other.Status == BuildStatus.Failed)
				{
					return -1;
				}
				return 1;
			}
			if (this.Status == BuildStatus.InProgress && this.Status != other.Status)
			{
				if (other.Status == BuildStatus.Failed)
				{
					return -1;
				}
				if (other.Status == BuildStatus.PartiallySucceeded)
				{
					return -1;
				}
				return 1;
			}
			return this.StartTime.CompareTo(other.StartTime);
		}

		#endregion // IComparable<Build> Members
	}

	public class BuildCollection : List<Build>
    {
		#region Constructors

		public BuildCollection()
		{ 
		}

		#endregion // Constructors

		#region Methods

		public void Load(TfsTeamProjectCollection tfsProjects, BuildFilter filter)
		{
			if (filter.Status == 0)
			{
				return;
			}

			// Get buildserver
			var versionControlServer = tfsProjects.GetService<VersionControlServer>(); // tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;
			var buildServer = tfsProjects.GetService<IBuildServer>(); //tfs.GetService(typeof(IBuildServer)) as IBuildServer;

			// Team projects
			IEnumerable<string> teamProjects;
			if (filter.TeamProjects == null || filter.TeamProjects.Count() == 0)
			{
				var tfsTps = versionControlServer.GetAllTeamProjects(false);
				if (tfsTps != null && tfsTps.Count() > 0)
				{
					teamProjects = tfsTps.Select(tp => tp.Name);
				}
				else
				{
					teamProjects = new List<string>();
				}
			}
			else
			{
				teamProjects = filter.TeamProjects;
			}

			// Add builds
			AddBuilds(buildServer, teamProjects, filter);
		}

		private void AddBuilds(IBuildServer bs, IEnumerable<string> teamProjects, BuildFilter filter)
		{
			var blist = new List<Build>();
			foreach (var tp in teamProjects)
            {
				IBuildDetailSpec spec = bs.CreateBuildDetailSpec(tp);
				spec.Status = BuildStatus.Failed | BuildStatus.InProgress | BuildStatus.PartiallySucceeded | BuildStatus.Succeeded;
				spec.QueryOrder = BuildQueryOrder.StartTimeDescending;
				spec.QueryOptions = QueryOptions.All;
				spec.MaxBuildsPerDefinition = Properties.Settings.Default.MaxBuildsPerDefinition;
                spec.QueryOrder = BuildQueryOrder.FinishTimeDescending;
				spec.InformationTypes = null;
                
				IBuildQueryResult details = bs.QueryBuilds(spec);
                
				if (details != null && details.Builds != null)
				{
                    IBuildDetail[] buildDetails = BuildUtility.FilterMontoredBuildsOnly(details);
                    var builds = buildDetails.Where(bd => bd.BuildDefinition.Enabled).Select(bd => new Build(bd));
				    var uniqueBuilds = BuildUtility.GetUniqueBuilds(builds);
				    blist.AddRange(uniqueBuilds);
				}
            }

			// Add grouped
			var maxFetch = filter.MaxFetch;
			if ((filter.Status & BuildFilterStatus.Failed) != 0)
			{
				this.AddRange(blist.Where(b => b.Status == BuildStatus.Failed).OrderByDescending(b => b.StartTime).Take(maxFetch));
			}
			if ((filter.Status & BuildFilterStatus.PartiallySucceeded) != 0)
			{
				this.AddRange(blist.Where(b => b.Status == BuildStatus.PartiallySucceeded).OrderByDescending(b => b.StartTime).Take(maxFetch - Count));
			}
			if ((filter.Status & BuildFilterStatus.InProgress) != 0)
			{
				this.AddRange(blist.Where(b => b.Status == BuildStatus.InProgress).OrderByDescending(b => b.StartTime).Take(maxFetch - Count));
			}
			if ((filter.Status & BuildFilterStatus.Succeeded) != 0)
			{
				this.AddRange(blist.Where(b => b.Status == BuildStatus.Succeeded).OrderByDescending(b => b.StartTime).Take(maxFetch - Count));
			}
		}
		
		private BuildStatus GetBuildStatus(BuildFilterStatus status)
		{
			BuildStatus bs = 0;

			bs |= ((status & BuildFilterStatus.Failed) != 0) ? BuildStatus.Failed : (BuildStatus)0;
			bs |= ((status & BuildFilterStatus.Succeeded) != 0) ? BuildStatus.Succeeded : (BuildStatus)0;
			bs |= ((status & BuildFilterStatus.InProgress) != 0) ? BuildStatus.InProgress : (BuildStatus)0;
			bs |= ((status & BuildFilterStatus.PartiallySucceeded) != 0) ? BuildStatus.PartiallySucceeded : (BuildStatus)0;
			bs |= ((status & BuildFilterStatus.NotStarted) != 0) ? BuildStatus.NotStarted : (BuildStatus)0;
			bs |= ((status & BuildFilterStatus.Stopped) != 0) ? BuildStatus.Stopped : (BuildStatus)0;

			return bs;
		}
		
		#endregion // Methods
	}
}
