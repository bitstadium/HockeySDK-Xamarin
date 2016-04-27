## Obtain an App Identifier

Please see the [How to create a new app](http://support.hockeyapp.net/kb/about-general-faq/how-to-create-a-new-app) tutorial. This will provide you with an HockeyApp specific App Identifier to be used to initialize the SDK.

## Add crash reporting

This will add crash reporting capabilities to your app. 

Open your AppDelegate.cs file, and add the following lines:

```csharp
using HockeySDK;

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
