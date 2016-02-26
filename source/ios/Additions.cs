using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

namespace HockeyApp
{
	public partial class Setup
	{
		public Setup ()
		{
		}

		[DllImport ("libc")]
		private static extern int sigaction (Signal sig, IntPtr act, IntPtr oact);

		enum Signal {
			SIGBUS = 10,
			SIGSEGV = 11
		}

		public static void EnableCustomCrashReporting (Action customCrashReportingEnableCode)
		{
			IntPtr sigbus = Marshal.AllocHGlobal (512);
			IntPtr sigsegv = Marshal.AllocHGlobal (512);

			// Store Mono SIGSEGV and SIGBUS handlers
			sigaction (Signal.SIGBUS, IntPtr.Zero, sigbus);
			sigaction (Signal.SIGSEGV, IntPtr.Zero, sigsegv);

			// Enable crash reporting libraries
			customCrashReportingEnableCode ();

			// Restore Mono SIGSEGV and SIGBUS handlers            
			sigaction (Signal.SIGBUS, sigbus, IntPtr.Zero);
			sigaction (Signal.SIGSEGV, sigsegv, IntPtr.Zero);

			Marshal.FreeHGlobal (sigbus);
			Marshal.FreeHGlobal (sigsegv);
		}

		public static void ThrowExceptionAsNative(Exception exception)
		{
			ConvertToNsExceptionAndAbort (exception);
		}

		public static void ThrowExceptionAsNative(object exception)
		{
			ConvertToNsExceptionAndAbort (exception);
		}

        #if __UNIFIED__
		[DllImport(global::ObjCRuntime.Constants.FoundationLibrary, EntryPoint = "NSGetUncaughtExceptionHandler")]
        #else
        [DllImport(global::MonoTouch.Constants.FoundationLibrary, EntryPoint = "NSGetUncaughtExceptionHandler")]
        #endif
		static extern IntPtr NSGetUncaughtExceptionHandler();

		private delegate void ReporterDelegate(IntPtr ex);

//		static void ConvertToNsExceptionAndAbort(object e)
//		{
//			var nse = new NSException(".NET Exception", e.ToString(), null);
//			var uncaught = NSGetUncaughtExceptionHandler();
//			var dele = (ReporterDelegate)Marshal.GetDelegateForFunctionPointer(uncaught, typeof(ReporterDelegate));
//			dele(nse.Handle);
//		}

		static void ConvertToNsExceptionAndAbort(object e)
		{	
			var name = "Managed Xamarin.iOS .NET Exception";
			var msg = e.ToString();

			var ex = e as Exception;
			if(ex != null) 
				name = string.Format("{0}: {1}", ex.GetType().FullName, ex.Message);

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

