using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using System.Threading.Tasks;
using System.Reflection;

namespace HockeyApp.iOS
{
	public partial class BITHockeyManager
	{
		private static bool startedManager = false;
		private static readonly object setupLock= new object();
		private static bool terminateOnUnobservedTaskException;

		public static bool TerminateOnUnobservedTaskException
		{
			get { return terminateOnUnobservedTaskException; }
			set { terminateOnUnobservedTaskException = value; }
		}

		[DllImport ("libc")]
		private static extern int sigaction (Signal sig, IntPtr act, IntPtr oact);

		private enum Signal {
			SIGBUS = 10,
			SIGSEGV = 11
		}

		private BITHockeyManager () {}

		public void StartManager()
		{
			if (startedManager) return;

			lock (setupLock)
			{
				if (startedManager) return;

				var type = Type.GetType("Mono.Runtime");
				var installSignalHandlers = type?.GetMethod("InstallSignalHandlers", BindingFlags.Public | BindingFlags.Static);
				var removeSignalHandlers = type?.GetMethod("RemoveSignalHandlers", BindingFlags.Public | BindingFlags.Static);

				if (installSignalHandlers != null && removeSignalHandlers != null)
				{

					// The code is executed in a finally block, so that the Mono runtime will never abort it under any circumstance.
					try
					{
					}
					finally
					{
						removeSignalHandlers.Invoke(null, null);
						try
						{
							// Enable crash reporting libraries
							DoStartManager();

							AppDomain.CurrentDomain.UnhandledException += (sender, e) => ThrowExceptionAsNative(e.ExceptionObject);
							TaskScheduler.UnobservedTaskException += (sender, e) =>
							{
								if (terminateOnUnobservedTaskException)
								{
									ThrowExceptionAsNative(e.Exception);
								}
							};
						}
						finally
						{
							installSignalHandlers.Invoke(null, null);
						}
					}
				}
				else
				{
					IntPtr sigbus = Marshal.AllocHGlobal(512);
					IntPtr sigsegv = Marshal.AllocHGlobal(512);

					// Store Mono SIGSEGV and SIGBUS handlers
					sigaction(Signal.SIGBUS, IntPtr.Zero, sigbus);
					sigaction(Signal.SIGSEGV, IntPtr.Zero, sigsegv);

					// Enable crash reporting libraries
					DoStartManager();

					AppDomain.CurrentDomain.UnhandledException += (sender, e) => ThrowExceptionAsNative(e.ExceptionObject);
					TaskScheduler.UnobservedTaskException += (sender, e) =>
					{
						if (terminateOnUnobservedTaskException)
						{
							ThrowExceptionAsNative(e.Exception);
						}
					};

					// Restore Mono SIGSEGV and SIGBUS handlers            
					sigaction(Signal.SIGBUS, sigbus, IntPtr.Zero);
					sigaction(Signal.SIGSEGV, sigsegv, IntPtr.Zero);

					Marshal.FreeHGlobal(sigbus);
					Marshal.FreeHGlobal(sigsegv);
				}

				startedManager = true;
			}
		}

		private void ThrowExceptionAsNative(Exception exception)
		{
			ConvertToNsExceptionAndAbort (exception);
		}

		private void ThrowExceptionAsNative(object exception)
		{
			ConvertToNsExceptionAndAbort (exception);
		}

        #if __UNIFIED__
		[DllImport(global::ObjCRuntime.Constants.FoundationLibrary, EntryPoint = "NSGetUncaughtExceptionHandler")]
        #else
        [DllImport(global::MonoTouch.Constants.FoundationLibrary, EntryPoint = "NSGetUncaughtExceptionHandler")]
        #endif
		private static extern IntPtr NSGetUncaughtExceptionHandler();

		private delegate void ReporterDelegate(IntPtr ex);

//		static void ConvertToNsExceptionAndAbort(object e)
//		{
//			var nse = new NSException(".NET Exception", e.ToString(), null);
//			var uncaught = NSGetUncaughtExceptionHandler();
//			var dele = (ReporterDelegate)Marshal.GetDelegateForFunctionPointer(uncaught, typeof(ReporterDelegate));
//			dele(nse.Handle);
//		}

		private void ConvertToNsExceptionAndAbort(object e)
		{
            Console.WriteLine("ConvertToNSExceptionAndAbort");

			var name = "Managed Xamarin.iOS .NET Exception";
			var msg = e.ToString();

			var ex = e as Exception;
			if (ex != null) {
				name = ex.GetType ().FullName;
				if (ex.StackTrace != null) {
					msg = msg.Insert (msg.IndexOf('\n'), "Xamarin Exception Stack:");
					Console.WriteLine("Inserted Xamarin Exception Stack Line!");
				}
                else {
					Console.WriteLine("Could not find stacktrace!");
				}
			}
            else {
                Console.WriteLine("Could not convert to exception!");
			}
			Console.WriteLine("Name: " + name);
            Console.WriteLine("Message" + msg);

			name = name.Replace("%", "%%"); 
			msg = msg.Replace("%", "%%");
			var nse = new NSException(name, msg, null);
			var sel = new Selector("raise");
			global::Xamarin.ObjCRuntime.Messaging.void_objc_msgSend(nse.Handle, sel.Handle);
		}
	}
}

namespace Xamarin.ObjCRuntime {
    internal static class Messaging {
        const string LIBOBJC_DYLIB = "/usr/lib/libobjc.dylib";

        [DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
        internal extern static void void_objc_msgSend (IntPtr receiver, IntPtr selector);
    }
}

