using System;

namespace HockeyApp
{
	/// <summary>
	/// The crash reporting module.
	/// </summary>
	/// <description>
	/// This is the HockeySDK module for handling crash reports.
	/// 
	/// This module works as a wrapper around the underlying crash reporting framework and provides functionality to detect new crashes, queues them if networking is not available, present a user interface to approve sending the reports to the HockeyApp servers and more.
	/// 
	/// It also provides options to add additional meta information to each crash report, like UserName, UserEmail
	/// 
	/// Crashes are send the next time the app starts. If CrashManagerStatus is set to CrashManagerStatus.AutoSend, crashes will be send without any user interaction, otherwise an alert will appear allowing the users to decide whether they want to send the report or not. This module is not sending the reports right when the crash happens deliberately, because if is not safe to implement such a mechanism while being async-safe and not causing more danger like a deadlock of the device, than helping. We found that users do start the app again because most don’t know what happened, and you will get by far most of the reports.
	/// 
	/// Sending the reports on startup is done asynchronously (non-blocking). This is the only safe way to ensure that the app won’t be possibly killed by the iOS watchdog process, because startup could take too long and the app could not react to any user input when network conditions are bad or connectivity might be very slow.
	/// 
	/// It is possible to check upon startup if the app crashed before using DidCrashInLastSession. This allows you to add additional code to your app delaying the app start until the crash has been successfully send if the crash occurred within a critical startup timeframe, e.g. after 10 seconds.
	/// </description>
	public static class CrashManager
	{
		internal static IPlatformCrashManager PlatformCrashManager = new PlatformCrashManager();

		/// <summary>
		/// Indicates if the app crash in the previous session.
		/// </summary>
		/// <value><c>true</c> if did crash in last session; otherwise, <c>false</c>.</value>
		public static bool DidCrashInLastSession { get { return PlatformCrashManager.DidCrashInLastSession; } }
		public static bool TerminateOnUnobservedTaskException 
		{ 
			get { return PlatformCrashManager.TerminateOnUnobservedTaskException; } 
			set { PlatformCrashManager.TerminateOnUnobservedTaskException = value; }
		}
	}
}

