using System;
using HockeyAndroid = HockeyApp.Android;

namespace HockeyApp
{
	internal class PlatformCrashManager : IPlatformCrashManager
	{
		public PlatformCrashManager() {}

		public bool DidCrashInLastSession { get { return (Boolean) HockeyAndroid.CrashManager.DidCrashInLastSession().Get(); } }

		public bool TerminateOnUnobservedTaskException {
		
			get { return HockeyAndroid.CrashManager.TerminateOnUnobservedTaskException; }
			set { HockeyAndroid.CrashManager.TerminateOnUnobservedTaskException = value; }
		}
	}
}

