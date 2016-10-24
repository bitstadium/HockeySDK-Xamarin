using System;
using HockeyApp.iOS;

namespace HockeyApp
{
	internal class PlatformCrashManager : IPlatformCrashManager
	{
		public PlatformCrashManager() {}

		public bool DidCrashInLastSession { get { return BITHockeyManager.SharedHockeyManager.CrashManager.DidCrashInLastSession; } }

		public bool TerminateOnUnobservedTaskException 
		{
			get { return BITHockeyManager.TerminateOnUnobservedTaskException; }
			set { BITHockeyManager.TerminateOnUnobservedTaskException = value; 	}
		}
	}
}

