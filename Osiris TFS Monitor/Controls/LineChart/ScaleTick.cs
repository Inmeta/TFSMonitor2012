namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	/// <summary>
	/// Class for getting best-rounded ticks.
	/// </summary>
	public static class ScaleTick
	{
		#region Methods

		/// <summary>
		/// Get rounded ticks.
		/// </summary>
		/// <param name="maxValue"></param>
		/// <param name="numTicks"></param>
		/// <returns></returns>
		public static double GetTick(double maxValue, out int numTicks)
		{
			numTicks = 1;
			double optiMax = maxValue * 2;
			for (int i = 5; i <= 10; i++)
			{
				var tmpMaxValue = BestTick(maxValue, i);
				if ((optiMax > tmpMaxValue) && (tmpMaxValue > (maxValue + maxValue * 0.05)))
				{
					optiMax = tmpMaxValue;
					numTicks = i;
				}
			}
			return optiMax;
		}

		private static double BestTick(double maxValue, int mostTicks)
		{
			double tick;
			var minimum = maxValue / mostTicks;
			var magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));
			var residual = minimum / magnitude;
			if (residual > 5)
			{
				tick = 10 * magnitude;
			}
			else if (residual > 2)
			{
				tick = 5 * magnitude;
			}
			else if (residual > 1)
			{
				tick = 2 * magnitude;
			}
			else
			{
				tick = magnitude;
			}
			return (tick * mostTicks);
		}

		#endregion // Methods
	}
}
