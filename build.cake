#tool nuget:?package=XamarinComponent
#addin nuget:?package=Cake.Xamarin
#addin nuget:?package=Cake.FileHelpers

var COMPONENT_VERSION = "4.1.0";
var NUGET_VERSION = "4.1.0-beta1";

var ANDROID_URL = "https://s3.amazonaws.com/hockey-app-download/sdk/xamarin/android/HockeySDK-Android-4.1.0-beta.1.zip";
var IOS_URL = "https://s3.amazonaws.com/hockey-app-download/sdk/xamarin/ios/HockeySDK-iOS-4.1.0-beta.1.zip";
var SAMPLES = new [] {
	"./samples/HockeyAppSampleAndroid.sln",
	// "./samples/HockeyAppSampleiOS.sln",
	// "./samples/HockeyAppSampleiOSCrashOnly.sln",
};

var TARGET = Argument ("target", Argument ("t", "all"));

Task ("libs")
	.IsDependentOn ("externals")
	.Does (() => 
{
	NuGetRestore ("./HockeySDK.sln");
	DotNetBuild ("./HockeySDK.sln", c => c.Configuration = "Release");
});

Task ("samples")
	.IsDependentOn ("libs")
	.Does (() => 
{
	foreach (var s in SAMPLES) {
		NuGetRestore (s);
		DotNetBuild (s, c => c.Configuration = "Release");
	}
});

Task ("externals-android")
	.WithCriteria (!FileExists ("./externals/android/hockeyapp.android.zip"))
	.Does (() => 
{
	if (!DirectoryExists ("./externals/android"))
		CreateDirectory ("./externals/android");

	DownloadFile (ANDROID_URL, "./externals/android/hockeyapp.android.zip");
	Unzip ("./externals/android/hockeyapp.android.zip", "./externals/android/");

	var aar = GetFiles ("./externals/android/**/*.aar").FirstOrDefault ();
	CopyFile (aar, "./externals/android/HockeySDK.aar");
});

Task ("externals-ios")
	.WithCriteria (!FileExists ("./externals/ios/hockeyapp.ios.zip"))
	.Does (() => 
{
	if (!DirectoryExists ("./externals/ios"))
		CreateDirectory ("./externals/ios");

	DownloadFile (IOS_URL, "./externals/ios/hockeyapp.ios.zip");
	Unzip ("./externals/ios/hockeyapp.ios.zip", "./externals/ios/");


	CopyFile ("./externals/ios/HockeySDK-iOS/HockeySDK.embeddedframework/HockeySDK.framework/Versions/A/HockeySDK", "./externals/ios/libHockeySDK.a");
	CopyFile ("./externals/ios/HockeySDK-iOS/HockeySDKCrashOnly/HockeySDK.framework/Versions/A/HockeySDK", "./externals/ios/libHockeySDKCrashOnly.a");
	
	CopyDirectory ("./externals/ios/HockeySDK-iOS/HockeySDK.embeddedframework/HockeySDK.framework/Versions/A/Resources/HockeySDKResources.bundle", "./externals/ios/HockeySDKResources.bundle");
});

// Create a common externals task depending on platform specific ones
Task ("externals").IsDependentOn ("externals-ios").IsDependentOn ("externals-android");


Task ("nuget")
	.IsDependentOn ("libs")
	.Does (() => 
{
	// NuGet on mac trims out the first ./ so adding it twice works around
	var basePath = IsRunningOnUnix () ? (System.IO.Directory.GetCurrentDirectory().ToString() + @"/.") : "./";

	NuGetPack ("./HockeySDK.nuspec", new NuGetPackSettings {
		Version = NUGET_VERSION,
		BasePath = basePath,
		Verbosity = NuGetVerbosity.Detailed
	});

	if (!DirectoryExists ("./output"))
		CreateDirectory ("./output");

	CopyFiles ("./**/HockeySDK*.nupkg", "./output");
});

Task ("components")
	.IsDependentOn ("samples")
	.Does (() =>
{
	var yamls = GetFiles ("./**/component.template.yaml");

	foreach (var yaml in yamls) {
		var contents = FileReadText (yaml).Replace ("$version$", COMPONENT_VERSION);
		
		var fixedFile = yaml.GetDirectory ().CombineWithFilePath ("component.yaml");
		FileWriteText (fixedFile, contents);
		
		PackageComponent (fixedFile.GetDirectory (), new XamarinComponentSettings ());
	}

	if (!DirectoryExists ("./output"))
		CreateDirectory ("./output");

	CopyFiles ("./**/*.xam", "./output");
});

Task ("all").IsDependentOn ("nuget").IsDependentOn ("components");

Task ("clean").Does (() =>
{
	if (DirectoryExists ("./externals"))
		DeleteDirectory ("./externals", true);

	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});

RunTarget (TARGET);

