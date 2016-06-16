using System;

namespace HockeyApp
{
	internal interface IPlatformCrashManager
	{
		bool DidCrashInLastSession { get; }
	}
}

