#tool nuget:?package=XamarinComponent
#addin nuget:?package=Cake.Xamarin
#addin nuget:?package=Cake.FileHelpers

var COMPONENT_VERSION = "5.2.0.0";
var NUGET_VERSION = "5.2.0";

var ANDROID_URL = "https://download.hockeyapp.net/sdk/android/HockeySDK-Android-5.2.0.zip";
var IOS_URL = "https://download.hockeyapp.net/sdk/ios/HockeySDK-iOS-5.1.2.zip";

var SAMPLES = new [] {
	"./samples/HockeyAppSampleAndroid.sln",
	"./samples/HockeyAppSampleiOS.sln",
	"./samples/HockeyAppSampleiOSCrashOnly.sln",
	"./samples/HockeyAppSampleForms.sln",
};

var TARGET = Argument ("target", Argument ("t", "all"));

Task ("libs")
	.IsDependentOn ("externals")
	.Does (() => 
{
	NuGetRestore ("./HockeySDK.sln");
	MSBuild ("./HockeySDK.sln", c => c.Configuration = "Release");
});

Task ("samples")
	.IsDependentOn ("libs")
	.Does (() => 
{
	foreach (var s in SAMPLES) {
		NuGetRestore (s);
		MSBuild (s, c => c.Configuration = "Release");
	}
});

Task("bindings-android").IsDependentOn("externals-android");
Task("bindings-ios").IsDependentOn("externals-ios");

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

	var docs = GetDirectories ("./externals/android/HockeySDK-Android-*/docs").FirstOrDefault ();
	CopyDirectory (docs, "./externals/android/docs");
});

Task ("externals-ios")
	.ReportError(exception =>
{  
    Console.WriteLine(exception.ToString());

    if(!(exception is Cake.Core.CakeException)) throw exception;
})
	.WithCriteria (!FileExists ("./externals/ios/hockeyapp.ios.zip"))
	.Does (() => 
{
	if (!DirectoryExists ("./externals/ios"))
		CreateDirectory ("./externals/ios");

	DownloadFile (IOS_URL, "./externals/ios/hockeyapp.ios.zip");
	Unzip ("./externals/ios/hockeyapp.ios.zip", "./externals/ios/");

	CopyFile ("./externals/ios/HockeySDK-iOS/HockeySDKAllFeatures/HockeySDK.embeddedframework/HockeySDK.framework/HockeySDK", "./externals/ios/libHockeySDK.a");
	CopyFile ("./externals/ios/HockeySDK-iOS/HockeySDKCrashOnly/HockeySDK.framework/HockeySDK", "./externals/ios/libHockeySDKCrashOnly.a");
	CopyFile ("./externals/ios/HockeySDK-iOS/HockeySDKFeedbackOnly/HockeySDK.embeddedframework/HockeySDK.framework/HockeySDK", "./externals/ios/libHockeySDKFeedbackOnly.a");

	CopyDirectory ("./externals/ios/HockeySDK-iOS/HockeySDKAllFeatures/HockeySDK.embeddedframework/HockeySDKResources.bundle", "./externals/ios/HockeySDKResources.bundle");
});

// Create a common externals task depending on platform specific ones
Task ("externals").IsDependentOn ("externals-ios").IsDependentOn ("externals-android");

// Create default nuget with all features.
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

	NuGetPack("./HockeySDKFeedbackOnly.nuspec", new NuGetPackSettings {
		Version = NUGET_VERSION,
		BasePath = basePath,
		Verbosity = NuGetVerbosity.Detailed
	});

	if (!DirectoryExists ("./output"))
		CreateDirectory ("./output");

	CopyFiles ("./HockeySDK*.nupkg", "./output");
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

	CopyFiles ("./component/**/*.xam", "./output");
});

Task ("all").IsDependentOn ("nuget").IsDependentOn ("components");

Task ("clean").Does (() =>
{
	if (DirectoryExists ("./externals"))
		CleanDirectory ("./externals");

	if (DirectoryExists ("./output"))
		CleanDirectory ("./output");

	DeleteFiles("./*.nupkg");
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});

RunTarget (TARGET);

