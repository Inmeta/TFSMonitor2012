namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Usings

	public class TfsReportException : Exception
	{
		public TfsReportException(string msg) : base(msg) {	}
	}
}
