namespace Osiris.Tfs.Monitor
{
	#region Using
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Practices.Unity;

	#endregion // Using

	/*
	public class ViewModelLocator
	{
		#region Fields

		private IUnityContainer _container;

		#endregion // Fields

		#region Properties

		public ApplicationOptionsVm ApplicationOptionsVm { get; set { return _container.Resolve<ApplicationOptionsVm>(); } }
		public ApplicationRibbonVm ApplicationRibbonVm { get { return _container.Resolve<ApplicationRibbonVm>(); } }
		public ConsoleVm ConsoleVm { get { return _container.Resolve<ConsoleVm>(); } }

		#endregion // Properties

		#region Constructors

		/// <summary>
		/// This default constructor needs to be initialized once in global resources, 
		/// for example in application.xaml.
		/// </summary>
		public ViewModelLocator()
		{
			//_container = new ViewModelContainer();
		}

		#endregion // Constructors
	}*/

	public class ViewModelLocator : UnityContainer
	{
		public ApplicationOptionsVm ApplicationOptionsVm { get { return Resolve<ApplicationOptionsVm>(); } }
		public ApplicationRibbonVm ApplicationRibbonVm { get { return Resolve<ApplicationRibbonVm>(); } }
		public ConsoleVm ConsoleVm { get { return Resolve<ConsoleVm>(); } }

		#region Constructors

		/// <summary>
		/// Initialize container with modules.
		/// </summary>
		public ViewModelLocator()
		{
			RegisterType<ApplicationVm>();
			RegisterType<ApplicationOptionsVm>();
			RegisterType<ApplicationRibbonVm>();
			RegisterType<ConsoleVm>();
		}

		#endregion // Constructors
	}

}
