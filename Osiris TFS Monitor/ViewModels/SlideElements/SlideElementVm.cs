using System;

namespace Osiris.Tfs.Monitor
{
	public abstract class SlideElementVm : ViewModelBase
	{
		public virtual void Refresh() { }
		public abstract void Unload();
	}
}