namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;

	#endregion // Using

	public static class DependencyObjectExtensions
	{
		#region Methods

		/// <summary>
		/// Finds visual child.
		/// </summary>
		/// <typeparam name="childItem"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static childItem FindVisualChild<childItem>(this DependencyObject obj)
		   where childItem : DependencyObject
		{
		   // Search immediate children first (breadth-first)
		   for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
		   {
			  DependencyObject child = VisualTreeHelper.GetChild(obj, i);

			  if (child != null && child is childItem)
			  {
				  return (childItem)child;
			  }
			  else
			  {
				  childItem childOfChild = FindVisualChild<childItem>(child);

				  if (childOfChild != null)
				  {
					  return childOfChild;
				  }
			  }
		   }

		   return null;
		}

		#endregion // Methods
	}

}