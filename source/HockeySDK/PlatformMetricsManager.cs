using System;
using System.Collections.Generic;

namespace HockeyApp
{
	internal class PlatformMetricsManager : IPlatformMetricsManager
	{
		public PlatformMetricsManager() {}

		public bool Disabled
		{
			get { throw new NotImplementedException(PCL.BaitWithoutSwitchMessage); }
			set { throw new NotImplementedException(PCL.BaitWithoutSwitchMessage); }
		}

		public void TrackEvent(string eventName)
		{
			throw new NotImplementedException(PCL.BaitWithoutSwitchMessage);
		}

		public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
		{
			throw new NotImplementedException(PCL.BaitWithoutSwitchMessage);
		}
	}
}

