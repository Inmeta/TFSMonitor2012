namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Data;
	using System.Windows.Controls;
	using Microsoft.Windows.Controls.Ribbon;
	using System.Windows;
	using System.Diagnostics;

	#endregion // Using

	/// <summary>
	/// ComboBox which can be edited for selecting duration in seconds,
	/// minutes or hours.
	/// 
	/// The property duration is bindable and holds a nulleable int which
	/// is the duration selected.
	/// 
	/// If value is null nothing is selected, if less than zero, the value
	/// is not valid.
	/// 
	/// The input is parsed and will validate evrytime userinput is typed.
	/// 
	/// Valid formats:
	/// 
	/// <no>			: Seconds
	/// <no> s			: ---=---
	/// <no> sec		: ---=---
	/// <no> second		: ---=---
	/// <no> seconds	: ---=---
	/// <no> m			: Minutes
	/// <no> min		: ---=---
	/// <no> minute		: ---=---
	/// <no> minutes	: ---=---
	/// 
	/// </summary>
	public class RibbonComboBoxDuration : ComboBox, IRibbonControl
	{
		#region Fields

		Binding _durBinding;
		string _duration;

		public static readonly DependencyProperty SelectedValueExProperty = DependencyProperty.Register(
		  "SelectedValueEx",
		  typeof(int?),
		  typeof(RibbonComboBoxDuration),
		  new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueExChangedCallback)
		);

		#endregion // Fields

		#region Properties
		
		public int? SelectedValueEx
		{
			get { return (int?)this.GetValue(SelectedValueExProperty); }
			set { this.SetValue(SelectedValueExProperty, value); }
		}
		
		public string Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				this.SetValue(SelectedValueExProperty, DurationValidationRule.Parse(value)); 
				_duration = value;
			}
		}

		public string BindingSource { get; set; }

		#endregion // Properties

		#region Constructors

		public RibbonComboBoxDuration() : base()
		{
			// Add selectable values
			AddItem(" 5 seconds");
			AddItem("10 seconds");
			AddItem("30 seconds");
			AddItem(" 1 minute");
			AddItem(" 5 minutes");
		}

		#endregion // Constructors

		#region Methods

		protected void AddItem(object content)
		{
			var item = new ComboBoxItem();
			item.Content = content;
			item.DataContext = this;
			this.Items.Add(item);
		}

		private string Encode(int? value)
		{
			if (value == null || value.Value < 0)
			{
				return "";
			}

			string suffix;

			var rem = decimal.Remainder(value.Value, 60);
			if (rem != 0 || value.Value < 60)
			{
				suffix = "second";
			}
			else
			{
				value = value.Value / 60;
				suffix = "minute";
			}

			return value.Value.ToString() + " " + suffix + (value.Value > 1 ? "s" : "");
		}

		

		private static void OnSelectedValueExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var rcb = d as RibbonComboBoxDuration;
			rcb.EnableDurationBinding(e.NewValue != null);
			rcb.Text = rcb.Encode(e.NewValue as int?);
		}

		private void EnableDurationBinding(bool enable)
		{
			if (!enable)
			{
				if (_durBinding == null)
				{
					return;
				}
				BindingOperations.ClearBinding(this, ComboBox.TextProperty);
				_durBinding = null;
				return;
			}
			else
			{
				if (_durBinding != null)
				{
					return;
				}

				_durBinding = new Binding(/*this.BindingSource*/ "Duration");
				_durBinding.Source = this;
				_durBinding.Mode = BindingMode.TwoWay;
				_durBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
				_durBinding.ValidationRules.Add(new RequiredFieldValidationRule("Value cannot be empty."));
				_durBinding.ValidationRules.Add(new DurationValidationRule("Invalid duration format."));
			}
			this.SetBinding(ComboBox.TextProperty, _durBinding);
		}

		#endregion // Methods
	}
}
