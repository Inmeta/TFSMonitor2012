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
	using Osiris.Tfs.Monitor.Properties;
	using System.IO;

	#endregion // Using

	/// <summary>
	/// Factory for converting Model to ViewModel, and from Type to Model.
	/// </summary>
	public abstract class SlideElementType
	{
		#region Constructors

		protected SlideElementType()
		{
		}

		#endregion // Constructors

		#region Methods

		public abstract SlideElement CreateModel();


		public abstract SlideElementVm CreateViewModel(SlideElement model, SlideElementContainerVm container);

		public virtual SlideElementVm CreateDesignViewModel(SlideElement model, SlideElementContainerVm container) 
		{ 
			return CreateViewModel(model, container); 
		}

		public virtual object CreateTypedMenuViewModel(SlideElement model) { return null; }

		/// <summary>
		/// Factory creating viewmodel from model.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="designMode"></param>
		/// <returns></returns>
		public static SlideElementVm CreateViewModel(SlideElementContainerVm container, SlideElement model, bool designMode)
		{
			var type = CreateTypeFromModel(model);
			if (type != null)
			{
				if (!designMode)
				{
					return type.CreateViewModel(model, container);
				}
				else
				{
					return type.CreateDesignViewModel(model, container);
				}
			}
			return null;
		}
		
		/// <summary>
		/// Create menu-viewmodel from model.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static object CreateMenuViewModel(SlideElement model)
		{
			var type = CreateTypeFromModel(model);
			if (type != null)
			{
				return type.CreateTypedMenuViewModel(model);
			}
			return null;
		}

		/// <summary>
		/// Create type from model
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private static SlideElementType CreateTypeFromModel(SlideElement model)
		{
			if (model is WebPage)
			{
				return new WebPageType(); 
			}
			if (model is BurndownChart)
			{
				return new BurndownChartType();
			}
			if (model is BuildMonitor)
			{
				return new BuildMonitorType(); 
			}
			if (model is TaskManager)
			{
				return new TaskManagerType();
			}
			if (model is Twitter)
			{
				return new TwitterType();
			}

			// Instance null or type not supported
			Debug.Assert(false);
			return null;
		}


		#endregion // Methods
	}


	public class WebPageType : SlideElementType
	{
		#region Constructors

		public WebPageType() : base()
		{
		}

		#endregion // Constructors

		#region Methods

		public override SlideElement CreateModel()
		{
			return new WebPage();
		}

		public override SlideElementVm CreateViewModel(SlideElement model, SlideElementContainerVm container)
		{
			return new WebPageVm(container, model as WebPage);
		}

		public override SlideElementVm CreateDesignViewModel(SlideElement model, SlideElementContainerVm container)
		{
			return new WebPageDesignVm(model as WebPage);
		}

		public override object CreateTypedMenuViewModel(SlideElement model)
		{
			return new WebPageMenuVm(model as WebPage);
		}

		#endregion // Methods
	}

	public class BurndownChartType : SlideElementType
	{
		#region Constructors

		public BurndownChartType() : base()
		{
		}

		#endregion // Constructors
	
		#region Methods

		public override SlideElement CreateModel()
		{
			return new BurndownChart();
		}

		public override SlideElementVm CreateViewModel(SlideElement model, SlideElementContainerVm container)
		{
			return new BurndownChartVm(model as BurndownChart, container.Parent.EnablePolling, container.Parent.AutoDispose);
		}

		public override object CreateTypedMenuViewModel(SlideElement model)
		{
			return new BurndownChartMenuVm(model as BurndownChart);
		}

		#endregion // Methods
	}

	public class BuildMonitorType : SlideElementType
	{
		#region Properties

		#endregion // Properties

		#region Constructors

		public BuildMonitorType() 
		{
		}

		#endregion // Constructors

		#region Methods

		public override SlideElement CreateModel()
		{
			return new BuildMonitor();
		}

		public override SlideElementVm CreateViewModel(SlideElement model, SlideElementContainerVm container)
		{
			return new BuildMonitorVm(model as BuildMonitor, container.Parent.EnablePolling, container.Parent.AutoDispose);
		}

		public override object CreateTypedMenuViewModel(SlideElement model)
		{
			return new BuildMonitorMenuVm(model as BuildMonitor);
		}

		#endregion // Methods
	}

	public class TaskManagerType : SlideElementType
	{
		#region Properties

		#endregion // Properties

		#region Constructors

		public TaskManagerType()
		{
		}

		#endregion // Constructors

		#region Methods

		public override SlideElement CreateModel()
		{
			return new TaskManager();
		}

		public override SlideElementVm CreateViewModel(SlideElement model, SlideElementContainerVm container)
		{
			return new TaskManagerVm(model as TaskManager);
		}

		public override object CreateTypedMenuViewModel(SlideElement model)
		{
			return new TaskManagerMenuVm(model as TaskManager);
		}

		#endregion // Methods
	}

	public class TwitterType : SlideElementType
	{
		#region Properties

		#endregion // Properties

		#region Constructors

		public TwitterType()
		{
		}

		#endregion // Constructors

		#region Methods

		public override SlideElement CreateModel()
		{
			return new Twitter();
		}

		public override SlideElementVm CreateViewModel(SlideElement model, SlideElementContainerVm container)
		{
			return new TwitterVm(model as Twitter, container.Parent.EnablePolling, container.Parent.AutoDispose);
		}

		public override object CreateTypedMenuViewModel(SlideElement model)
		{
			return new TwitterMenuVm(model as Twitter);
		}

		#endregion // Methods
	}

}
