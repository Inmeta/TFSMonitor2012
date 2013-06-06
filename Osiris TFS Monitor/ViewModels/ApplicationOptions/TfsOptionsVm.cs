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
	public class TfsOptionsVm : OptionVm
	{
		#region Properties

		public TfsMonitorSettings Model { get { return this.Parent.Model; } }

		#endregion // Properties

		#region Constructors

		public TfsOptionsVm(ApplicationOptionsVm parent)
			: base(parent, "TFS Connection", "Connection settings for Team Foundation Server.",
			@"pack://application:,,,/Resources/Icons/TfsIcon32x32.png")
		{
		}

		#endregion // Constructors
	}


}
