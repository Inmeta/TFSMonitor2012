namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Input;
	using System.Linq.Expressions;

	#endregion // Using

	/// <summary>
	/// Provides common functionality for ViewModel classes
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion // Events

		#region Methods

		/// <summary>
		/// Raise PropertyChanged with static property-name.
		/// </summary>
		/// <param name="propertyName"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaisePropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Allows raising PropertyChanged with dynamic property-name reference
		/// through lamda expression.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="exp"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaisePropertyChanged<T>(Expression<Func<T>> exp)
		{
			// The cast will always succeed  
			var memberExpression = (MemberExpression)exp.Body;
			var propertyName = memberExpression.Member.Name;
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion // Methods
	}
}
