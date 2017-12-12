## Version 5.1.0

HockeySDK-Xamarin implements support for HockeyApp in your iOS and Android applications.

The following features are currently supported:

1. **Collect crash reports:** If your app crashes, a crash log is written to the device's storage. If the user starts the app again, they will be asked to submit the crash report to HockeyApp. This works for both beta and live apps, i.e. those submitted to the App Store. Crash logs contain viable information for you to help resolve the issue. Furthermore, you as a developer can add additional information to the report as well.

2. **User Metrics:** Understand user behavior to improve your app. Track usage through daily and monthly active users. Monitor crash impacted users. Measure customer engagement through session count. You can also track custom events and view the aggregate results on the HockeyApp dashboard.

3. **Update Ad-Hoc / Enterprise apps:** The app will check with HockeyApp if a new version for your Ad-Hoc or Enterprise build is available. If yes, it will show an alert view to the user and let him see the release notes, the version history and start the installation process right away.

4. **Update notification for app store:** The app will check if a new version for your app store release is available. If yes, it will show an alert view to the user and let him open your app in the App Store app. (Disabled by default!)

5. **Feedback:** Besides crash reports, collecting feedback from your users from within your app is a great option to help with improving your app. You act on and answer feedback directly from the HockeyApp backend.

6. **Authenticate:** To help you stay in control of closed tester groups, you can identify and authenticate users against your registered testers with the HockeyApp backend. The authentication feature supports several ways of authentication.

## 1. Setup

It is super easy to use HockeyApp in your Xamarin app. Have a look at our [documentation](https://support.hockeyapp.net/kb/client-integration-cross-platform/how-to-integrate-hockeyapp-with-xamarin) and onboard your app within minutes.

## 2. Documentation

Please visit [our landing page](https://support.hockeyapp.net/kb) as a starting point for all of our documentation and check out our [getting started documentation](https://support.hockeyapp.net/kb/client-integration-cross-platform/how-to-integrate-hockeyapp-with-xamarin), [changelog](https://github.com/bitstadium/HockeySDK-Xamarin/releases), and our [troubleshooting section](https://support.hockeyapp.net/kb/client-integration-cross-platform/how-to-integrate-hockeyapp-with-xamarin#5-troubleshooting).

The Xamarin SDK wraps our native SDKs – [HockeySDK-iOS 5.1.0](https://github.com/bitstadium/HockeySDK-iOS/releases/tag/5.1.0) and [HockeySDK-Android 5.0.4](https://github.com/bitstadium/HockeySDK-Android/releases/tag/5.0.4). For more info on advanced, platform-specific behaviors, check out the documentation for [HockeySDK-iOS](https://support.hockeyapp.net/kb/client-integration-ios-mac-os-x-tvos/hockeyapp-for-ios) and [HockeySDK-Android](https://support.hockeyapp.net/kb/client-integration-android/hockeyapp-for-android-sdk).

## 3. Contributing

We're looking forward to your contributions via [pull requests on GitHub](https://github.com/bitstadium/HockeySDK-Xamarin).

### 3.1 Development environment

* A Mac, running the latest version of macOS.
* Get the latest Xcode from the Mac App Store.

### 3.2 Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

### 3.3 Contributor License

You must sign a [Contributor License Agreement](https://cla.microsoft.com/) before submitting your pull request. To complete the Contributor License Agreement (CLA), you will need to submit a request via the [form](https://cla.microsoft.com/) and then electronically sign the CLA when you receive the email containing the link to the document. You need to sign the CLA only once to cover submission to any Microsoft OSS project. 

## 4. Contact

If you have further questions or are running into trouble that cannot be resolved by any of the steps [in our troubleshooting section](https://support.hockeyapp.net/kb/client-integration-cross-platform/how-to-integrate-hockeyapp-with-xamarin#5-troubleshooting), feel free to open an issue here, contact us at [support@hockeyapp.net](mailto:support@hockeyapp.net), or join our [Slack](https://slack.hockeyapp.net).