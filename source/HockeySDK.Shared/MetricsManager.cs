using System;

namespace HockeyApp
{
	/// <summary>
	/// This is the HockeySDK module that handles users, sessions and events tracking.
	/// </summary>
	/// <description>
	/// Unless disabled, this module automatically tracks users and session of your app to give you better insights about how your app is being used. Users are tracked in a completely anonymous way without collecting any personally identifiable information.
	/// 
	/// Before starting to track events, ask yourself the questions that you want to get answers to. For instance, you might be interested in business, performance/quality or user experience aspects. Name your events in a meaningful way and keep in mind that you will use these names when searching for events in the HockeyApp web portal.
	/// 
	/// It is your reponsibility to not collect personal information as part of the events tracking or get prior consent from your users as necessary.
	/// </description>
	public static class MetricsManager
	{
		private static readonly IPlatformMetricsManager PlatformMetricsManager = new PlatformMetricsManager();

#if __ANDROID__
		/// <summary>
		/// A property indicating whether User Metrics and Session Tracking is disabled.
		/// </summary>
		/// <value><c>true</c> if User Metrics and Session Tracking are both disabled; otherwise, <c>false</c>.</value>
#else
		/// <summary>
		/// A property indicating whether the MetricsManager instance is disabled.
		/// </summary>
		/// <value><c>true</c> if disabled; otherwise, <c>false</c>.</value>
#endif
		public static bool Disabled
		{
			get { return PlatformMetricsManager.Disabled; }
			set { PlatformMetricsManager.Disabled = value; }
		}

		/// <summary>
		/// This method allows to track an event that happened in your app. Remember to choose meaningful event names to have the best experience when diagnosing your app in the HockeyApp web portal.
		/// </summary>
		/// <param name="eventName">The event’s name as a string.</param>
		public static void TrackEvent(string eventName)
		{
			PlatformMetricsManager.TrackEvent(eventName);
		}
	}
}

