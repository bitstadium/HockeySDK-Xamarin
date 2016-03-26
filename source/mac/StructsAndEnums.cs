using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace HockeyApp
{
    // TODO:
    //[StructLayout (LayoutKind.Sequential)]
    //public struct CrashManagerCallbacks
    //{
    //    public unsafe void* context;

    //    public unsafe CrashManagerPostCrashSignalCallback* handleSignal;
    //}

    [Native]
    public enum CrashManagerUserInput : ulong
    {
        DontSend = 0,
        Send = 1,
        AlwaysSend = 2
    }

    [Native]
    public enum FeedbackUserDataElement : long
    {
        DontShow = 0,
        Optional = 1,
        Required = 2
    }

    [Native]
    public enum CrashErrorReason : long
    {
        ErrorUnknown,
        AppVersionRejected,
        ReceivedEmptyResponse,
        ErrorWithStatusCode
    }

    [Native]
    public enum FeedbackErrorReason : long
    {
        ErrorUnknown,
        ServerReturnedInvalidStatus,
        ServerReturnedInvalidData,
        ServerReturnedEmptyResponse,
        ClientAuthorizationMissingSecret,
        ClientCannotCreateConnection
    }

    [Native]
    public enum HockeyErrorReason : long
    {
        HockeyErrorUnknown
    }
}