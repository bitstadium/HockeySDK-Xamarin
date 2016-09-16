## Obtain an App Identifier

Please see the [How to create a new app](http://support.hockeyapp.net/kb/about-general-faq/how-to-create-a-new-app) tutorial. This will provide you with a HockeyApp specific App Identifier to be used to initialize the SDK.

## Add crash reporting

This will add crash reporting capabilities to your app.

In your `MainActivity.cs` file, add the following lines:

```csharp
using HockeyApp.Android;

namespace YourNameSpace
{
	[Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity 
	{
		protected override void OnCreate(Bundle savedInstanceState) 
		{
			base.OnCreate(savedInstanceState);

			// ... your own OnCreate implementation
			CrashManager.Register(this, "Your-App-Id");
		}
	}
}
```

When the app is resumed, the crash manager is triggered and checks if a new crash was created before. If a previous crash is detected, it presents a dialog to ask the user whether they want to send the crash log to HockeyApp. On app launch the crash manager registers a new exception handler to recognize app crashes.

## Add AppId to manifest

Add the following assembly level attribute in `Properties/AssemblyInfo.cs`

```csharp
[assembly: MetaData ("net.hockeyapp.android.appIdentifier", Value="Your-Api-Key")]
```

This will allow you to set your AppId once and simplify register calls

```csharp
using HockeyApp.Android;

namespace YourNameSpace
{
	[Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity 
	{
		protected override void OnCreate(Bundle savedInstanceState) 
		{
			base.OnCreate(savedInstanceState);

			// ... your own OnCreate implementation
			CrashManager.Register(this);
		}
	}
}
```

## Add User Metrics

HockeyApp automatically provides you with nice, intelligible, and informative metrics about how your app is used and by whom.

- **Sessions**: A new session is tracked by the SDK whenever the containing app is restarted (this refers to a 'cold start', i.e. when the app has not already been in memory prior to being launched) or whenever it becomes active again after having been in the background for 20 seconds or more.
- **Users**: The SDK anonymously tracks the users of your app by creating a random UUID.

On Android, User Metrics is not automatically gathered, you have to start this manually:

```csharp
// add the HockeyApp namespace
using HockeyApp.Android.Metrics;

// in your main activity OnCreate-method add:
MetricsManager.Register(this, Application, "$Your_App_Id");
```

## Add Custom Events

HockeyApp allows you to track custom events to understand user actions inside your app.

**Please note:** To use custom events, please first make sure that User Metrics is set up correctly, e.g. you registered the MetricsManager.

1. Make sure to add the correct namespace:
  ```csharp
  using HockeyApp;
  using System.Collections.Generic;
  ```

2. Track custom events like this:
  ```csharp
  HockeyApp.MetricsManager.TrackEvent("Custom Event");
  ```
  if you want to add custom properties or measurements, use this:

  ```csharp
  HockeyApp.MetricsManager.TrackEvent(
    "Custom Event",
    new Dictionary<string, string> { { "property", "value" } },
    new Dictionary<string, double> { { "time", 1.0 } }
  )
  ```

## Add Update Distribution

This will add the in-app update mechanism to your app. 

Open the activity where you want to inform the user about eventual updates. We'll assume you want to do this on startup of your main activity.

Add the following lines and make sure to always balance `Register(...)` calls to SDK managers with `Unregister()` calls in the corresponding lifecycle callbacks:

```csharp
using HockeyApp.Android;

namespace YourNameSpace 
{
	[Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
	public class YourActivity : Activity 
	{
		protected override void OnCreate(Bundle savedInstanceState) 
		{
			base.OnCreate(savedInstanceState);
	
			// Your own code to create the view
			// ...
   
			CheckForUpdates();
		}

		void CheckForUpdates()
		{
			// Remove this for store builds!
			UpdateManager.Register(this, "Your_App_Id");
		}

		void UnregisterManagers() 
		{
			UpdateManager.Unregister();
		}

		protected override void OnPause() 
		{
			base.OnPause();
			
			UnregisterManagers();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			UnregisterManagers();
		}
	}
}
```

When the activity is created, the update manager checks for new updates in the background. If it finds a new update, an alert dialog is shown and if the user presses Show, they will be taken to the update activity. The reason to only do this once upon creation is that the update check causes network traffic and therefore potential costs for your users.



## Add in-app feedback

This will add the ability for your users to provide feedback from right inside your app. 

You'll typically only want to show the feedback interface upon user interaction, for this example we assume you have a button `feedback_button` in your view for this.

Add the following lines to your respective activity, handling the touch events and showing the feedback interface:

```csharp
using HockeyApp.Android;

namespace YourNameSpace
{
	public class YourActivity : Activitiy
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
   			// Your own code to create the view
			// ...

			FeedbackManager.Register(this, "Your-App-Id");

			var feedbackButton = FindViewById<Button>(Resource.Id.feedback_button);

			feedbackButton.Click += delegate {
				FeedbackManager.ShowFeedbackActivity(ApplicationContext);
			});
		}
	}
}
```

When the user taps on the feedback button it will launch the feedback interface of the HockeySDK, where the user can create a new feedback discussion, add screenshots or other files for reference, and act on their previous feedback conversations.


## Add authentication

You can force authentication of your users through the `LoginManager` class. This will show a login screen to users if they are not fully authenticated to protect your app.

Retrieve your app secret from the HockeyApp backend. You can find this on the app details page in the backend right next to the ***App ID*** value. Click ***Show*** to access it.

Open the activity you want to protect, if you want to protect all of your app this will be your main activity.

Add the following lines to this activity:
using HockeyApp.Android;

```csharp
namespace YourNameSpace
{
	[Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
	public class YourActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Your own code to create the view
			// ...
			
			LoginManager.Register(this, APP_SECRET, 				LoginManager.LOGIN_MODE_EMAIL_PASSWORD);
				LoginManager.VerifyLogin(this, Intent);
		}
	}
}
```

Make sure to replace ***APP_SECRET*** with the value retrieved in the first step. This will launch the login activity every time a user launches your app.



### Permissions

Permissions get automatically merged into your manifest. If your app does not use update distribution you might consider removing the ***Write External Storage*** permission.


### Control output to LogCat

You can control the amount of log messages from HockeySDK that show up in LogCat. By default, we keep the noise as low as possible, only errors will show up. To enable additional logging, i.e. while debugging, add the following line of code:

```csharp
HockeyLog.LogLevel(3);
```

The different log levels match Android's own log levels.

```csharp
HockeyLog.LogLevel(2); // Verbose, show all log statements
HockeyLog.LogLevel(3); // Debug, show most log statements – useful for debugging
HockeyLog.LogLevel(4); // Info, show informative or higher log messages
HockeyLog.LogLevel(5); // Warn, show warnings and errors
HockeyLog.LogLevel(6); // Error, show only errors – the default log level
```

## More Information

For more information, see the [HockeySDK for Xamarin Source Repository](https://github.com/bitstadium/HockeySDK-Xamarin)
