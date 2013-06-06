namespace Osiris.Tfs.Monitor
{
	using System;
	using System.Runtime.InteropServices;
	using System.Windows;
	using System.Windows.Interop;

	public class DialogWindow : Window
	{
		/// <summary>
		/// Native methods/properties wrapper.
		/// </summary>
		private static class NativeMethods
		{
			public const int WM_HELP = 0x53;
			public const int WM_LBUTTONDOWN = 0x201;
			public const int WM_SYSCOMMAND = 0x112;
			public const int SC_CONTEXTHELP = 0xF180;
			public const int GWL_STYLE = -16;
			public const int GWL_EXSTYLE = -20;
			public const int WS_BORDER = 0x800000;
			public const int WS_CAPTION = 0xc00000;
			public const int WS_CHILD = 0x40000000;
			public const int WS_CLIPCHILDREN = 0x2000000;
			public const int WS_CLIPSIBLINGS = 0x4000000;
			public const int WS_DISABLED = 0x8000000;
			public const int WS_DLGFRAME = 0x400000;
			public const int WS_EX_APPWINDOW = 0x40000;
			public const int WS_EX_CLIENTEDGE = 0x200;
			public const int WS_EX_COMPOSITED = 0x2000000;
			public const int WS_EX_CONTEXTHELP = 0x400;
			public const int WS_EX_CONTROLPARENT = 0x10000;
			public const int WS_EX_DLGMODALFRAME = 1;
			public const int WS_EX_LAYERED = 0x80000;
			public const int WS_EX_LAYOUTRTL = 0x400000;
			public const int WS_EX_LEFT = 0;
			public const int WS_EX_LEFTSCROLLBAR = 0x4000;
			public const int WS_EX_MDICHILD = 0x40;
			public const int WS_EX_NOACTIVATE = 0x8000000;
			public const int WS_EX_NOINHERITLAYOUT = 0x100000;
			public const int WS_EX_RIGHT = 0x1000;
			public const int WS_EX_RTLREADING = 0x2000;
			public const int WS_EX_STATICEDGE = 0x20000;
			public const int WS_EX_TOOLWINDOW = 0x80;
			public const int WS_EX_TOPMOST = 8;
			public const int WS_EX_TRANSPARENT = 0x20;
			public const int WS_EX_WINDOWEDGE = 0x100;
			public const int WS_HSCROLL = 0x100000;
			public const int WS_MAXIMIZE = 0x1000000;
			public const int WS_MAXIMIZEBOX = 0x10000;
			public const int WS_MINIMIZE = 0x20000000;
			public const int WS_MINIMIZEBOX = 0x20000;
			public const int WS_OVERLAPPED = 0;
			public const int WS_OVERLAPPEDWINDOW = 0xcf0000;
			public const int WS_POPUP = -2147483648;
			public const int WS_SYSMENU = 0x80000;
			public const int WS_TABSTOP = 0x10000;
			public const int WS_THICKFRAME = 0x40000;
			public const int WS_VISIBLE = 0x10000000;
			public const int WS_VSCROLL = 0x200000;
			public const int WSF_VISIBLE = 1;

			public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
			{
				return IntPtr.Size == 4 ? SetWindowLongPtr32(hWnd, nIndex, dwNewLong) : SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			}

			[DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
			private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

			[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
			private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		}

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DialogWindow()
		{
		}

		#endregion // Constructors



		#region Properties
		
		public static readonly DependencyProperty IsSystemMenuVisibleProperty =
			DependencyProperty.Register("IsSystemMenuVisible", typeof(bool), typeof(DialogWindow),
										new FrameworkPropertyMetadata(false, OnPropertyChanged));

		public static readonly DependencyProperty IsHelpButtonVisibleProperty =
			DependencyProperty.Register("IsHelpButtonVisible", typeof(bool), typeof(DialogWindow),
										new FrameworkPropertyMetadata(false, OnPropertyChanged));

		/// <summary>
		/// Gets or sets whether the dialog's system menu should be visible.
		/// For a dialog, the default is no system menu.
		/// </summary>
		public bool IsSystemMenuVisible
		{
			get { return (bool)GetValue(IsSystemMenuVisibleProperty); }
			set { SetValue(IsSystemMenuVisibleProperty, value); }
		}

		/// <summary>
		/// Gets or sets whether the dialog's context help button should be visible.
		/// For a dialog, the default is false.
		/// </summary>
		public bool IsHelpButtonVisible
		{
			get { return (bool)GetValue(IsHelpButtonVisibleProperty); }
			set { SetValue(IsHelpButtonVisibleProperty, value); }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Overrides the base source initialization and sets the appropriate
		/// window styles for dialogs.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			SetWindowStyle();
		}

		/// <summary>
		/// Handles when one of DialogWindow's dependency properties has changed.
		/// </summary>
		/// <param name="obj">Object instance on which the property changed.</param>
		/// <param name="e">Event args</param>
		static void OnPropertyChanged(object obj, DependencyPropertyChangedEventArgs e)
		{
			DialogWindow dialog = (DialogWindow)obj;
			dialog.SetWindowStyle();
		}

		/// <summary>
		/// Sets the appropriate window style for the dialog window based on the
		/// IsSystemMenuVisible and IsHelpButtonVisible properties.
		/// 
		/// Note: this makes several calls into native Windows methods to deliver
		/// this functionality. NativeMethods is a wrapper for native Windows 
		/// calls.
		/// </summary>
		protected virtual void SetWindowStyle()
		{
			// Gets a window handle for this dialog window.
			WindowInteropHelper wih = new WindowInteropHelper(this);
			IntPtr hwnd = wih.Handle;

			// Disable Maximize/Minimize box
			int windowStyle2 = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
			windowStyle2 &= ~NativeMethods.WS_MAXIMIZEBOX;
			windowStyle2 &= ~NativeMethods.WS_MAXIMIZE;
			windowStyle2 &= ~NativeMethods.WS_MINIMIZE;
			windowStyle2 &= ~NativeMethods.WS_MINIMIZEBOX;
			NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, new IntPtr(windowStyle2));

			// Gets the current windows StyleEx value.
			int windowStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);

			// Turns modal dialog frame on/off depending on whether we want to show 
			// the system menu.
			if (IsSystemMenuVisible)
			{
				windowStyle &= ~NativeMethods.WS_EX_DLGMODALFRAME;
			}
			else
			{
				windowStyle |= NativeMethods.WS_EX_DLGMODALFRAME;
			}

			// Turns context help on/off for the dialog depending is we want it shown.
			if (IsHelpButtonVisible)
			{
				windowStyle |= NativeMethods.WS_EX_CONTEXTHELP;
			}
			else
			{
				windowStyle &= ~NativeMethods.WS_EX_CONTEXTHELP;
			}

			// Now, sets the new windows StyleEx value.
			//NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE, new IntPtr(windowStyle));
			NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE, new IntPtr(windowStyle));

			/*if (!IsSystemMenuVisible && this.ResizeMode == ResizeMode.NoResize)
			{
				// Note: this is a workaround for a WPF bug. When NoResize is chosen,
				// the system menu doesn't get set up correctly. The code below disables
				// the appropriate system menu items in this case.
				IntPtr hmenu = NativeMethods.GetSystemMenu(hwnd, false);
				NativeMethods.EnableMenuItem(hmenu, NativeMethods.SysMenuPos_Maximize,
											 NativeMethods.MF_DISABLE | NativeMethods.MF_BYPOSITION);
				NativeMethods.EnableMenuItem(hmenu, NativeMethods.SysMenuPos_Minimize,
											 NativeMethods.MF_DISABLE | NativeMethods.MF_BYPOSITION);
				NativeMethods.EnableMenuItem(hmenu, NativeMethods.SysMenuPos_Size,
											 NativeMethods.MF_DISABLE | NativeMethods.MF_BYPOSITION);
				NativeMethods.EnableMenuItem(hmenu, NativeMethods.SysMenuPos_Restore,
											 NativeMethods.MF_DISABLE | NativeMethods.MF_BYPOSITION);

				NativeMethods.DrawMenuBar(hwnd);
			}*/
		}

		#endregion // Methods
	}
}
