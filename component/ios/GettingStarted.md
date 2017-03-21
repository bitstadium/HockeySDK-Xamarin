## Obtain an App Identifier

Please see the [How to create a new app](http://support.hockeyapp.net/kb/about-general-faq/how-to-create-a-new-app) tutorial. This will provide you with a HockeyApp specific App Identifier to be used to initialize the SDK.

## Add crash reporting

This will add crash reporting capabilities to your app. 

Open your AppDelegate.cs file, and add the following lines:

```csharp
using HockeyApp.iOS;

namespace YourNameSpace
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("Your_App_Id");
			manager.StartManager();
		}
	}
}
```

## User Metrics

HockeyApp automatically provides you with nice, intelligible, and informative metrics about how your app is used and by whom.

- **Sessions**: A new session is tracked by the SDK whenever the containing app is restarted (this refers to a 'cold start', i.e. when the app has not already been in memory prior to being launched) or whenever it becomes active again after having been in the background for 20 seconds or more.
- **Users**: The SDK anonymously tracks the users of your app by creating a random UUID.

On iOS the random UUID securely stored in the keychain, so that it persist across reinstallations. On iOS, User Metrics is enabled by default. If you want to turn off User Metrics, follow this code:

```csharp
// add the HockeyApp namespace
using HockeyApp.iOS;

// in your FinishedLaunching-method add:
var manager = BITHockeyManager.SharedHockeyManager;
manager.Configure("$Your_App_Id");
manager.DisableMetricsManager = true;
manager.StartManager();
```

It is important that you set `DisableMetricsManager` before you start the manager.

## Custom Events

HockeyApp allows you to track custom events to understand user actions inside your app.

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

The feature handles version updates, presents update and version information in an App Store like user interface, collects usage information and provides additional authorization options when using Ad-Hoc provisioning profiles.

This module automatically disables itself when running in an App Store build by default!

This feature can be disabled manually as follows:

```csharp
var manager = BITHockeyManagerSharedHockeyManager;
manager.Configure("Your_App_Id");
manager.SetDisableUpdateManager = true;
manager.StartManager();
manager.Authenticator.AuthenticateInstallation(); 
```

If you want to see beta analytics, use the beta distribution feature with in-app updates, restrict versions to specific users, or want to know who is actually testing your app, you need to follow the instructions on our guide [Authenticating Users on iOS](https://support.hockeyapp.net/kb/client-integration-ios-mac-os-x-tvos/authenticating-users-on-ios)




## Add in-app feedback

This will add the ability for your users to provide feedback from right inside your app.

```csharp
var feedbackManager = BITHockeyManager.SharedHockeyManager.FeedbackManager;

// Show current feedback
feedbackManager.ShowFeedbackListView();

// Send new feedback
feedbackManager.ShowFeedbackComposeView();
```



## Add authentication

Instructions for iOS Authentication can be found [here](https://support.hockeyapp.net/kb/client-integration-ios-mac-os-x-tvos/authenticating-users-on-ios)



## Control logging output

You can control the amount of log messages from HockeySDK.  By default, we keep the noise as low as possible, only errors will show up. To enable additional logging, i.e. while debugging, add the following line of code:

```csharp
var manager = BITHockeyManager.SharedHockeyManager;
manager.Configure("Your_App_Id");
manager.SetDebugLogEnabled = true;
manager.StartManager();
```


## More Information

For more information, see the [HockeySDK for Xamarin Source Repository](https://github.com/bitstadium/HockeySDK-Xamarin)
