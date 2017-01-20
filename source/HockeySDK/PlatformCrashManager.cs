using System;

namespace HockeyApp
{
	internal class PlatformCrashManager : IPlatformCrashManager
	{
		public PlatformCrashManager() {}

		public bool DidCrashInLastSession { get { throw new NotImplementedException(PCL.BaitWithoutSwitchMessage); } }

		public bool TerminateOnUnobservedTaskException {
			get { throw new NotImplementedException(PCL.BaitWithoutSwitchMessage); }
			set { throw new NotImplementedException(PCL.BaitWithoutSwitchMessage); }
		}
	}
}

