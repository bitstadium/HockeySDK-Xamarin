## Adding HockeyApp to your iOS app

In your `AppDelegate.cs`'s your `FinishedLaunching` override should look something like this: (be sure to replace "YOUR-HOCKEYAPP-APPID" with your own):

```
public override bool FinishedLaunching (UIApplication app, NSDictionary options)
{
	//We MUST wrap our setup in this block to wire up
	// Mono's SIGSEGV and SIGBUS signals
	HockeyApp.Setup.EnableCustomCrashReporting (() => {

		//Get the shared instance
		var manager = BITHockeyManager.SharedHockeyManager;

		//Configure it to use our APP_ID
		manager.Configure ("YOUR-HOCKEYAPP-APPID");

		//Start the manager
		manager.StartManager ();

		//Authenticate (there are other authentication options)
		manager.Authenticator.AuthenticateInstallation ();
		
		//Rethrow any unhandled .NET exceptions as native iOS 
		// exceptions so the stack traces appear nicely in HockeyApp
		AppDomain.CurrentDomain.UnhandledException += (sender, e) => 
			Setup.ThrowExceptionAsNative(e.ExceptionObject);

		TaskScheduler.UnobservedTaskException += (sender, e) => 
			Setup.ThrowExceptionAsNative(e.Exception);
	});

	//The rest of your code here
	// ...
}
```

Note that you must wrap the code in your FinishedLaunching method in the `HockeyApp.Setup.EnableCustomCrashReporting` block so that we can temporarily redirect mono's SIGSEGV and SIGBUS handlers.  This is important for Crash Reporting.

Also note that you can wire up unhandled exception and unobserved task exception events and use the `Setup.ThrowExceptionAsNative(...)` to re-throw the exception as a native iOS exception.  This will ensure that you see a stack trace in your HockeyApp dashboard.

After your initial setup is complete, you can access the `BITHockeyManager.SharedHockeyManager` share instance everywhere else in your app.  For example, you can show existing feedback or show a form for submitting new feedback to the user:

```
BITHockeyManager.FeedbackManager.ShowFeedbackListView();

BITHockeyManager.FeedbackManager.ShowFeedbackComposeView();
```


## Targeting iOS 6.0
If you would like your app to target iOS 6.0 you will need to add the following arguments to your application project settings.

1. Open project Options
2. Under Build -> iOS Build
3. Go to the Additional Options -> Additional mtouch arguments and add:
   `-cxx -gcc_flags "-lc++"`
