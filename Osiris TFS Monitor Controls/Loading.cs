namespace TfsMonitor.Controls
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;

	#endregion // Using

	public class Loading : Control
	{
		static Loading()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Loading), new FrameworkPropertyMetadata(typeof(Loading)));
		}
	}
}
