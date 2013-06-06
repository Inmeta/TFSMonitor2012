namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.Windows.Input;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Report;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using System.Net;
	using Microsoft.TeamFoundation.Client;
	using Osiris.Tfs.Monitor.Properties;
	using System.IO;

	#endregion // Using

	public interface IApplicationView
	{
		#region Methods

		void Close();
		string PromptSaveDocumentFile(string fileName);
		string PromptOpenDocumentFile();
		void DisplayApplicationOptions();
		void SlideShowFromBeginning();
		SlideTemplateVm SelectNewSlide();

		#endregion // Methods
	}
}
