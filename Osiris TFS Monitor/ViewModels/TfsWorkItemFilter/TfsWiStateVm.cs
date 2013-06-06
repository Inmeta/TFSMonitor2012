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
	/// ViewModel of TFS WiState
	/// </summary>
	public class TfsWiStateVm : ViewModelBase
	{
		private readonly TfsWorkItemFilterVm _filter;

		#region Fields

		bool _isWiStatesEnabled = false;
		WiStateItem _selectedWiState;
		//TfsWorkItemFilterVm _parent;

		#endregion // Fields

		#region Properties

		public ObservableNodeList WiStates { get; private set; }

		public bool IsWiStatesEnabled
		{
			get { return _isWiStatesEnabled; }
			set
			{
				_isWiStatesEnabled = value;
				RaisePropertyChanged(() => IsWiStatesEnabled);
			}
		}

		public WiStateItem SelectedWiState
		{
			get { return _selectedWiState; }
			set
			{
				_selectedWiState = value;
				RaisePropertyChanged(() => SelectedWiState);
			}
		}

		#endregion // Properties

		public TfsWiStateVm(TfsWorkItemFilterVm filter)
		{
			_filter = filter;
			this.WiStates = new ObservableNodeList();
		}

		public void Invalidate(TfsWiStateFilter filter)
		{
			// Clear
			this.SelectedWiState = null; 
			this.WiStates.Clear();

			// No teamproject = disable all
			if (filter  == null)
			{
				this.IsWiStatesEnabled = false; 
				return;
			}

			AddItems(filter.States);
			RaisePropertyChanged(() => WiStates);
			this.IsWiStatesEnabled = true;
		}

		private void AddItems(IEnumerable<string> states)
		{
			foreach (var state in states)
			{
				this.WiStates.Add(new WiStateItem(this, state, _filter.Model.WiStateFilter.States.Any(s => s == state)));
			}

		}

		public void DisplayLoadingStatus()
		{
			this.WiStates.Clear();
			this.WiStates.Add(new WiStateItem("Loading..."));
			this.SelectedWiState = this.WiStates.First() as WiStateItem;
			this.IsWiStatesEnabled = false;
		}

		public void Add(string state)
		{
			if (!_filter.Model.WiStateFilter.States.Any(s => s == state))
			{
				_filter.Model.WiStateFilter.States.Add(state);
			}
			_filter.UpdateModels();
		}

		public void Remove(string state)
		{
			var item = _filter.Model.WiStateFilter.States.FirstOrDefault(s => s == state);
			if (item != null)
			{
				_filter.Model.WiStateFilter.States.Remove(item);
			}
			_filter.UpdateModels();
		}
	}

	public class WiStateItem : ComboWithCheckBoxNode
	{
		private readonly TfsWiStateVm _parent;
		private readonly string _state;

		public WiStateItem(TfsWiStateVm parent, string state, bool isSelected) : base(state)
		{
			_parent = parent;
			_state = state;
			this.IsSelected = isSelected;
		}

		public WiStateItem(string name, bool isSelected = false) : base(name)
		{
			this.IsSelected = isSelected;
		}

		public override void Changed()
		{
			if (_parent != null)
			{
				if (this.IsSelected)
				{
					_parent.Add(_state);
				}
				else
				{
					_parent.Remove(_state);
				}
			}
		}
	}

}
