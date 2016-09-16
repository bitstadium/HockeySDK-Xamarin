using System;
using System.Collections.Generic;

namespace HockeyApp
{
	internal interface IPlatformMetricsManager
	{
		bool Disabled { get; set; }

		void TrackEvent(string eventName);

		void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements);
	}
}

