namespace Osiris.TFS.Monitor.Common
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;

	#endregion // Using

	public static class ScreenExstension
	{
		public static string MonitorName(this Screen screen)
		{
			for (uint id = 0; true; id++)
			{
				var d = new DISPLAY_DEVICE();
				d.cb = Marshal.SizeOf(d);
				if (!ScreenExstension.EnumDisplayDevices(null, id, ref d, 0))
				{
					break;
				}
				if (d.DeviceName == screen.DeviceName)
				{
					var d2 = new DISPLAY_DEVICE();
					d2.cb = Marshal.SizeOf(d2);
					if (ScreenExstension.EnumDisplayDevices(d.DeviceName, 0, ref d2, 0))
					{
						return d2.DeviceString;
					}
					return d.DeviceString;
				}
			}
		
			return screen.DeviceName;
		}

		[DllImport("user32.dll")]
		static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct DISPLAY_DEVICE
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cb;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceString;
		[MarshalAs(UnmanagedType.U4)]
		public DisplayDeviceStateFlags StateFlags;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceID;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceKey;
	}


	[Flags()]
	public enum DisplayDeviceStateFlags : int
	{
		/// <summary>The device is part of the desktop.</summary>
		AttachedToDesktop = 0x1,
		MultiDriver = 0x2,
		/// <summary>The device is part of the desktop.</summary>
		PrimaryDevice = 0x4,
		/// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
		MirroringDriver = 0x8,
		/// <summary>The device is VGA compatible.</summary>
		VGACompatible = 0x16,
		/// <summary>The device is removable; it cannot be the primary display.</summary>
		Removable = 0x20,
		/// <summary>The device has more display modes than its output devices support.</summary>
		ModesPruned = 0x8000000,
		Remote = 0x4000000,
		Disconnect = 0x2000000
	}
}
