namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.ObjectModel;

	#endregion // Using

	public abstract class OptionVm : ViewModelBase
	{
		#region Fields

		private ApplicationOptionsVm _parent;

		#endregion // Fields

		#region Properties

		protected ApplicationOptionsVm Parent { get { return _parent; }}
		public string Name { get; private set; }
		public string Description { get; private set; }
		public string Icon { get; private set; }

		#endregion // Properties

		#region Constructors

		protected OptionVm(ApplicationOptionsVm parent, string name, string description, string icon)
		{
			_parent = parent;
			this.Name = name;
			this.Description = description;
			this.Icon = icon;
		}

		#endregion // Constructors
	}
}
