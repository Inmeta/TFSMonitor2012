﻿namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;

	#endregion // Using

	public partial class SlidesGroupView : RibbonGroup
	{
		#region Constructors

		public SlidesGroupView()
		{
			InitializeComponent();

			this.DataContext = new SlidesRibbonGroupVm();
		}

		#endregion // Constructors
	}
}
