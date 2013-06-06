namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	/// <summary>
	/// Wrapper for Win32 window-handle
	/// </summary>
	public class Win32Window : System.Windows.Forms.IWin32Window
	{
		public Win32Window(IntPtr handle)
		{
			_hwnd = handle;
		}

		public IntPtr Handle
		{
			get { return _hwnd; }
		}

		private IntPtr _hwnd;
	}
}
