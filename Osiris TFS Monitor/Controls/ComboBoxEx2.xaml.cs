namespace Osiris.Tfs.Monitor
{
	#region Using

	using System.Windows;
	using System.Windows.Controls;
	using Microsoft.Windows.Controls.Ribbon;
	using System.Diagnostics;

	#endregion // Using

	public partial class ComboBoxEx2 : IRibbonControl
	{
		#region Dependency Properties

		/// <summary> 
		/// Gets or sets a collection used to generate the content of the ComboBox 
		/// </summary> 
		public object ItemsSource
		{
			get { return GetValue(ItemsSourceProperty); }
			set
			{
				SetValue(ItemsSourceProperty, value);
				//SetText();
			}
		}

		public bool IsEditable
		{
			get { return (bool)GetValue(IsEditableProperty); }
			set
			{
				SetValue(IsEditableProperty, value);
				//SetText();
			}
		}

		/*public string DisplayMemberPath
		{
			get { return (string)GetValue(IsEditableProperty); }
			set
			{
				SetValue(DisplayMemberPathProperty, value);
			}
		}*/

		/*public object SelectedValue
		{
			get { return GetValue(SelectedValueProperty); }
			set
			{
				SetValue(SelectedValueProperty, value);
			}
		}*/

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(object), typeof(ComboBoxEx2), new UIPropertyMetadata(null));

		public static readonly DependencyProperty IsEditableProperty =
			DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboBoxEx2), new UIPropertyMetadata(null));

		/*public static readonly DependencyProperty DisplayMemberPathProperty =
			DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ComboBoxEx2), new UIPropertyMetadata(null));*/

		/*public static readonly DependencyProperty SelectedValueProperty =
			DependencyProperty.Register("SelectedValue", typeof(object), typeof(ComboBoxEx2), new UIPropertyMetadata(null));*/

		/// <summary> 
		/// Gets or sets the text displayed in the ComboBox 
		/// </summary> 
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set {
				Debug.WriteLine("Tekst: '" + value + "'");
				SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(ComboBoxEx2), new UIPropertyMetadata(string.Empty, Text_Changed));


		/// <summary> 
		///Gets or sets the text displayed in the ComboBox if there are no selected items 
		/// </summary> 
		/*public string DefaultText
		{
			get { return (string)GetValue(DefaultTextProperty); }
			set { SetValue(DefaultTextProperty, value); }
		}*/

		// Using a DependencyProperty as the backing store for DefaultText.  This enables animation, styling, binding, etc... 
		/*public static readonly DependencyProperty DefaultTextProperty =
			 DependencyProperty.Register("DefaultText", typeof(string), typeof(ComboBoxEx2), new UIPropertyMetadata(string.Empty, DefaultText_Changed));*/

		#endregion // Dependency Properties

		#region Constructors

		public ComboBoxEx2()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private static void Text_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//var cb = d as ComboBoxEx2;
			//cb.SetText();
		}

		/*private static void DefaultText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as ComboBoxEx2;
			cb.SetText();
		}*/

		/// <summary> 
		/// Set the text property of this control (bound to the ContentPresenter of the ComboBox) 
		/// </summary> 
		/*private void SetText()
		{
			
			this.Text = (this.ItemsSource != null) ? this.ItemsSource.ToString() : this.DefaultText;

			// set DefaultText if nothing else selected 
			if (string.IsNullOrEmpty(this.Text))
			{
				this.Text = this.DefaultText;
			}
			
		}*/

		#endregion // Methods
	}
}


