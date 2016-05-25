using System;

namespace HockeyApp
{
	internal class PlatformMetricsManager : IPlatformMetricsManager
	{
		public PlatformMetricsManager() {}

		public void TrackEvent(string eventName)
		{
			throw new NotImplementedException(PCL.BaitWithoutSwitchMessage);
		}
	}
}

