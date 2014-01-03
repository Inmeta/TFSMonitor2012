namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.Windows.Input;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Report;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using System.Net;
	using Microsoft.TeamFoundation.Client;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Windows.Threading;
	using System.ComponentModel;
	using System.Collections.ObjectModel;
	using System.Windows;
	using Microsoft.TeamFoundation.Server;

	#endregion // Using

	public class TfsAssignedToVm : ViewModelBase
	{
		private readonly TfsWorkItemFilterVm _filter;
		bool _isAssignedTosEnabled = false;
		AssignedToItem _selectedAssignedTo;

		public ObservableNodeList AssignedTos { get; private set; }

		public bool IsAssignedTosEnabled
		{
			get { return _isAssignedTosEnabled; }
			set
			{
				_isAssignedTosEnabled = value;
				RaisePropertyChanged(() => IsAssignedTosEnabled);
			}
		}

		public AssignedToItem SelectedAssignedTo
		{
			get { return _selectedAssignedTo; }
			set
			{
				_selectedAssignedTo = value;
				RaisePropertyChanged(() => SelectedAssignedTo);
			}
		}

		public TfsAssignedToVm(TfsWorkItemFilterVm filter)
		{
			_filter = filter;
			this.AssignedTos = new ObservableNodeList();
		}

		public void Invalidate(TfsAssignedToFilter filter)
		{
			// Clear
			this.SelectedAssignedTo = null; 
			this.AssignedTos.Clear();

			// No teamproject = disable all
			if (filter  == null)
			{
				this.IsAssignedTosEnabled = false;
				return;
			}

			AddItems(filter.Users);
			RaisePropertyChanged(() => AssignedTos);
			this.IsAssignedTosEnabled = true;
		}

		private void AddItems(TfsIdentityCollection assignedTos)
		{
			foreach (var id in assignedTos)
			{
				this.AssignedTos.Add(new AssignedToItem(this, id, _filter.Model.AssignedToFilter.Users.Any(u => u == id)));
			}
		}

		public void DisplayLoadingStatus()
		{
			this.AssignedTos.Clear();
			this.AssignedTos.Add(new AssignedToItem("Loading..."));
			this.SelectedAssignedTo = this.AssignedTos.First() as AssignedToItem;
			this.IsAssignedTosEnabled = false;
		}

		public void Add(string user)
		{
			if (!_filter.Model.AssignedToFilter.Users.Any(u => u == user))
			{
				_filter.Model.AssignedToFilter.Users.Add(user);
			}
			_filter.UpdateModels();
		}

		public void Remove(string user)
		{
			var item = _filter.Model.AssignedToFilter.Users.FirstOrDefault(u => u == user);
			if (item != null)
			{
				_filter.Model.AssignedToFilter.Users.Remove(item);
			}
			_filter.UpdateModels();
		}
	}

	public class AssignedToItem : ComboWithCheckBoxNode
	{
		private readonly TfsAssignedToVm _parent;
		private readonly string _name;

		public AssignedToItem(TfsAssignedToVm parent, string name, bool isSelected) : base(name)
		{
			_parent = parent;
			_name = name;
			this.IsSelected = isSelected;
		}

		public AssignedToItem(string name, bool isSelected = false) : base(name)
		{
			this.IsSelected = isSelected;
		}

		public override void Changed()
		{
			if (_parent != null)
			{
				if (this.IsSelected)
				{
					_parent.Add(_name);
				}
				else
				{
					_parent.Remove(_name);
				}
			}
		}

	}

}
