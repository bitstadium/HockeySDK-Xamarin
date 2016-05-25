using System;
using HockeyApp.iOS;

namespace HockeyApp
{
	internal class PlatformMetricsManager : IPlatformMetricsManager
	{
		public PlatformMetricsManager() {}

		public void TrackEvent(string eventName)
		{
			BITHockeyManager.SharedHockeyManager.MetricsManager.TrackEvent(eventName);
		}
	}
}

