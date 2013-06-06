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
	/// ViewModel of TFS area
	/// </summary>
	public class TfsAreaVm : ViewModelBase
	{
		#region Fields

		bool _isAreasEnabled = false;
		AreaNode _selectedArea;
		//TfsWorkItemFilterVm _parent;

		#endregion // Fields

		#region Properties

		public ObservableCollection<AreaNode> Areas { get; private set; }

		public bool IsAreasEnabled
		{
			get { return _isAreasEnabled; }
			set
			{
				_isAreasEnabled = value;
				RaisePropertyChanged(() => IsAreasEnabled);
			}
		}

		public AreaNode SelectedArea
		{
			get { return _selectedArea; }
			set
			{
				_selectedArea = value;
				RaisePropertyChanged(() => SelectedArea);
			}
		}

		#endregion // Properties

		#region Constructors

		public TfsAreaVm()
		{
			//_parent = parent;
			this.Areas = new ObservableCollection<AreaNode>();
		}

		#endregion // Constructors

		#region Methods

		public void Invalidate(TfsAreaFilter filter)
		{
			// Clear
			this.SelectedArea = null;
			this.Areas.Clear();

			// No teamproject = disable all
			if (filter  == null)
			{
				this.IsAreasEnabled = false; 
				return;
			}

			AddNodes(null, filter.Areas);
			RaisePropertyChanged(() => Areas);
			//this.IsAreasEnabled = true;
		}

		private void AddNodes(AreaNode parent, AreaCollection areas)
		{
			foreach (Area a in areas)
			{
				var an = new AreaNode(parent, a.Node.Name);
				if (parent == null)
				{
					this.Areas.Add(an);
				}
				else
				{
					parent.Add(an);
				}
				AddNodes(an, a.Children);
			}
		}

		public void DisplayLoadingStatus()
		{
			this.Areas.Clear();
			this.Areas.Add(new AreaNode("Loading..."));
			this.SelectedArea = this.Areas.First();
			this.IsAreasEnabled = false;
		}

		#endregion // Methods
	}

	public class AreaNode : ComboBoxTreeNode
	{
		#region Fields

		AreaNode _parent;
		ObservableCollection<ComboBoxTreeNode> _children;

		#endregion // Fields

		#region Properties

		public override ObservableCollection<ComboBoxTreeNode> Children { get { return _children; } }

		//public override string Name { get; set; }

		public override string FullPath
		{
			get
			{
				if (_parent == null)
				{
					return this.Name;
				}
				return _parent.Concat(this.Name);
			}
		}

		#endregion // Properties

		#region Constructors

		public AreaNode(string name) : this(null, name) { }

		public AreaNode(AreaNode parent, string name)
		{
			_parent = parent;
			this.Name = name;
			_children = new ObservableCollection<ComboBoxTreeNode>();
		}

		#endregion // Constructors

		#region Methods

		public void Add(AreaNode node)
		{
			_children.Add(node);
		}

		private string Concat(string path)
		{
			if (_parent == null)
			{
				return this.Name + "/" + path;
			}
			return _parent.Concat(this.Name + "/" + path);
		}

		#endregion // Methods
	}

}
