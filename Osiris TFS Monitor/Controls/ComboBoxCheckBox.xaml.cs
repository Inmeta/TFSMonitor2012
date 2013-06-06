using System.Collections.Specialized;
using System.Diagnostics;

namespace Osiris.Tfs.Monitor
{
	#region Using

	using System.Windows;
	using System.Windows.Controls;
	using Microsoft.Windows.Controls.Ribbon;
	using System.Text;
	using System.Collections.ObjectModel;
	using System.Collections;

	#endregion // Using

	public class ObservableNodeList : ObservableCollection<ComboWithCheckBoxNode>
	{
		public ObservableNodeList()
		{
		}

		public override string ToString()
		{
			var outString = new StringBuilder();

			foreach (ComboWithCheckBoxNode s in this.Items)
			{
				if (s.IsSelected == true)
				{
					outString.Append(s.Title);
					outString.Append(',');
				}
			}
			return outString.ToString().TrimEnd(new char[] { ',' });
		}
	}

	public class ComboWithCheckBoxNode
	{
		bool _isSelected;
		public string Title { get; set; }
		public virtual bool IsSelected { get { return _isSelected; }
			set { _isSelected = value; } }

		public ComboWithCheckBoxNode() { }

		public ComboWithCheckBoxNode(string n) { Title = n; }

		public virtual void Changed() { }
	}

	public partial class ComboBoxCheckBox : IRibbonControl
	{
		#region Dependency Properties

		/// <summary> 
		/// Gets or sets a collection used to generate the content of the ComboBox 
		/// </summary> 
		public object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set
			{
				SetValue(ItemsSourceProperty, value);
				SetText();
			}
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(object), typeof(ComboBoxCheckBox), new UIPropertyMetadata(null, ItemSource_Changed));

		/// <summary> 
		/// Gets or sets the text displayed in the ComboBox 
		/// </summary> 
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(ComboBoxCheckBox), new UIPropertyMetadata());


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
			 DependencyProperty.Register("DefaultText", typeof(string), typeof(ComboBoxCheckBox), new UIPropertyMetadata(string.Empty, DefaultText_Changed));

		public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
		  "SelectedValue",
		  typeof(object),
		  typeof(ComboBoxCheckBox),
		  new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChangedCallback)
		);

		public object SelectedValue
		{
			get { return (object)GetValue(SelectedValueProperty); }
			set { SetValue(SelectedValueProperty, value); }
		}

		#endregion // Dependency Properties

		#region Constructors

		public ComboBoxCheckBox()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private static void DefaultText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as ComboBoxCheckBox;
			cb.SetText();
		}

		private static void ItemSource_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var items = e.NewValue; // as IEnumerable;

			var cb = d as ComboBoxCheckBox;
			cb.Text = (e.NewValue != null) ? e.NewValue.ToString() : cb.DefaultText;

			if (string.IsNullOrEmpty(cb.Text))
			{
				cb.Text = cb.DefaultText;
			}
		}

		/// <summary> 
		/// Whenever a CheckBox is checked, change the text displayed 
		/// </summary> 
		/// <param name="sender"></param> 
		/// <param name="e"></param> 
		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			SetText();

			var fe = sender as FrameworkElement;
			var cbNode = fe.DataContext as ComboWithCheckBoxNode;
			cbNode.Changed();
		}

		private static void OnSelectedValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as ComboBoxCheckBox;
			if (e.NewValue != null)
			{
				var item = e.NewValue as ComboWithCheckBoxNode;
				if (item != null)
				{
					cb.Text = item.Title;
				}
			}
			else
			{
				cb.Text = cb.DefaultText;
			}
		}


		/// <summary> 
		/// Set the text property of this control (bound to the ContentPresenter of the ComboBox) 
		/// </summary> 
		public void SetText()
		{
			this.Text = (this.ItemsSource != null) ? this.ItemsSource.ToString() : this.DefaultText;

			// set DefaultText if nothing else selected 
			if (string.IsNullOrEmpty(this.Text))
			{
				this.Text = this.DefaultText;
			}
		}

		private INotifyCollectionChanged _observableItemsSource;

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (_observableItemsSource == null)
			{
				_observableItemsSource = ItemsSource as INotifyCollectionChanged;
				if (_observableItemsSource != null)
				{
					_observableItemsSource.CollectionChanged += OnCollectionChanged;
				}
			}
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			if (_observableItemsSource != null)
			{
				_observableItemsSource.CollectionChanged -= OnCollectionChanged;
				_observableItemsSource = null;
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			SetText();
		}


		#endregion // Methods
	}
}


