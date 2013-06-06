namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;
	using Microsoft.Windows.Controls.Ribbon;
	using System.Windows.Threading;

	#endregion // Using

	/// <summary>
	/// Extend RibbonCommand to support commands. Unfortunately not through
	/// binding....
	/// 
	/// Adds property "Command" so we can bind to ICommand.
	/// </summary>
	public class RibbonCommandEx : RibbonCommand
	{
		#region Fields

		private ICommand _mappedToCommand;

		// Dependency property
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
			("Command", typeof(ICommand), typeof(RibbonCommandEx));

		#endregion // Fields

		#region Properties



		/// <summary>
		/// Command for mapping
		/// </summary>
		public ICommand Command
		{
			get
			{
				return _mappedToCommand;
			}
			set
			{
				// Mapped command cannot be null
				if (value == null)
				{
					throw new ArgumentException("RibbonCommandEx: Mapped command cannot be null.");
				}

				// Already mapped? Remove aold ref
				if (_mappedToCommand != null)
				{
					this.CanExecute -= OnCanExecute;
					this.Executed -= OnExecuted;
				}

				// Setup mapping
				_mappedToCommand = value;
				this.CanExecute += OnCanExecute;
				this.Executed += OnExecuted;
			}
		}

		#endregion // Properties

		#region Methods

		protected void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.Command is RoutedCommand && e.Source is IInputElement)
			{
				e.CanExecute = (this.Command as RoutedCommand).CanExecute(e.Parameter, e.Source as IInputElement);
			}
			else
			{
				e.CanExecute = this.Command.CanExecute(e.Parameter);
			}
			e.Handled = true;
			e.ContinueRouting = false;
		}

		protected void OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.Command is RoutedCommand && e.Source is IInputElement)
			{
				(this.Command as RoutedCommand).Execute(e.Parameter, e.Source as IInputElement);
			}
			else
			{
				this.Command.Execute(e.Parameter);
			}
			e.Handled = true;
		}

		#endregion // Methods
	}
	
	/*public class RibbonCommandEx2 : UserControl, IRibbonControl
	{
		#region Fields

		//private ICommand _mappedToCommand;

		public RibbonCommand Inner { get; private set; }


		// RibbonCommand-mapping
		public string LabelDescription { get { return (string)GetValue(LabelDescriptionProperty); } set { SetValue(LabelDescriptionProperty, value); } }
		public string LabelTitle { get { return (string)GetValue(LabelTitleProperty); } set { SetValue(LabelTitleProperty, value); } }
		public ImageSource LargeImageSource { get { return (ImageSource)GetValue(LargeImageSourceProperty); } set { SetValue(LargeImageSourceProperty, value); this.Inner.LargeImageSource = value; } }
		public ImageSource SmallImageSource { get { return (ImageSource)GetValue(SmallImageSourceProperty); } set { SetValue(SmallImageSourceProperty, value); } }
		public string ToolTipDescription { get { return (string)GetValue(ToolTipDescriptionProperty); } set { SetValue(ToolTipDescriptionProperty, value); } }
		public string ToolTipFooterDescription { get { return (string)GetValue(ToolTipFooterDescriptionProperty); } set { SetValue(ToolTipFooterDescriptionProperty, value); } }
		public ImageSource ToolTipFooterImageSource { get { return (ImageSource)GetValue(ToolTipFooterImageSourceProperty); } set { SetValue(ToolTipFooterImageSourceProperty, value); } }
		public string ToolTipFooterTitle { get { return (string)GetValue(ToolTipFooterTitleProperty); } set { SetValue(ToolTipFooterTitleProperty, value); } }
		public ImageSource ToolTipImageSource { get { return (ImageSource)GetValue(ToolTipImageSourceProperty); } set { SetValue(ToolTipImageSourceProperty, value); } }
		public string ToolTipTitle { get { return (string)GetValue(ToolTipTitleProperty); } set { SetValue(ToolTipTitleProperty, value); } }


		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
			("Command", typeof(ICommand), typeof(RibbonCommandEx2));

		public static readonly DependencyProperty LabelDescriptionProperty = DependencyProperty.Register("LabelDescription", typeof(string), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty LabelTitleProperty = DependencyProperty.Register("LabelTitle", typeof(string), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty LargeImageSourceProperty = DependencyProperty.Register("LargeImageSource", typeof(ImageSource), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty SmallImageSourceProperty = DependencyProperty.Register("SmallImageSource", typeof(ImageSource), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty ToolTipDescriptionProperty = DependencyProperty.Register("ToolTipDescription", typeof(string), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty ToolTipFooterDescriptionProperty = DependencyProperty.Register("ToolTipFooterDescription", typeof(string), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty ToolTipFooterImageSourceProperty = DependencyProperty.Register("ToolTipFooterImageSource", typeof(ImageSource), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty ToolTipFooterTitleProperty = DependencyProperty.Register("ToolTipFooterTitle", typeof(string), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty ToolTipImageSourceProperty = DependencyProperty.Register("ToolTipImageSource", typeof(ImageSource), typeof(RibbonCommandEx2));
		public static readonly DependencyProperty ToolTipTitleProperty = DependencyProperty.Register("ToolTipTitle", typeof(string), typeof(RibbonCommandEx2));




		#endregion // Fields

		#region Properties



		/// <summary>
		/// Command for mapping
		/// </summary>
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		#endregion // Properties

		public RibbonCommandEx2()
		{
			this.Inner = new RibbonCommand();
			this.AddChild(this.Inner);
		}

		#region Methods

		protected void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.Command is RoutedCommand && e.Source is IInputElement)
			{
				e.CanExecute = (this.Command as RoutedCommand).CanExecute(e.Parameter, e.Source as IInputElement);
			}
			else
			{
				e.CanExecute = this.Command.CanExecute(e.Parameter);
			}
			e.Handled = true;
			e.ContinueRouting = false;
		}

		protected void OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.Command is RoutedCommand && e.Source is IInputElement)
			{
				(this.Command as RoutedCommand).Execute(e.Parameter, e.Source as IInputElement);
			}
			else
			{
				this.Command.Execute(e.Parameter);
			}
			e.Handled = true;
		}

		#endregion // Methods
	}*/

}
