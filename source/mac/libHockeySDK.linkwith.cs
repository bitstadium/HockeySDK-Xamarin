using ObjCRuntime;

[assembly: LinkWith ("libHockeySDK.a", 
    SmartLink = true, 
    ForceLoad = true)]
