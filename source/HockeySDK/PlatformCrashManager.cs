using System;

namespace HockeyApp
{
	internal class PlatformCrashManager : IPlatformCrashManager
	{
		public PlatformCrashManager() {}

		public bool DidCrashInLastSession { get { throw new NotImplementedException(PCL.BaitWithoutSwitchMessage); } }
	}
}

