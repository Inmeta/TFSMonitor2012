namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.Windows.Controls;

	#endregion // Using

	public class SlideEditView : SlideView
	{
		public SlideEditView()
		{
			Debug.WriteLine("SlideEditView()");
		}

		protected override UserControl CreateContainer(SlideElementContainerVm elem)
		{
			return new SlideElementEditContainerView(canvas, elem);
		}
	}
}
