using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;

namespace Osiris.Tfs.Monitor.Models.Utilities
{
    public static class BuildUtility
    {
        private const string BuildMustContainThisToBeMonitored = "refs/heads";
        private const string TfsVsStartsWithThis = "C";
        public static string GetBuildNameWithBranchName(IBuildDetail bd)
        {
            var sourceGetVersion = bd.SourceGetVersion;
            if (IsGitBuild(sourceGetVersion))
            {
                var versionParts = sourceGetVersion.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (versionParts.Any())
                {
                    var branchName =
                        versionParts.Last()
                            .Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries)
                            .ToList()
                            .FirstOrDefault();
                    return bd.BuildDefinition.Name + "/" + branchName;
                }
            }
            return bd.BuildDefinition.Name;
        }

        private static bool IsGitBuild(string sourceGetVersion)
        {
            return sourceGetVersion.Contains(BuildMustContainThisToBeMonitored);
        }

        public static IEnumerable<Build> GetUniqueBuilds(IEnumerable<Build> builds)
        {
            HashSet<Build> list = HashSetHelper<Build>.Create(b => b.BuildName);
            var orderedListOfBuilds = builds.Select(b => b).OrderByDescending(z => z.FinishTime);
            foreach (var build in orderedListOfBuilds)
            {
                list.Add(build);
            }
            return list;
        }

        public static IBuildDetail[] FilterMontoredBuildsOnly(IBuildQueryResult details)
        {
            return details.Builds.Where(b => (b.SourceGetVersion.Contains(BuildMustContainThisToBeMonitored) || b.SourceGetVersion.StartsWith(TfsVsStartsWithThis))).ToArray();
        }
    }
}