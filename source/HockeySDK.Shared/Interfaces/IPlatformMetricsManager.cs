using System;

namespace HockeyApp
{
	internal interface IPlatformMetricsManager
	{
		void TrackEvent(string eventName);
	}
}

