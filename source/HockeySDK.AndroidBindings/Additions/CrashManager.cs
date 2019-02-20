using System;
using Android.Runtime;
using HockeyApp.Android.Utils;
using System.Threading.Tasks;

namespace HockeyApp.Android
{
	public partial class CrashManager
	{
		private static bool connectedToUnhandledExceptionEvents = false;
		private static readonly object crashManagerLock = new object();
		private static bool terminateOnUnobservedTaskException;

		public static bool TerminateOnUnobservedTaskException
		{
			get { return terminateOnUnobservedTaskException; }
			set { terminateOnUnobservedTaskException = value; }
		}

		public static void Register(global::Android.Content.Context context)
		{
			DoRegister(context);
			ConnectUnhandledExceptionEvents();
		}

		public static void Register(global::Android.Content.Context context, string appIdentifier)
		{
			DoRegister(context, appIdentifier);
			ConnectUnhandledExceptionEvents();
		}

		public static void Register(global::Android.Content.Context context, string appIdentifier, global::HockeyApp.Android.CrashManagerListener listener)
		{
			DoRegister(context, appIdentifier, listener);
			ConnectUnhandledExceptionEvents(listener);
		}

		public static void Register(global::Android.Content.Context context, string urlString, string appIdentifier, global::HockeyApp.Android.CrashManagerListener listener)
		{
			DoRegister(context, urlString, appIdentifier, listener);
			ConnectUnhandledExceptionEvents(listener);
		}

		public static void Initialize(global::Android.Content.Context context, string appIdentifier, global::HockeyApp.Android.CrashManagerListener listener)
		{
			DoInitialize(context, appIdentifier, listener);
			ConnectUnhandledExceptionEvents(listener);
		}

		public static void Initialize(global::Android.Content.Context context, string urlString, string appIdentifier, global::HockeyApp.Android.CrashManagerListener listener)
		{
			DoInitialize(context, urlString, appIdentifier, listener);
			ConnectUnhandledExceptionEvents(listener);
		}

		private static void ConnectUnhandledExceptionEvents(global::HockeyApp.Android.CrashManagerListener listener = null)
		{
			if (connectedToUnhandledExceptionEvents)
			{
				HockeyLog.Debug("Crash Manager has already been registered.");
				return;
			}

			lock (crashManagerLock)
			{
				if (connectedToUnhandledExceptionEvents)
				{
					HockeyLog.Debug("Crash Manager has already been registered.");
					return;
				};

				TraceWriter.Initialize(listener);

				AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) => TraceWriter.WriteTrace(e.Exception);
				AppDomain.CurrentDomain.UnhandledException += (sender, e) => TraceWriter.WriteTrace(e.ExceptionObject);
				TaskScheduler.UnobservedTaskException += (sender, e) =>
				{
						if (!e.Observed)
						{
								TraceWriter.WriteTrace(e.Exception, terminateOnUnobservedTaskException);
						}
				};
				connectedToUnhandledExceptionEvents = true;
			}
		}
	}
}

