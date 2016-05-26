using System;

namespace HockeyApp
{
	internal interface IPlatformMetricsManager
	{
		bool Disabled { get; set; }

		void TrackEvent(string eventName);
	}
}

