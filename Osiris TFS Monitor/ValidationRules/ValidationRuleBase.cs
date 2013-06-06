namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Controls;
	using System.Globalization;

	#endregion // Using

	/// <summary>
	/// Base class for validation rules-
	/// </summary>
	public abstract class ValidationRuleBase : ValidationRule
	{
		#region Properties

		public string ErrorMessage { get; set; }

		#endregion // Properties

		#region Constructors

		protected ValidationRuleBase(string errorMsg)
		{
			this.ErrorMessage = errorMsg;
		}

		#endregion // Constructors

	}
}
