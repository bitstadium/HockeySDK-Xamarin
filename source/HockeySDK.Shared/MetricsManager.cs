using System;

namespace HockeyApp
{
	public static class MetricsManager
	{
		private static readonly IPlatformMetricsManager PlatformMetricsManager = new PlatformMetricsManager();

		public static void TrackEvent(string eventName)
		{
			PlatformMetricsManager.TrackEvent(eventName);
		}
	}
}

