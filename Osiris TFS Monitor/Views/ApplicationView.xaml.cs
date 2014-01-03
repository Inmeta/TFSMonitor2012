namespace Osiris.Tfs.Monitor
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
	using System.Windows.Shapes;
	using System.Diagnostics;
	using Osiris.Tfs.Monitor.Models;
	using Microsoft.Windows.Controls.Ribbon;
	using System.IO;
	using System.Windows.Interop;
	using System.Windows.Automation.Peers;
	using System.Windows.Automation.Provider;
using System.Reflection;
using System.ComponentModel;

	#endregion // Using

	/// <summary>
	/// Run application in monitor mode windowed. Window can also run
	/// fullscreen like a the screensaver.
	/// </summary>
	public partial class ApplicationView : RibbonWindow, IApplicationView
	{
		#region Fields

		ApplicationVm _vm;

		#endregion // Fields

		#region Constructors

		public ApplicationView() : this(new ApplicationVm()) { }

		public ApplicationView(ApplicationVm vm)
		{
			InitializeComponent();

			_vm = vm;
			this.DataContext = _vm;
		}

		#endregion // Constructors

		#region Methods

		public void DisplayApplicationOptions()
		{
			var dlg = new ApplicationOptionsView(this);
			dlg.ShowDialog();
		}

		public string PromptSaveDocumentFile(string fileName)
		{
			var dlg = new System.Windows.Forms.SaveFileDialog();

			dlg.InitialDirectory = System.IO.Path.GetFullPath(fileName);
			dlg.FileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
			return DisplayFileDialog(dlg);
		}

		public string PromptOpenDocumentFile()
		{
			var dlg = new System.Windows.Forms.OpenFileDialog();
			return DisplayFileDialog(dlg);
		}

		private string DisplayFileDialog(System.Windows.Forms.FileDialog dlg)
		{
			dlg.DefaultExt = ".tmdoc";
			dlg.Filter = "TFS Monitor Document|*.tmdoc";
			WindowInteropHelper helper = new WindowInteropHelper(this);
			if (dlg.ShowDialog(new Win32Window(helper.Handle)) == System.Windows.Forms.DialogResult.OK)
			{
				return dlg.FileName;
			}
			return null;
		}

		public void SlideShowFromBeginning()
		{
			var ss = new SlideshowView();
			ss.OnFullscreenClosed += new EventHandler(OnFullscreenClosed);
			ss.Show();
		}

		public SlideTemplateVm SelectNewSlide()
		{
			var dlg = new SelectSlideTemplateView(this);
			dlg.ShowDialog();
			return dlg.Selected;
		}

		private void OnFullscreenClosed(object sender, EventArgs e)
		{
			_vm.FullscreenClosed();
		}

		private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_vm.View = this;
		}

		/// <summary>
		/// Error in ribbonscontrol previewmousedown prevents underlying controls to recieve mousewheel event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Delta == 0 || e.OriginalSource is HorizontalScrollViewer)
			{
				return;
			}

			// ToDo: Make extension method on DependenyObject
			var obj = e.OriginalSource as DependencyObject;
			while (obj != null && !(obj is ScrollViewer) && !(obj is Run))
			{
				obj = VisualTreeHelper.GetParent(obj);
			}

			// Forward scrolling...
			if (obj is ScrollViewer)
			{
				var sv = obj as ScrollViewer;
				for (int i = 0; i < 3; i++ )
				{
					if (e.Delta < 0)
					{
						sv.LineDown();
					}
					else
					{
						sv.LineUp();
					}
				}
				e.Handled = true;
			}
		}

		#endregion // Methods
	}
}
