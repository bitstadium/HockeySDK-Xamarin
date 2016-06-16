# HockeySDK for Xamarin

## Version 4.1.0-beta2
- [changelog](https://github.com/bitstadium/HockeySDK-Xamarin/releases)

## Introduction
HockeySDK-Xamarin implements support for using HockeyApp in your iOS and Android applications.
Please refer to [HockeySDK-iOS](https://github.com/bitstadium/HockeySDK-iOS) and [HockeySDK-Android](https://github.com/bitstadium/HockeySDK-Android) for advanced platform-specific behaviours

The following features are currently supported:

1. **Collect crash reports:** If your app crashes, a crash log is written to the device's storage. If the user starts the app again, they will be asked asked to submit the crash report to HockeyApp. This works for both beta and live apps, i.e. those submitted to the App Store. Crash logs contain viable information for you to help resolve the issue. Furthermore, you as a developer can add additional information to the report as well.

2. **User Metrics:** Understand user behavior to improve your app. Track usage through daily and monthly active users. Monitor crash impacted users. Measure customer engagement through session count. This feature requires a minimum API level of 14 (Android 4.x Ice Cream Sandwich).

3. **Update Ad-Hoc / Enterprise apps:** The app will check with HockeyApp if a new version for your Ad-Hoc or Enterprise build is available. If yes, it will show an alert view to the user and let him see the release notes, the version history and start the installation process right away.

4. **Update notification for app store:** The app will check if a new version for your app store release is available. If yes, it will show an alert view to the user and let him open your app in the App Store app. (Disabled by default!)

5. **Feedback:** Besides crash reports, collecting feedback from your users from within your app is a great option to help with improving your app. You act on and answer feedback directly from the HockeyApp backend.

6. **Authenticate:** To help you stay in control of closed tester groups, you can identify and authenticate users against your registered testers with the HockeyApp backend. The authentication feature supports several ways of authentication.

This document contains the following sections:

1. [Requirements](#requirements)
2. [Setup](#setup)
 1. [Obtain an App Identifier](#app-identifier)
 2. [Integrate HockeySDK](#integrate-sdk)
 3. [Add crash reporting](#crash-reporting)
 4. [Add user metrics](#user-metrics)
 5. [Add Update Distribution](#updated-distribution)
 6. [Add in-app feedback](#feedback)
 7. [Add authentication](#authentication)
3. [Advanced setup](#advanced-setup)
 1. [Adding App ID to manifest (Android-Only)](#appid-manifest)
 2. [Permissions (Android-Only)](#permissions)
 3. [Control output to LogCat](#logcat-output)
4. [Documentation](#documentation)
5. [Troubleshooting](#troubleshooting)
6. [Contributing](#contributing)
7. [Contributor license](#contributor-license)
8. [Contact](#contact)

Currently, the following platforms are supported:

 - Xamarin.iOS
 - Xamarin.iOS (Crash Only)
 - Xamarin.Android

<a id="requirements"></a>
## 1. Requirements

1. We assume that you have a project in Xamarin Studio, or Xamarin for Visual Studio.
2. We assume you are not using other crash-analytic services on the same mobile application simultaneously.
3. *Important:* We assume, that your project builds using a shared library. PCLs are not supported yet. We are working on this.

<a id="setup"></a>
## 2. Setup

<a id="app-identifier"></a>
### 2.1 Obtain an App Identifier

Please see the "[How to create a new app](http://support.hockeyapp.net/kb/about-general-faq/how-to-create-a-new-app)" tutorial. This will provide you with an HockeyApp specific App Identifier to be used to initialize the SDK.

<a id="integrate-sdk"></a>
### 2.2 Integrate the SDK
For each iOS and Android project desired, add the HockeySDK-Xamarin nuget.

#### For Xamarin Studio
1. Navigate `Project->Add NuGet Packages...`
2. Search `HockeySDK.Xamarin` (enable show pre-release packages)

#### For Xamarin for Visual Studio
1. Navigate `Project->Manage NuGet Packages...`
2. Search `HockeySDK.Xamarin` (enable include prerelease)

<a id="crash-reporting"></a>
### 2.4 Add crash reporting
This will add crash reporting capabilities to your app. Advanced ways to configure crash reporting are covered in advanced setup: [iOS](https://github.com/bitstadium/HockeySDK-iOS#advancedsetup) | [Android](https://github.com/bitstadium/HockeySDK-Android#advancedsetup)

#### For iOS
1. Open your `AppDelegate.cs` file.
2. Add the following lines:

```C#
using HockeyApp;

namespace YourNameSpace {

 [Register("AppDelegate")]
 public partial class AppDelegate : UIApplicationDelegate {
 
  public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions) {
   var manager = BITHockeyManager.SharedHockeyManager;
   manager.Configure("Your_App_Id");
   manager.StartManager();
  }
 }
}
```

#### For Android
1. Open your `MainActivity.cs` file.
2. Add the following lines:

```C#
using HockeyApp;

namespace YourNameSpace {

 [Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
 public class MainActivity : Activity {
  protected override void OnCreate(Bundle savedInstanceState) {
   base.OnCreate(savedInstanceState);
   // ... your own OnCreate implementation
   CrashManager.Register(this, "Your-App-Id");
  }
 }
}
```

When the app is resumed, the crash manager is triggered and checks if a new crash was created before. If yes, it presents a dialog to ask the user whether they want to send the crash log to HockeyApp. On app launch the crash manager registers a new exception handler to recognize app crashes.

<a id="user-metrics"></a>
### 2.4 Add user metrics
HockeyApp automatically provides you with nice, intelligible, and informative metrics about how your app is used and by whom.
- **Sessions**: A new session is tracked by the SDK whenever the containing app is restarted (this refers to a 'cold start', i.e. when the app has not already been in memory prior to being launched) or whenever it becomes active again after having been in the background for 20 seconds or more.
- **Users**: The SDK anonymously tracks the users of your app by creating a random UUID that is then securely stored in the iOS keychain. Because this anonymous ID is stored in the keychain it persists across reinstallations.

Just in case you want to opt-out of this feature, there is a way to turn this functionality off:


#### For iOS
```C#
using HockeyApp;

var manager = BITHockeyManager.SharedHockeyManager;
manager.Configure("Your_App_Id");
manager.DisableMetricsManager = true;
manager.StartManager();
```

Android does not automatically start the MetricsManager:

#### For Android
```C#
using HockeyApp.Metrics;

MetricsManager.Register(this, Application, "Your-App-Id");
```

<a id="updated-distribution"></a>
### 2.5 Add Update Distribution
This will add the in-app update mechanism to your app. Detailed configuration options are in advanced setup: [iOS](https://github.com/bitstadium/HockeySDK-iOS#advancedsetup) | [Android](https://github.com/bitstadium/HockeySDK-Android#advancedsetup)

#### For iOS
The feature handles version updates, presents update and version information in a App Store like user interface, collects usage information and provides additional authorization options when using Ad-Hoc provisioning profiles.

To enable automatic in-app updates you need to make sure to add `manager.Authenticator.AuthenticateInstallation();` after starting the SDK:

```C#
using HockeyApp;

var manager = BITHockeyManagerSharedHockeyManager;
manager.Configure("Your_App_Id");
manager.StartManager();
manager.Authenticator.AuthenticateInstallation();
```

This module automatically disables itself when running in an App Store build by default!

This feature can be disabled manually as follows:

```C#
using HockeyApp;

var manager = BITHockeyManagerSharedHockeyManager;
manager.Configure("Your_App_Id");
manager.SetDisableUpdateManager = true;
manager.StartManager();
```

If you want to see beta analytics, use the beta distribution feature with in-app updates, restrict versions to specific users, or want to know who is actually testing your app, you need to follow the instructions on our guide [Authenticating Users on iOS](https://support.hockeyapp.net/kb/client-integration-ios-mac-os-x-tvos/authenticating-users-on-ios)

#### For Android
1. Open the activity where you want to inform the user about eventual updates. We'll assume you want to do this on startup of your main activity.
2. Add the following lines and make sure to always balance `register(...)` calls to SDK managers with `unregister()` calls in the corresponding lifecycle callbacks:

```C#
using HockeyApp;

namespace YourNameSpace {
 [Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
 public class YourActivity : Activity {
  protected override void OnCreate(Bundle savedInstanceState) {
   base.OnCreate(savedInstanceState);
   // Your own code to create the view
   // ...
    
   CheckForUpdates();
  }

  private void CheckForUpdates() {
   // Remove this for store builds!
   UpdateManager.Register(this, "Your_App_Id");
  }
  
  private void UnregisterManagers() {
   UpdateManager.Unregister();
  }

  protected override void OnPause() {
   base.OnPause();
   UnregisterManagers();
  }
  
  protected override void OnDestroy() {
   base.OnDestroy();
   UnregisterManagers();
  }
 }
}
```

When the activity is created, the update manager checks for new updates in the background. If it finds a new update, an alert dialog is shown and if the user presses Show, they will be taken to the update activity. The reason to only do this once upon creation is that the update check causes network traffic and therefore potential costs for your users.

<a id="feedback"></a>
### 2.6 Add in-app feedback
The feedback manager lets your users communicate directly with you via the app and an integrated user interface. It provides a single threaded discussion with a user running your app. Detailed configuration options are in advanced setup: [iOS](https://github.com/bitstadium/HockeySDK-iOS#advancedsetup) | [Android](https://github.com/bitstadium/HockeySDK-Android#advancedsetup)

1. You'll typically only want to show the feedback interface upon user interaction, for this example we assume you have a button `feedbackButton` in your view for this.
2. Add the following lines to your respective view controller/activity, handling the touch events and showing the feedback interface:

#### For iOS

You should never create your own instance of BITFeedbackManager but use the one provided by the `BITHockeyManager.sharedHockeyManager()`.

```C#
using HockeyApp;

namespace YourNameSpace {
{

	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions) {
		{
			// Initialise the Hockey SDK here
   			var manager = BITHockeyManagerSharedHockeyManager;
			manager.Configure("Your_App_Id");
			manager.StartManager();
   			
			// Create button and add action for click event
			var app = new App ();
   			var ShowFeedbackListViewButton = new Xamarin.Forms.Button {
				Text = "Show Feedback List View"
			};
			ShowFeedbackListViewButton.Clicked += ShowFeedbackList;
			app.AddChild (ShowFeedbackListViewButton);
   		}
   		
   		private static void ShowFeedbackList(object sender, EventArgs e) {
   			// This is where the feedback form gets displayed
			var feedbackManager = BITHockeyManager.SharedHockeyManager.FeedbackManager;
			feedbackManager.ShowFeedbackListView ();
		}
	}
}	
```

Please check the [documentation](#documentation) of the `BITFeedbackManager` class on more information on how to leverage this feature.

#### For Android

1. You'll typically only want to show the feedback interface upon user interaction, for this example we assume you have a button `feedback_button` in your view for this.
2. Add the following lines to your respective activity, handling the touch events and showing the feedback interface:

```C#
using HockeyApp;

namespace YourNameSpace {
 public class YourActivity : Activitiy {
  protected override void OnCreate(Bundle savedInstanceState) {
   base.OnCreate(savedInstanceState);
   // Your own code to create the view
   // ...

   FeedbackManager.Register(this, "Your-App-Id");

   Button feedbackButton = FindViewById<Button>(Resource.Id.feedback_button);
   feedbackButton.Click += delegate {
    FeedbackManager.ShowFeedbackActivity(ApplicationContext);
   });
  }
}
```

When the user taps on the feedback button it will launch the feedback interface of the HockeySDK, where the user can create a new feedback discussion, add screenshots or other files for reference, and act on their previous feedback conversations.

<a id="authentication"></a>
### 2.7 Add authentication
#### For iOS
Instructions for iOS Authentication can be forund [here](https://support.hockeyapp.net/kb/client-integration-ios-mac-os-x-tvos/authenticating-users-on-ios)

#### For Android
You can force authentication of your users through the `LoginManager` class. This will show a login screen to users if they are not fully authenticated to protect your app.

1. Retrieve your app secret from the HockeyApp backend. You can find this on the app details page in the backend right next to the "App ID" value. Click "Show" to access it. 
2. Open the activity you want to protect, if you want to protect all of your app this will be your main activity.
3. Add the following lines to this activity:

```C#
using HockeyApp;

namespace YourNameSpace {
 [Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
 public class YourActivity : Activity {
  protected override void OnCreate(Bundle savedInstanceState) {
   base.OnCreate(savedInstanceState);
   // Your own code to create the view
   // ...

   LoginManager.Register(this, APP_SECRET, LoginManager.LOGIN_MODE_EMAIL_PASSWORD);
   LoginManager.VerifyLogin(this, Intent);
  }
 }
}
```

Make sure to replace `APP_SECRET` with the value retrieved in step 1. This will launch the login activity every time a user launches your app.

<a id="advanced-setup"></a>
## 3. Advanced setup

<a id="appid-manifest"></a>
### 3.1 Adding App ID to Manifest (Android-Only)

Add the following assembly level attribute in `Properties/AssemblyInfo.cs`

```csharp
[assembly: MetaData ("net.hockeyapp.android.appIdentifier", Value="Your-Api-Key")]
```

This will allow you to set your App ID once and simplify register calls

```csharp
using HockeyApp;

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

<a id="permissions"></a>
### 3.2 Permissions (Android-Only)
Permissions get automatically merged into your manifest. If your app does not use update distribution you might consider removing the permission `WRITE_EXTERNAL_STORAGE` - see the [advanced permissions section](https://github.com/bitstadium/HockeySDK-Android#permissions-advanced) for details.

<a id="logcat-output"></a>
### 3.3 Control output to LogCat
You can control the amount of log messages from HockeySDK that show up in LogCat. By default, we keep the noise as low as possible, only errors will show up. To enable additional logging, i.e. while debugging, add the following line of code:

#### For iOS
```C#
var manager = BITHockeyManager.SharedHockeyManager;
manager.Configure("Your_App_Id");
manager.SetDebugLogEnabled = true;
manager.StartManager();
```

#### For Android
```C#
using HockeyApp.Util;

HockeyLog.LogLevel = 3;
```

The different log levels match Android's own log levels.

```C#
HockeyLog.LogLevel = 2; // Verbose, show all log statements
HockeyLog.LogLevel = 3; // Debug, show most log statements – useful for debugging
HockeyLog.LogLevel = 4; // Info, show informative or higher log messages
HockeyLog.LogLevel = 5; // Warn, show warnings and errors
HockeyLog.LogLevel = 6; // Error, show only errors – the default log level
```

<a id="documentation"></a>
## 4. Documentation
Our documentation can be found on HockeyApp [iOS](http://hockeyapp.net/help/sdk/ios/4.1.0-beta.2/index.html) | [Android](http://hockeyapp.net/help/sdk/android/4.1.0-beta.2/index.html)

<a id="troubleshooting"></a>
## 5. Troubleshooting
1. Check if the Your_App_Id matches the App ID in HockeyApp.
2. Check if the `Package name` in `Project Options->Android Application` file matches the Bundle Identifier of the app in HockeyApp. HockeyApp accepts crashes only if both the App ID and the bundle identifier match their corresponding values in your app. Please note that the package value in your `AndroidManifest.xml` file might differ from the bundle identifier.
3. If your app crashes and you start it again, does the dialog show up which asks the user to send the crash report? If not, please [enable logging](#logcat-output)
4. If it still does not work, please [contact us](http://support.hockeyapp.net/discussion/new).

<a id="contributing"></a>
## 6. Contributing
We're looking forward to your contributions via pull requests.

<a id="contributor-license"></a>
## 7. Contributor license
You must sign a [Contributor License Agreement](https://cla.microsoft.com/) before submitting your pull request. To complete the Contributor License Agreement (CLA), you will need to submit a request via the [form](https://cla.microsoft.com/) and then electronically sign the CLA when you receive the email containing the link to the document. You need to sign the CLA only once to cover submission to any Microsoft OSS project. 

<a id="contact"></a>
## 8. Contact

If you have further questions or are running into trouble that cannot be resolved by any of the steps here, feel free to open a GitHub issue here or contact us at [support@hockeyapp.net](mailto:support@hockeyapp.net) or in our [public Slack channel](https://slack.hockeyapp.net).

## Building from Source

Build Prerequisites:

 - Mac OSX 10.11
 - Xamarin.Android
 - Xamarin.iOS
 - XCode 7.2+
 
The file `build.cake` is the main build script used to compile the SDK source.  This script is running on the [Cake](http://cakebuild.net) build system.  A `bootstrapper.sh` file is provided to execute the build without installing cake explicitly.

You can build the source including all samples, nuget packages and components by executing the following command:

```
sh ./bootstrapper.sh -t all
```

You can alternatively execute the targets `libs`, `samples`, `nuget`, or `components` instead of `all`.

## Components

The build script produces 3 separate components that are currently published on the [Xamarin Component Store](http://components.xamarin.com):

 - HockeyApp for iOS
 - HockeyApp for iOS (Crash Only)
 - HockeyApp for Android
 
## NuGet

The build script produces a single NuGet package which contains binaries for and is installable on all the supported platforms. 

## License

Please see the `License.md` file for details.
