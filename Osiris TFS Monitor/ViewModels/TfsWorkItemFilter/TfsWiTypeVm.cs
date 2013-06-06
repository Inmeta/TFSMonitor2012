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

	#endregion // Using

	/// <summary>
	/// ViewModel of TFS WiType
	/// </summary>
	public class TfsWiTypeVm : ViewModelBase
	{
		private TfsWorkItemFilterVm _filter;
		bool _isWiTypesEnabled = false;
		WiTypeItem _selectedWiType;

		#region Properties

		public ObservableNodeList WiTypes { get; private set; }

		public bool IsWiTypesEnabled
		{
			get { return _isWiTypesEnabled; }
			set
			{
				_isWiTypesEnabled = value;
				RaisePropertyChanged(() => IsWiTypesEnabled);
			}
		}

		public WiTypeItem SelectedWiType
		{
			get { return _selectedWiType; }
			set
			{
				_selectedWiType = value;
				RaisePropertyChanged(() => SelectedWiType);
			}
		}

		#endregion // Properties

		#region Constructors

		public TfsWiTypeVm(TfsWorkItemFilterVm filter)
		{
			_filter = filter;
			this.WiTypes = new ObservableNodeList();
		}

		#endregion // Constructors

		public void Invalidate(TfsWiTypeFilter filter)
		{
			// Clear
			this.SelectedWiType = null; 
			this.WiTypes.Clear();

			// No teamproject = disable all
			if (filter == null)
			{
				this.IsWiTypesEnabled = false;
				return;
			}

			AddItems(filter.WiTypes);
			RaisePropertyChanged(() => WiTypes);
			this.IsWiTypesEnabled = true;
		}

		private void AddItems(IEnumerable<WorkItemTypeEx> wiTypes)
		{
			if (_filter.Model != null)
			{
				foreach (WorkItemTypeEx wit in wiTypes)
				{
					this.WiTypes.Add(new WiTypeItem(this, wit, _filter.Model.WiTypeFilter.WiTypes.Any(t => t.Name == wit.Name)));
				}
			}
		}

		public void DisplayLoadingStatus()
		{
			this.WiTypes.Clear();
			this.WiTypes.Add(new WiTypeItem("Loading..."));
			this.SelectedWiType = this.WiTypes.First() as WiTypeItem;
			this.IsWiTypesEnabled = false;
		}

		public void Add(WorkItemTypeEx wit)
		{
			if (!_filter.Model.WiTypeFilter.WiTypes.Any(t => t.Name == wit.Name))
			{
				_filter.Model.WiTypeFilter.WiTypes.Add(wit);
			}
			_filter.UpdateModels();
		}

		public void Remove(WorkItemTypeEx wit)
		{
			var w = _filter.Model.WiTypeFilter.WiTypes.FirstOrDefault(t => t.Name == wit.Name);
			if (w != null)
			{
				_filter.Model.WiTypeFilter.WiTypes.Remove(w);
			}
			_filter.UpdateModels();
		}
	}

	public class WiTypeItem : ComboWithCheckBoxNode
	{
		private readonly TfsWiTypeVm _parent;
		private readonly WorkItemTypeEx _wit;

		public WiTypeItem(TfsWiTypeVm parent, WorkItemTypeEx wit, bool isSelected)
			: base(wit.Name)
		{
			_parent = parent;
			_wit = wit;
			this.IsSelected = isSelected;
		}

		public WiTypeItem(string name, bool isSelected = false) : base(name)
		{
			this.IsSelected = isSelected;
		}

		public override void Changed()
		{
			if (_parent != null)
			{
				if (this.IsSelected)
				{
					_parent.Add(_wit);
				}
				else
				{
					_parent.Remove(_wit);
				}

			}
		}

	}

}
