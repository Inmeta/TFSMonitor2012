namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.Specialized;

	#endregion // Using

	public interface ISettings
	{
		#region Properties

		string TfsUri { get; set; }
		string TfsUser { get; set; }
		string TfsDomain { get; set; }
		string TfsPassword { get; set; }
		string TfsUseLocalAccount { get; set; }
		string SlideshowMonitors { get; set; }
		string SlideshowTurnOffScreenSaver { get; set; }
		string UserProfilesUri { get; set; }
		string RecentFiles { get; set; }

		#endregion // Properties
		
		#region Methods

		void Save();

		#endregion // Methods
	}
}
