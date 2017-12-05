using System;
using ObjCRuntime;

[assembly: LinkWith (
    "libHockeySDKFeedbackOnly.a", 
    LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64 | LinkTarget.Simulator | LinkTarget.Simulator64,
    IsCxx = false,
    SmartLink = true,
    Frameworks = "CoreGraphics CoreText CoreTelephony Foundation MobileCoreServices Photos QuartzCore QuickLook Security SystemConfiguration UIKit",
    LinkerFlags = "-ObjC")]
