using System;
using HockeyApp.iOS;

namespace HockeyApp
{
	internal class PlatformMetricsManager : IPlatformMetricsManager
	{
		public PlatformMetricsManager() {}

		public bool Disabled
		{
			get { return BITHockeyManager.SharedHockeyManager.MetricsManager.Disabled; }
			set { BITHockeyManager.SharedHockeyManager.MetricsManager.Disabled = value; }
		}

		public void TrackEvent(string eventName)
		{
			BITHockeyManager.SharedHockeyManager.MetricsManager.TrackEvent(eventName);
		}
	}
}

