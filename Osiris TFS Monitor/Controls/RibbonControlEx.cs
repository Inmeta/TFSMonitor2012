namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Windows.Controls.Ribbon;
	using System.Windows.Controls;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.ComponentModel;
	using System.Windows.Input;

	#endregion // Using

	public class ComboBoxEx : ComboBox, IRibbonControl { }

	public class TextBoxEx : TextBox, IRibbonControl { }

	public class DatePickerEx2 : DatePicker, IRibbonControl { }
}
