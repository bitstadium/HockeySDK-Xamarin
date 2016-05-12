﻿// public domain ... derived from https://github.com/bitstadium/HockeySDK-Android/blob/db7fff12beecea715f2894cb69ba358ea324ad17/src/main/java/net/hockeyapp/android/internal/ExceptionHandler.java
using System;
using System.Text.RegularExpressions;
using Android.OS;
using Environment = System.Environment;
using Process = System.Diagnostics.Process;

namespace HockeyApp
{
    internal static class TraceWriter
    {
        private static CrashManagerListener _Listener;

        private const string UNKNOWN_STATIC = "Unknown: call TraceWriter.InitializeConstants or TraceWriter.Initialize(listener) after CrashManager.Initialize";
        private static string _AppPackage = UNKNOWN_STATIC;
        private static string _AppVersion = UNKNOWN_STATIC;
        private static string _AndroidVersion = UNKNOWN_STATIC;
        private static string _PhoneManufacturer = UNKNOWN_STATIC;
        private static string _PhoneModel = UNKNOWN_STATIC;
        private static string _FilesPath = ".";
        private static readonly int _Version = (int)Build.VERSION.SdkInt;
        private static bool _IncludeDeviceData = true;
        private const string UNKNOWN_DYNAMIC = "Unknown: call TraceWriter.Initialize(listener) after CrashManager.Initialize";
        private static string _User = UNKNOWN_DYNAMIC;
        private static string _Contact = UNKNOWN_DYNAMIC;
        private static string _Description = UNKNOWN_DYNAMIC;
        public static bool AllowCachedDescription = false;
        public static string[] AppNamespaces = null;

        /// <summary>
        /// Copy build properties into c# land so that the handler won't crash accessing java.  When an unhandled exception occurs
        /// in a backgroud task or thread that is owned by the c# runtime, Xamarin.Android crashes if you try to invoke anything
        /// inside the java runtime.
        /// see https://bugzilla.xamarin.com/show_bug.cgi?id=10379 for more info
        /// </summary>
        public static void Initialize()
        {
            _AppPackage = Constants.AppPackage;
            _AppVersion = Constants.AppVersion;
            _AndroidVersion = Constants.AndroidVersion;
            _PhoneManufacturer = Constants.PhoneManufacturer;
            _PhoneModel = Constants.PhoneModel;
            _FilesPath = Constants.FilesPath;
        }

        private static string ClipString(string s)
        {
            if ((s != null) && (s.Length > 255))
                s = s.Substring(0, 255);
            return s;
        }
        /// <summary>
        /// Initialize the TraceWriter with a given CrashManagerListener
        /// </summary>
        /// <param name="listener"></param>
        /// <remarks>Thuis is important to use the UserID, Contact and Description from CrashManagerListener.</remarks>
        public static void Initialize(CrashManagerListener listener)
        {
            _Listener = listener;
            Initialize();
            if (listener != null)
            {
                _IncludeDeviceData = _Listener.IncludeDeviceData();
                _User = ClipString(listener.UserID) + "";
                _Contact = ClipString(listener.Contact);
                if (AllowCachedDescription)
                    _Description = listener.Description;
            }
        }
        public static void WriteTrace(object exception)
        {
            WriteTrace(exception, true);
        }
        /*
         from
           at System.IO.Directory.CreateDirectoriesInternal (System.String path) [0x00000] in <filename unknown>:0 
         to
         \tat android.support.v4.app.FragmentManagerImpl.checkStateLoss(FragmentManager.java:1343)
        */
        static Regex _StackTraceLine = new Regex(@"\s*at\s*(\S+)\s*\(([^\)]*)\)");

        /// <summary>
        /// Writes the given object (usually an exception) to disc so that it can be picked up by the CrashManager and send to Hockeyapp.
        /// </summary>
        /// <param name="exception">The object to write (usually an exception)</param>
        /// <param name="terminate">Flag that determines whether the process should be terminated after logging the exception</param>
        /// <remarks>This method controls exactly what is written to disc.  Its really a translation of the ExceptionHandler.saveException
        /// This method doesn't call any java if the exception object isn't a java class so it is safe to call when the mono runtime is no 
        /// longer able to invoke JNI methods, for example in the case of a crash in the pure c# managed space.</remarks>
        public static void WriteTrace(object exception, bool terminate)
        {
            if(exception is Java.Lang.Exception)
                ExceptionHandler.SaveException(exception as Java.Lang.Exception, Java.Lang.Thread.CurrentThread(), _Listener);
            else
                ExceptionHandler.SaveException(Java.Lang.Throwable.FromException(exception as Exception), Java.Lang.Thread.CurrentThread(), _Listener);

            if (terminate)
            {
                Process.GetCurrentProcess ().Kill ();
                Environment.Exit (10);
            }
        }

        /// <summary>
        /// Writes the given exception to disc using the standard Exceptionhandler.  Does not terminate the process, so it could be suitable
        /// for reporting non-fatal logs that upload on the next CrashManager execution
        /// </summary>
        /// <param name="exception">The exception to write.</param>
        /// <remarks>All features from CrashManagerListener are used (including informations like UserId, Contact and Description).</remarks>
        public static void WriteTraceAndInfo(Exception exception)
        {
            WriteTrace(exception, false);
        }

        public static string Logcat(string appTag)
        {
            string description = "";

            try
            {
                var p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "logcat";
                if (_Version >= 16)
                    p.StartInfo.Arguments = "-d";
                else
                    p.StartInfo.Arguments = "-d ActivityManager:I " + appTag + ":D *:S";
                p.Start();
                description = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldnt get logcat: {0}", e);
            }

            return description;
        }

    }
}