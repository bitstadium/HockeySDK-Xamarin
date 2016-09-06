using System;
using System.Collections.Generic;
using HockeyApp.iOS;
using Foundation;

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

		public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
		{
			var propertiesHelper = new NSMutableDictionary();
			foreach (KeyValuePair<string, string> pair in properties)
			{
				propertiesHelper.Add(NSString.FromObject(pair.Key), NSString.FromObject(pair.Value));
			}

			var measurementsHelper = new NSMutableDictionary();
			foreach (KeyValuePair<string, double> pair in measurements)
			{
				measurementsHelper.Add(NSString.FromObject(pair.Key), NSNumber.FromDouble(pair.Value));
			}


			BITHockeyManager.SharedHockeyManager.MetricsManager.TrackEvent(eventName, 
			                                                               propertiesHelper,
			                                                               measurementsHelper);
		}

		
	}
}

