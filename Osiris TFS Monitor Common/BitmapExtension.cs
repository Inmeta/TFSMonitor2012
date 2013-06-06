using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Osiris.Tfs.Monitor
{
	public static class BitmapExtension
	{
		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject); 

		public static BitmapSource ToBitmapSource(this Bitmap source)
		{
			BitmapSource bitSrc = null;
			var hBitmap = source.GetHbitmap();

			try
			{
				bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					hBitmap,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());
				bitSrc.Freeze();
			}
			catch (Win32Exception)
			{
				bitSrc = null;
			}
			finally
			{
				DeleteObject(hBitmap);
			}

			return bitSrc; 

		}
	}
}