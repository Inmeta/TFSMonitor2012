using System.Windows;

namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.ObjectModel;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Monitor.Properties;

	#endregion // Using

	/// <summary>
	/// Options for Application
	/// </summary>
	public class TfsIntegrationOptionsVm : OptionVm
	{
		#region Properties

		public TfsMonitorSettings Model { get { return this.Parent.Model; } }

		public string UserProfilesUri 
		{
			get
			{
				return this.Model.UserProfilesUri == null ? null : this.Model.UserProfilesUri.OriginalString;
			}
			set
			{
				try
				{
					this.Model.UserProfilesUri = string.IsNullOrEmpty(value) ? null : new Uri(value);
				}
				catch (System.UriFormatException ex)
				{
					MessageBox.Show("Invalid Uri", "Invalid Uri", MessageBoxButton.OK, MessageBoxImage.Error);
					this.Model.UserProfilesUri = null;
				}
			}
		}

		#endregion // Properties

		#region Constructors

		public TfsIntegrationOptionsVm(ApplicationOptionsVm parent)
			: base(parent, "TFS Integration", "Customize integration with Team Foundation Server.",
			@"pack://application:,,,/Resources/Icons/TfsIcon32x32.png")
		{
		}

		#endregion // Constructors
	}


}
