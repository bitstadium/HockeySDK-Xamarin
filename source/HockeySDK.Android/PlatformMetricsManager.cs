using System;
using HockeyMetrics = HockeyApp.Android.Metrics;

namespace HockeyApp
{
	internal class PlatformMetricsManager : IPlatformMetricsManager
	{
		public PlatformMetricsManager() {}

		public void TrackEvent(string eventName)
		{
			HockeyMetrics.MetricsManager.TrackEvent(eventName);
		}
	}
}

