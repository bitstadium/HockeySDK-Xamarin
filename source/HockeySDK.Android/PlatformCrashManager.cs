using System;
using HockeyAndroid = HockeyApp.Android;

namespace HockeyApp
{
	internal class PlatformCrashManager : IPlatformCrashManager
	{
		public PlatformCrashManager() {}

		public bool DidCrashInLastSession { get { return HockeyAndroid.CrashManager.DidCrashInLastSession(); } }
	}
}

