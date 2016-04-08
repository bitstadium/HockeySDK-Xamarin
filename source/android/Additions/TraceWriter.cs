// public domain ... derived from https://github.com/bitstadium/HockeySDK-Android/blob/db7fff12beecea715f2894cb69ba358ea324ad17/src/main/java/net/hockeyapp/android/internal/ExceptionHandler.java
using System;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
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
            //if the exception came from java, then we should be able to invoke it
            if (exception is Java.Lang.Exception)
            {
                ExceptionHandler.SaveException(exception as Java.Lang.Exception, _Listener);
            }
            else
            {
                var date = DateTime.Now;
                var filename = Guid.NewGuid().ToString();
                var path = Path.Combine(_FilesPath, filename + ".stacktrace");
                Console.WriteLine("Writing unhandled exception to: {0}", path);
                try
                {
                    using (var f = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (var sw = new StreamWriter(f))
                    {
                        // Write the stacktrace to disk
                        sw.WriteLine("Package: {0}", _AppPackage);
                        sw.WriteLine("Version: {0}", _AppVersion);
                        if (_IncludeDeviceData)
                        {
                            sw.WriteLine("Android: {0}", _AndroidVersion);
                            sw.WriteLine("Manufacturer: {0}", _PhoneManufacturer);
                            sw.WriteLine("Model: {0}", _PhoneModel);
                        }
                        sw.WriteLine("Date: {0}", date);
                        sw.WriteLine();
                        //make sure there is something actually on disk
                        sw.Flush();
                        try
                        {
                            if (!(exception is Exception))
                            {
                                sw.WriteLine(exception);
                            }
                            else
                            {
                                while ((exception as AggregateException) != null)
                                    exception = (exception as AggregateException).InnerException;

                                var e = (Exception)exception;
                                var trace = e.StackTrace;
                                if (trace == null)
                                {
                                    sw.WriteLine(e);
                                }
                                else
                                {
                                    sw.WriteLine("{0}: {1}", e.GetType().FullName, e.Message);
                                    foreach (Match m in _StackTraceLine.Matches(trace))
                                    {
                                        var method = m.Groups[1].Value;
                                        if (AppNamespaces != null)
                                        {
                                            //use an application provided list of classes to determine which namespaces
                                            //should be mapped into the package name so hockey app can pick out the correct
                                            //top stacktrace line.  It unfortunately means you end up with an extra copy of
                                            //package name at the beginning of your c# crash traces
                                            foreach (var prefix in AppNamespaces)
                                            {
                                                if (method.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    method = _AppPackage + "." + method;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //default to things that don't match mono/android/java, again, this forces an extra copy of
                                            //package name at the beginning of your c# crash traces
                                            if (!method.StartsWith("mono", StringComparison.OrdinalIgnoreCase) &&
                                                !method.StartsWith("android", StringComparison.OrdinalIgnoreCase) &&
                                                !method.StartsWith("system", StringComparison.OrdinalIgnoreCase) &&
                                                !method.StartsWith("java", StringComparison.OrdinalIgnoreCase))
                                            {
                                                method = _AppPackage + "." + method;
                                            }
                                        }
                                        //this forces the arguments part to look more like the file line number part that
                                        //android stack traces normally have
                                        var arguments = m.Groups[2].Value.Trim();
                                        arguments = arguments.Replace(' ', '_');
                                        arguments = arguments.Replace(',', '_');

                                        sw.WriteLine("\tat {0}({1}.args:1337)", method, arguments);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            // log something just in case something triggers the null refrence with the attached exception
                            // see https://bugzilla.xamarin.com/show_bug.cgi?id=10379
                            sw.WriteLine();
                            sw.WriteLine("Exception writing exception: {0}", e);
                            throw new Exception("Problem writing exception", e);
                        }
                        //make sure there is something actually on disk
                        sw.Flush();
                        if (_Listener != null)
                        {
                            try
                            {
                                File.WriteAllText(Path.Combine(_FilesPath, filename + ".user"), ClipString(_Listener.UserID), Encoding.UTF8);
                            }
                            catch (Exception)
                            {
                                if (_User != null)
                                {
                                    sw.WriteLine();
                                    sw.WriteLine("UserId was cached");
                                    File.WriteAllText(Path.Combine(_FilesPath, filename + ".user"), ClipString(_User),
                                        Encoding.UTF8);
                                }
                            }
                            try
                            {
                                File.WriteAllText(Path.Combine(_FilesPath, filename + ".contact"), ClipString(_Listener.Contact), Encoding.UTF8);
                            }
                            catch (Exception)
                            {
                                if (_Contact != null)
                                {
                                    sw.WriteLine();
                                    sw.WriteLine("Contact was cached");

                                    File.WriteAllText(Path.Combine(_FilesPath, filename + ".contact"), ClipString(_Contact), Encoding.UTF8);
                                }
                            }
                            try
                            {
                                File.WriteAllText(Path.Combine(_FilesPath, filename + ".description"), _Listener.Description, Encoding.UTF8);
                            }
                            catch (Exception)
                            {
                                if (AllowCachedDescription &&_Description != null)
                                {
                                    sw.WriteLine();
                                    sw.WriteLine("Description was cached");
                                    File.WriteAllText(Path.Combine(_FilesPath, filename + ".description"), _Description, Encoding.UTF8);
                                }
                            }
                        }
                    }
                }
                catch (Exception another)
                {
                    Console.WriteLine("Error saving exception stacktrace! {0}", another);
                }
            }
            
            if (terminate) {
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