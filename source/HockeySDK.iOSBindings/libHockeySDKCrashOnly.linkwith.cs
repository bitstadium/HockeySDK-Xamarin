using System;
using ObjCRuntime;

[assembly: LinkWith (
    "libHockeySDKCrashOnly.a", 
    LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64 | LinkTarget.Simulator | LinkTarget.Simulator64,
    IsCxx = true,
    SmartLink = true,
    Frameworks = "Foundation Security SystemConfiguration UIKit",
    LinkerFlags = "-ObjC -lc++")]
