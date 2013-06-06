namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Windows.Controls.Ribbon;
	using System.Windows.Controls;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.ComponentModel;
	using System.Windows.Input;

	#endregion // Using

	public class ComboBoxAutoComplete : ComboBox, IRibbonControl
	{
		#region Fields

		private int silenceEvents = 0;
		private int start = 0, length = 0;
		//private bool ignore = false;

		#endregion // Fields

		/// <summary>
		/// Creates a new instance of <see cref="ComboBoxAutoComplete" />.
		/// </summary>
		public ComboBoxAutoComplete()
		{
			DependencyPropertyDescriptor textProperty = DependencyPropertyDescriptor.FromProperty(
				ComboBox.TextProperty, typeof(ComboBoxAutoComplete));
			textProperty.AddValueChanged(this, this.OnTextChanged);
			

			this.RegisterIsCaseSensitiveChangeNotification();
		}

		#region IsCaseSensitive Dependency Property
		/// <summary>
		/// The <see cref="DependencyProperty"/> object of the <see cref="IsCaseSensitive" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsCaseSensitiveProperty =
			DependencyProperty.Register("IsCaseSensitive", typeof(bool), typeof(ComboBoxAutoComplete), new UIPropertyMetadata(false));

		/// <summary>
		/// Gets or sets the way the combo box treats the case sensitivity of typed text.
		/// </summary>
		/// <value>The way the combo box treats the case sensitivity of typed text.</value>
		[System.ComponentModel.Description("The way the combo box treats the case sensitivity of typed text.")]
		[System.ComponentModel.Category("ComboBoxAutoComplete ComboBox")]
		[System.ComponentModel.DefaultValue(true)]
		public bool IsCaseSensitive
		{
			[System.Diagnostics.DebuggerStepThrough]
			get
			{
				return (bool)this.GetValue(IsCaseSensitiveProperty);
			}
			[System.Diagnostics.DebuggerStepThrough]
			set
			{
				this.SetValue(IsCaseSensitiveProperty, value);
			}
		}

		protected virtual void OnIsCaseSensitiveChanged(object sender, EventArgs e)
		{
			if (this.IsCaseSensitive)
				this.IsTextSearchEnabled = false;

			this.RefreshFilter();
		}

		private void RegisterIsCaseSensitiveChangeNotification()
		{
			System.ComponentModel.DependencyPropertyDescriptor.FromProperty(IsCaseSensitiveProperty, typeof(ComboBoxAutoComplete)).AddValueChanged(
				this, this.OnIsCaseSensitiveChanged);
		}
		#endregion

		/// <summary>
		/// Called when <see cref="ComboBox.ApplyTemplate()"/> is called.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.EditableTextBox != null)
			{
				this.EditableTextBox.SelectionChanged += this.EditableTextBox_SelectionChanged;
			}
		}

		/// <summary>
		/// Gets the text box in charge of the editable portion of the combo box.
		/// </summary>
		protected TextBox EditableTextBox
		{
			get
			{
				return ((TextBox)base.GetTemplateChild("PART_EditableTextBox"));
			}
		}

		private void EditableTextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("SelectionChanged");

			if (this.silenceEvents == 0)
			{
				this.start = ((TextBox)(e.OriginalSource)).SelectionStart;
				this.length = ((TextBox)(e.OriginalSource)).SelectionLength;
				this.RefreshFilter();
			}
		}

		private void RefreshFilter()
		{
			if (this.ItemsSource != null)
			{
				ICollectionView view = CollectionViewSource.GetDefaultView(this.ItemsSource);
				view.Refresh();
			}
		}

		private bool FilterPredicate(object value)
		{
			// We don't like nulls.
			if (value == null)
				return false;

			// If there is no text, there's no reason to filter.
			if (this.Text.Length == 0)
				return true;

			string prefix = this.Text;

			// If the end of the text is selected, do not mind it.
			if (this.length > 0 && this.start + this.length == this.Text.Length)
			{
				prefix = prefix.Substring(0, this.start);
			}

			return value.ToString()
				.StartsWith(prefix, !this.IsCaseSensitive, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Called when the source of an item in a selector changes.
		/// </summary>
		/// <param name="oldValue">Old value of the source.</param>
		/// <param name="newValue">New value of the source.</param>
		protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
		{
			if (newValue != null)
			{
				ICollectionView view = CollectionViewSource.GetDefaultView(newValue);
				view.Filter += this.FilterPredicate;
			}

			if (oldValue != null)
			{
				ICollectionView view = CollectionViewSource.GetDefaultView(oldValue);
				view.Filter -= this.FilterPredicate;
			}

			base.OnItemsSourceChanged(oldValue, newValue);
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			if (!this.IsTextSearchEnabled && this.silenceEvents == 0)
			{
				this.RefreshFilter();

				// Manually simulate the automatic selection that would have been
				// available if the IsTextSearchEnabled dependency property was set.
				if (this.Text.Length > 0)
				{
					foreach (object item in CollectionViewSource.GetDefaultView(this.ItemsSource))
					{
						int text = item.ToString().Length, prefix = this.Text.Length;
						this.SelectedItem = item;

						this.silenceEvents++;
						this.EditableTextBox.Text = item.ToString();
						this.EditableTextBox.Select(prefix, text - prefix);
						this.silenceEvents--;
						break;
					}
				}
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (/*e.Key == Key.Tab || */e.Key == Key.Enter)
			{
				this.silenceEvents++;
				this.EditableTextBox.Select(this.EditableTextBox.Text.Length, 0);
				this.silenceEvents--;
				e.Handled = true;
				return;
			}
			base.OnPreviewKeyDown(e);
		}
	}
}
