namespace Osiris.Tfs.Monitor
{
	#region Using

	using System.Windows;
	using System.Diagnostics;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using Microsoft.Windows.Controls.Ribbon;

	#endregion // Using

	/// <summary>
	/// ComboBox selecting items from a tree-view.
	/// 
	/// ItemsSource in this control must be of type IComboBoxTreeNode.
	/// </summary>
	public partial class ComboBoxTree : IRibbonControl
	{
		#region Fields

		ComboBoxTreeNode _selectedItem;

		#endregion // Fields

		#region Dependency Properties

		/// <summary> 
		/// Gets or sets a collection used to generate the content of the ComboBox 
		/// </summary> 
		public object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); SetText(); }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(object), typeof(ComboBoxTree), new UIPropertyMetadata(null));

		/// <summary> 
		/// Gets or sets the text displayed in the ComboBox 
		/// </summary> 
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(ComboBoxTree), new UIPropertyMetadata(string.Empty));


		/// <summary> 
		///Gets or sets the text displayed in the ComboBox if there are no selected items 
		/// </summary> 
		public string DefaultText
		{
			get { return (string)GetValue(DefaultTextProperty); }
			set { SetValue(DefaultTextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DefaultText.  This enables animation, styling, binding, etc... 
		public static readonly DependencyProperty DefaultTextProperty =
			 DependencyProperty.Register("DefaultText", typeof(string), typeof(ComboBoxTree), new UIPropertyMetadata(string.Empty, DefaultText_Changed));


		public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
			  "SelectedValue",
			  typeof(object),
			  typeof(ComboBoxTree),
			  new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChangedCallback)
		);

		public object SelectedValue
		{
			get { return (object)GetValue(SelectedValueProperty); }
			set { SetValue(SelectedValueProperty, value); }
		}

		#endregion // Dependency Properties

		#region Constructors

		public ComboBoxTree()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private static void OnSelectedValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as ComboBoxTree;
			if (e.NewValue != null)
			{
				var item = e.NewValue as ComboBoxTreeNode;
				if (item != null)
				{
					cb.Text = item.Name; //item.FullPath;
				}
			}
			else
			{
				cb.Text = cb.DefaultText;
			}
		}


		private static void DefaultText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as ComboBoxTree;
			cb.SetText();
		}

		/// <summary> 
		/// Set the text property of this control (bound to the ContentPresenter of the ComboBox) 
		/// </summary> 
		private void SetText()
		{
			this.Text = (this.ItemsSource != null) ? this.ItemsSource.ToString() : this.DefaultText;

			// Set DefaultText if nothing else selected 
			if (string.IsNullOrEmpty(this.Text))
			{
				this.Text = this.DefaultText;
			}
		}

		private void _treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			_selectedItem = e.NewValue as ComboBoxTreeNode;
			if (_selectedItem != null)
			{
				if (!_selectedItem.Selectable)
				{
					return;
				}
				this.Text = _selectedItem.Name; //_selectedItem.FullPath;
			}
			this.SelectedValue = _selectedItem;
		}

		private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			var tb = sender as TextBlock;
			var dc = tb.DataContext as ComboBoxTreeNode;
			if (dc.Selectable)
			{
				tb.TextDecorations = TextDecorations.Underline;
				tb.Foreground = (tb.DataContext == _selectedItem) ? Brushes.Blue : Brushes.Blue;
			}
		}

		private void TextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			var tb = sender as TextBlock;
			var dc = tb.DataContext as ComboBoxTreeNode;
			if (dc != null && dc.Selectable)
			{
				tb.TextDecorations = null;
				tb.Foreground = Brushes.Black;
			}
		}

		private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var tb = sender as TextBlock;
			var dc = tb.DataContext as ComboBoxTreeNode;
			if (dc != null && dc.Selectable)
			{
				TreeCombo.IsDropDownOpen = false;
			}
		}

		private void _treeView_Loaded(object sender, RoutedEventArgs e)
		{
			var tv = sender as TreeView;
			tv.Focus();
		}

		#endregion // Methods
	}

	/// <summary>
	/// Node for item in control. 
	/// 
	/// This class should be refactored away if possible... Maybe attached properties
	/// of some sort...
	/// </summary>
	public abstract class ComboBoxTreeNode
	{
		private bool _selectable = true;

		#region Properties

		public string Name { get; set; }

		public abstract ObservableCollection<ComboBoxTreeNode> Children { get; }

		public abstract string FullPath { get; }

		public bool Selectable { get { return _selectable; } protected set { _selectable = value; } }

		public string Foreground { get { return Selectable ? "Black" : "Black"; } }

		public virtual string FontWeight { get { return "Normal"; } }

		#endregion // Properties
	}

}


