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
	using Microsoft.TeamFoundation.Server;

	#endregion // Usings
	
	/// <summary>
	/// Collection of TfsIdentity-nodes. Each node also holds instance of
	/// TfsIdentity work item matching the TfsIdentity path of the node.
	/// </summary>
	public class TfsIdentityCollection : List<string>
	{
		#region Fields

		#endregion // Fields

		#region Constructors

		public TfsIdentityCollection()
		{
		}

		#endregion // Constructors

		#region Methods

		public void Load(WorkItemStore wis)
		{

			var allowedValues = wis.FieldDefinitions[CoreField.AssignedTo].AllowedValues;
			foreach (String value in allowedValues)
			{
				if (!value.StartsWith("["))
				{
					this.Add(value);
				}
			}



			/*
			Identity sids = gss.ReadIdentity(SearchFactor.AccountName, "Team Foundation Valid Users", QueryMembership.Expanded);
			if (sids != null)
			{
				Identity[] users = gss.ReadIdentities(SearchFactor.Sid, sids.Members, QueryMembership.None);
				if (users != null)
				{
					foreach (var usr in users)
					{
						this.Add(usr);
					}
				}
			}
			*/
		}

		#endregion // Methods
	}
}
