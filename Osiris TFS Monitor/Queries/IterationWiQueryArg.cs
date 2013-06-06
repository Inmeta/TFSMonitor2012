using System;

namespace Osiris.Tfs.Monitor
{
	public class IterationWiQueryArg : IEquatable<IterationWiQueryArg>
	{
		public string TeamProject { get; private set; }

		public int IterationId { get; private set; }

		public IterationWiQueryArg(string tp)
		{
			this.TeamProject = tp;
		}

		public bool Equals(IterationWiQueryArg other)
		{
			return (this.TeamProject == other.TeamProject);
		}
	}
}