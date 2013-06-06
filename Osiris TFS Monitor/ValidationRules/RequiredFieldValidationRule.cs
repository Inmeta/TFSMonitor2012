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
	/// Requires != null and/or ToString() == ""
	/// </summary>
	public class RequiredFieldValidationRule : ValidationRuleBase
	{
		#region Constructors

		public RequiredFieldValidationRule(string errorMsg) : base(errorMsg)
		{
		}

		#endregion // Constructors

		#region Methods

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (string.IsNullOrEmpty(value.ToString()))
			{
				return new ValidationResult(false, this.ErrorMessage);
			}

			return new ValidationResult(true, null);
		}

		#endregion // Methods
	}
}
