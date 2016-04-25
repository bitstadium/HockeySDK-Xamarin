# HockeySDK for Xamarin

## Version 4.1.0-alpha3
- [changelog](https://github.com/bitstadium/HockeySDK-Xamarin/releases)

## Introduction
HockeySDK-Xamarin implements support for using HockeyApp in your iOS and Android applications.

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
 1. [Obtain an app identifier](#app-identifier)
 2. [Integrate HockeySDK](#integrate-sdk)
 3. [Add crash reporting](#crash-reporting)
 4. [Add user metrics](#user-metrics)
 5. [Add Update Distribution](#updated-distribution)
 6. [Add in-app feedback](#feedback)
 7. [Add authentication](#authentication)
3. [Advanced setup](#advanced-setup)
 1. [Permissions (Android-Only)](#permissions)
 2. [Control output to LogCat](#logcat-output)
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
This will add crash reporting capabilities to your app. Advanced ways to configure crash reporting are covered in [advanced setup](#advancedsetup).

#### For Android
1. Open your `MainActivity.cs` file.
2. Add the following lines:

```C#
using HockeyApp;

namespace YourNameSpace {

	[Activity(Label = "Your.App", MainLauncher = true, Icon = "@mipmap/icon")]
 public class MainActivity : Activity {
 
  protected override void OnCreate(Bundle bundle) {
   base.OnCreate();
   // ... your own OnCreate implementation
   checkForCrashes();
  }

  private void checkForCrashes() {
   CrashManager.register(this, "Your-App-Id");
  }
 }
}
```

#### For iOS
1. Open your `AppDelegate.cs` file.
2. Add the following lines:

```C#
using HockeySDK;

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

When the app is resumed, the crash manager is triggered and checks if a new crash was created before. If yes, it presents a dialog to ask the user whether they want to send the crash log to HockeyApp. On app launch the crash manager registers a new exception handler to recognize app crashes.

<a id="user-metrics"></a>
### 2.4 Add user metrics
*TBD*

<a id="updated-distribution"></a>
### 2.5 Add Update Distribution
*TBD*

<a id="feedback"></a>
### 2.6 Add in-app feedback
*TBD*

<a id="authentication"></a>
### 2.7 Add authentication
*TBD*

<a id="advanced-setup"></a>
## 3. Advanced setup
*TBD*

<a id="permissions"></a>
### 3.1 Permissions (Android-Only)
*TBD*

<a id="logcat-output"></a>
### 3.2 Control output to LogCat
*TBD*

<a id="documentation"></a>
## 4. Documentation
*TBD*

<a id="troubleshooting"></a>
## 5. Troubleshooting
*TBD*

<a id="contributing"></a>
## 6. Contributing
*TBD*

<a id="contributor-license"></a>
## 7. Contributor license
*TBD*

<a id="contact"></a>
## 8. Contact
*TBD*

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
