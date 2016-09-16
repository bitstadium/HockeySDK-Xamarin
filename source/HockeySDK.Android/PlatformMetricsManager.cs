using System;
using System.Collections.Generic;
using HockeyMetrics = HockeyApp.Android.Metrics;

namespace HockeyApp
{
	internal class PlatformMetricsManager : IPlatformMetricsManager
	{
		public PlatformMetricsManager() {}

		private bool _disabled;
		public bool Disabled
		{
			get {
				var cachedDisabled = _disabled;

				try
				{
					_disabled = !HockeyMetrics.MetricsManager.IsUserMetricsEnabled && !HockeyMetrics.MetricsManager.SessionTrackingEnabled();
				}
				catch
				{
					// if MetricsManager has not been yet registered, then return set property
					_disabled = cachedDisabled;
				}

				return _disabled;
			}
			set {
				try
				{
					if (value)
					{
						HockeyMetrics.MetricsManager.DisableUserMetrics();
					}
					else
					{
						HockeyMetrics.MetricsManager.EnableUserMetrics();
					}
				}
				/*
				 * If MetricsManager is not registered, will throw an exception.
				 * No current way to check if MetricsManager has yet registered, so consume try/catch.
				 */
				catch {}
				HockeyMetrics.MetricsManager.SetSessionTrackingDisabled(new Java.Lang.Boolean(value));

				_disabled = value;
			}
		}

		public void TrackEvent(string eventName)
		{
			HockeyMetrics.MetricsManager.TrackEvent(eventName);
		}

		public void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
		{
			Dictionary<string, Java.Lang.Double> measurementsHelper = null;
			if (measurements != null)
			{
				measurementsHelper = new Dictionary<string, Java.Lang.Double>();
				foreach (KeyValuePair<string, double> pair in measurements)
				{
					measurementsHelper.Add(pair.Key, new Java.Lang.Double(pair.Value));
				}
			}
			
			HockeyMetrics.MetricsManager.TrackEvent(eventName, properties, measurementsHelper);
		}

	}
}

