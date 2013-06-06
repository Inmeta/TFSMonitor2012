namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.ComponentModel;
	using System.Net;
	using Microsoft.TeamFoundation.Client;
	using System.Threading;
	using System.Collections;
	using Microsoft.TeamFoundation;
	using System.Windows.Threading;
	using Osiris.TFS.Monitor.Common;

	#endregion // Using

	/// <summary>
	/// 
	/// Acess to TeamFoundationServer
	/// 
	/// </summary>
	public class TfsService : IDisposable
	{
		#region Fields

		static TfsService _instance;
		bool _useLocalAccount = true;

		#endregion // Fields

		#region Properties

		private string TfsUri { get; set; }
		public string TfsUserName { get; private set; }
		public string TfsPassword { get; private set; }
		public string TfsDomain { get; private set; }

		public static TfsService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TfsService();
				}
				return _instance;
			}
		}

		#endregion // Properties

		#region Constructors

		private TfsService() { }

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Connect
		/// </summary>
		/// <param name="tfsUri"></param>
		public void SetConnectionInfo(string tfsUri)
		{
			_useLocalAccount = true;
			this.TfsUri = tfsUri;
		}

		public void SetConnectionInfo(string tfsUri, string domain, string userName, string passwd)
		{
			_useLocalAccount = false;
			this.TfsUri = tfsUri;
			this.TfsDomain = domain;
			this.TfsUserName = userName;
			this.TfsPassword = passwd;
		}

		public TfsTeamProjectCollection Connect()
		{
			TfsTeamProjectCollection tpCol = null;

			if (this.TfsUri != null)
			{

				if (!_useLocalAccount)
				{
					// Credentials
					var cred = new NetworkCredential(this.TfsUserName, this.TfsPassword, this.TfsDomain);

					// Create connection with credentials
					tpCol = new TfsTeamProjectCollection(new Uri(this.TfsUri), cred);
				}
				else
				{
					// Create connection with local credentials
					tpCol = new TfsTeamProjectCollection(new Uri(this.TfsUri));
				}

				tpCol.Authenticate();
			}
			return tpCol;
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			_instance = null;
		}

		#endregion // IDisposable Members
	}
}
