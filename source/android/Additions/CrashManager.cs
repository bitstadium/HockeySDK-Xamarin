using System;
using Android.Runtime;
using System.Threading.Tasks;

namespace HockeyApp
{
	public partial class CrashManager
	{
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

		public static void Register(global::Android.Content.Context context, string appIdentifier, global::HockeyApp.CrashManagerListener listener)
		{
			DoRegister(context, appIdentifier, listener);
			ConnectUnhandledExceptionEvents();
		}
			
		public static void Register(global::Android.Content.Context context, string urlString, string appIdentifier, global::HockeyApp.CrashManagerListener listener)
		{
			DoRegister(context, urlString, appIdentifier, listener);
			ConnectUnhandledExceptionEvents();
		}

		private static void ConnectUnhandledExceptionEvents()
		{
			TraceWriter.Initialize();

			AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) => {
				TraceWriter.WriteTrace(e.Exception);
				e.Handled = true;
			};

			AppDomain.CurrentDomain.UnhandledException += (sender, e) => TraceWriter.WriteTrace(e.ExceptionObject);;

			TaskScheduler.UnobservedTaskException += (sender, e) => TraceWriter.WriteTrace(e.Exception);
		}
	}
}

