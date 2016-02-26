## Adding HockeyApp to your Android app
Open your `AndroidManifest.xml` and add the following line as a child element of the tag `application`:

```
<activity android:name="net.hockeyapp.android.UpdateActivity" />
```

Add the following permissions to your app.  You can add them in your project settings, or by adding the xml directly to your AndroidManifest.xml or by adding the following attributes somewhere in your app:

```
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
```

HockeyApp was designed to catch unhandled/uncaught ***java*** exceptions, and doesn't really know much about Xamarin.Android.  So, if you want HockeyApp to catch your Managed .NET exceptions, you need to help it do so.  A good way to do this is to globally handle the .NET `UnhandledException` and `UnobservedTaskException` events.  The event handlers for these events should call the `HockeyApp.ManagedExceptionHandler.SaveException (..)` helper method.  Here is an example:

```
public class MainActivity : Activity
{      
    public const string HOCKEYAPP_APPID = "YOUR-APP-ID";

	protected override void OnCreate (Bundle bundle)
	{
		base.OnCreate (bundle);

        // Register the crash manager before Initializing the trace writer
        HockeyApp.CrashManager.Register (this, HOCKEYAPP_APPID); 

        //Register to with the Update Manager
        HockeyApp.UpdateManager.Register (this, HOCKEYAPP_APPID);

        // Initialize the Trace Writer
        HockeyApp.TraceWriter.Initialize ();

        // Wire up Unhandled Expcetion handler from Android
        AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) => 
        {
            // Use the trace writer to log exceptions so HockeyApp finds them
            HockeyApp.TraceWriter.WriteTrace(args.Exception);
            args.Handled = true;
        };

        // Wire up the .NET Unhandled Exception handler
        AppDomain.CurrentDomain.UnhandledException +=
            (sender, args) => HockeyApp.TraceWriter.WriteTrace(args.ExceptionObject);

        // Wire up the unobserved task exception handler
        TaskScheduler.UnobservedTaskException += 
            (sender, args) => HockeyApp.TraceWriter.WriteTrace(args.Exception);


        // ...
		
	}
}
```