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

	public class TfsIteration2Vm : ViewModelBase
	{
		#region Fields

		TfsWorkItemFilterVm _parent;
		bool _isIterationsEnabled = false;
		IterationNode _selectedIteration;

		#endregion // Fields

		#region Properties

		public ObservableCollection<IterationNode> Iterations { get; private set; }

		public bool IsIterationsEnabled
		{
			get { return _isIterationsEnabled; }
			set
			{
				_isIterationsEnabled = value;
				RaisePropertyChanged(() => IsIterationsEnabled);
			}
		}

		public IterationNode SelectedIteration
		{
			get { return _selectedIteration; }
			set
			{
				_selectedIteration = value;
				RaisePropertyChanged(() => SelectedIteration);

				if (_parent.Model != null)
				{
					_parent.Model.IterationFilter.IterationId = (value == null) ? null : value.IterationId;
				}
				_parent.UpdateModels();
			}
		}

		#endregion // Properties

		#region Constructors

		public TfsIteration2Vm(TfsWorkItemFilterVm parent)
		{
			_parent = parent;
			this.Iterations = new ObservableCollection<IterationNode>();
		}

		#endregion // Constructors

		#region Methods

		public void Invalidate(TfsIterationFilter filter)
		{
			// Clear
			this.SelectedIteration = null;
			this.Iterations.Clear();

			// No teamproject = disable all
			if (filter == null)
			{
				this.IsIterationsEnabled = false; 
				return;
			}

			AddNodes(null, filter.Iterations);
			RaisePropertyChanged(() => Iterations);
			//this.IsIterationsEnabled = true;
		}

		private void AddNodes(IterationNode parent, IterationCollection iterations)
		{
			foreach (Iteration it in iterations)
			{
				var itn = new IterationNode(parent, it.Node.Id, it.Node.Name);
				if (parent == null)
				{
					this.Iterations.Add(itn);
				}
				else
				{
					parent.Add(itn);
				}
				AddNodes(itn, it.Children);
			}
		}

		public void DisplayLoadingStatus()
		{
			this.Iterations.Clear();
			this.Iterations.Add(new IterationNode(null, "Loading..."));
			this.SelectedIteration = this.Iterations.First();
			this.IsIterationsEnabled = false;
		}

		#endregion // Methods
	}

	public class IterationNode : ComboBoxTreeNode
	{
		#region Fields

		IterationNode _parent;
		ObservableCollection<ComboBoxTreeNode> _children;

		#endregion // Fields

		#region Properties

		public override ObservableCollection<ComboBoxTreeNode> Children { get { return _children; } }

		public int? IterationId { get; private set; }

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

		public IterationNode(int? iterationId, string name) : this(null, iterationId, name) { }

		public IterationNode(IterationNode parent, int? iterationId, string name)
		{
			_parent = parent;
			this.IterationId = iterationId;
			this.Name = name;
			_children = new ObservableCollection<ComboBoxTreeNode>();
		}

		#endregion // Constructors

		#region Methods

		public void Add(IterationNode node)
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
