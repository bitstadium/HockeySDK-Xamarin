# HockeySDK for Xamarin

Currently, the following platforms are supported:

 - Xamarin.iOS
 - Xamarin.iOS (Crash Only)
 - Xamarin.Android
 

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
