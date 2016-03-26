using System;
using AppKit;
using Foundation;
using ObjCRuntime;

namespace HockeyApp
{
    // @interface BITHockeyManager : NSObject
    [BaseType (typeof (NSObject), Name="BITHockeyManager")]
    interface HockeyManager
    {
        // +(BITHockeyManager *)sharedHockeyManager;
        [Static]
        [Export ("sharedHockeyManager")]
        HockeyManager SharedHockeyManager { get; }

        // -(void)configureWithIdentifier:(NSString *)appIdentifier;
        [Export ("configureWithIdentifier:")]
        void Configure (string appIdentifier);

        // -(void)configureWithIdentifier:(NSString *)appIdentifier delegate:(id<BITHockeyManagerDelegate>)delegate;
        [Export ("configureWithIdentifier:delegate:")]
        void Configure (string appIdentifier, IHockeyManagerDelegate @delegate);

        // -(void)configureWithIdentifier:(NSString *)appIdentifier companyName:(NSString *)companyName delegate:(id<BITHockeyManagerDelegate>)delegate __attribute__((deprecated("Use configureWithIdentifier:delegate: instead")));
        [Export ("configureWithIdentifier:companyName:delegate:")]
        void Configure (string appIdentifier, string companyName, IHockeyManagerDelegate @delegate);

        // -(void)startManager;
        [Export ("startManager")]
        void StartManager ();

        //[Wrap ("WeakDelegate")]
        [NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
        IHockeyManagerDelegate Delegate { get; set; }

        // @property (nonatomic, unsafe_unretained) id<BITHockeyManagerDelegate> delegate;
        //[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
        //NSObject WeakDelegate { get; set; }

        // @property (nonatomic, strong) NSString * serverURL;
        [Export ("serverURL", ArgumentSemantic.Strong)]
        string ServerUrl { get; set; }

        // @property (readonly, nonatomic, strong) BITCrashManager * crashManager;
        [Export ("crashManager", ArgumentSemantic.Strong)]
        CrashManager CrashManager { get; }

        // @property (getter = isCrashManagerDisabled, nonatomic) BOOL disableCrashManager;
        [Export ("disableCrashManager")]
        bool DisableCrashManager { [Bind ("isCrashManagerDisabled")] get; set; }

        // @property (readonly, nonatomic, strong) BITFeedbackManager * feedbackManager;
        [Export ("feedbackManager", ArgumentSemantic.Strong)]
        FeedbackManager FeedbackManager { get; }

        // @property (getter = isFeedbackManagerDisabled, nonatomic) BOOL disableFeedbackManager;
        [Export ("disableFeedbackManager")]
        bool DisableFeedbackManager { [Bind ("isFeedbackManagerDisabled")] get; set; }

        // @property (readonly, nonatomic, strong) BITMetricsManager * metricsManager;
        [Export ("metricsManager", ArgumentSemantic.Strong)]
        MetricsManager MetricsManager { get; }

        // @property (getter = isMetricsManagerDisabled, nonatomic) BOOL disableMetricsManager;
        [Export ("disableMetricsManager")]
        bool DisableMetricsManager { [Bind ("isMetricsManagerDisabled")] get; set; }

        // -(void)setUserID:(NSString *)userID;
        [Export ("setUserID:")]
        void SetUserID (string userID);

        // -(void)setUserName:(NSString *)userName;
        [Export ("setUserName:")]
        void SetUserName (string userName);

        // -(void)setUserEmail:(NSString *)userEmail;
        [Export ("setUserEmail:")]
        void SetUserEmail (string userEmail);

        // @property (getter = isDebugLogEnabled, assign, nonatomic) BOOL debugLogEnabled;
        [Export ("debugLogEnabled")]
        bool DebugLogEnabled { [Bind ("isDebugLogEnabled")] get; set; }

        // -(void)testIdentifier;
        [Export ("testIdentifier")]
        void TestIdentifier ();
    }

    interface ICrashManagerDelegate { }

    // @protocol BITCrashManagerDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof (NSObject), Name="BITCrashManagerDelegate")]
    interface CrashManagerDelegate
    {
        // @optional -(void)showMainApplicationWindowForCrashManager:(BITCrashManager *)crashManager __attribute__((deprecated("The default crash report UI is not shown modal any longer, so this delegate is now called right away. We recommend to remove the implementation of this method.")));
        [Export ("showMainApplicationWindowForCrashManager:")]
        void ShowMainApplicationWindow (CrashManager crashManager);

        // @optional -(NSString *)applicationLogForCrashManager:(BITCrashManager *)crashManager;
        [Export ("applicationLogForCrashManager:")]
        string ApplicationLog (CrashManager crashManager);

        // @optional -(BITHockeyAttachment *)attachmentForCrashManager:(BITCrashManager *)crashManager;
        [Export ("attachmentForCrashManager:")]
        HockeyAttachment Attachment (CrashManager crashManager);

        // @optional -(void)crashManagerWillShowSubmitCrashReportAlert:(BITCrashManager *)crashManager;
        [Export ("crashManagerWillShowSubmitCrashReportAlert:")]
        void WillShowSubmitCrashReportAlert (CrashManager crashManager);

        // @optional -(void)crashManagerWillCancelSendingCrashReport:(BITCrashManager *)crashManager;
        [Export ("crashManagerWillCancelSendingCrashReport:")]
        void WillCancelSendingCrashReport (CrashManager crashManager);

        // @optional -(void)crashManagerWillSendCrashReport:(BITCrashManager *)crashManager;
        [Export ("crashManagerWillSendCrashReport:")]
        void WillSendCrashReport (CrashManager crashManager);

        // @optional -(void)crashManager:(BITCrashManager *)crashManager didFailWithError:(NSError *)error;
        [Export ("crashManager:didFailWithError:")]
        void DidFail (CrashManager crashManager, NSError error);

        // @optional -(void)crashManagerDidFinishSendingCrashReport:(BITCrashManager *)crashManager;
        [Export ("crashManagerDidFinishSendingCrashReport:")]
        void DidFinishSending (CrashManager crashManager);
    }

    interface IHockeyManagerDelegate { }

    // @protocol BITHockeyManagerDelegate <NSObject, BITCrashManagerDelegate>
    [Protocol, Model]
    [BaseType (typeof (NSObject), Name="BITHockeyManagerDelegate")]
    interface HockeyManagerDelegate : CrashManagerDelegate
    {
        // @optional -(NSString *)userIDForHockeyManager:(BITHockeyManager *)hockeyManager componentManager:(BITHockeyBaseManager *)componentManager;
        [Export ("userIDForHockeyManager:componentManager:")]
        string UserID (HockeyManager hockeyManager, HockeyBaseManager componentManager);

        // @optional -(NSString *)userNameForHockeyManager:(BITHockeyManager *)hockeyManager componentManager:(BITHockeyBaseManager *)componentManager;
        [Export ("userNameForHockeyManager:componentManager:")]
        string UserName (HockeyManager hockeyManager, HockeyBaseManager componentManager);

        // @optional -(NSString *)userEmailForHockeyManager:(BITHockeyManager *)hockeyManager componentManager:(BITHockeyBaseManager *)componentManager;
        [Export ("userEmailForHockeyManager:componentManager:")]
        string UserEmail (HockeyManager hockeyManager, HockeyBaseManager componentManager);
    }

    // @interface BITHockeyAttachment : NSObject <NSCoding>
    [BaseType (typeof (NSObject), Name="BITHockeyAttachment")]
    interface HockeyAttachment : INSCoding
    {
        // @property (readonly, nonatomic, strong) NSString * filename;
        [Export ("filename", ArgumentSemantic.Strong)]
        string Filename { get; }

        // @property (readonly, nonatomic, strong) NSData * hockeyAttachmentData;
        [Export ("hockeyAttachmentData", ArgumentSemantic.Strong)]
        NSData HockeyAttachmentData { get; }

        // @property (readonly, nonatomic, strong) NSString * contentType;
        [Export ("contentType", ArgumentSemantic.Strong)]
        string ContentType { get; }

        // -(instancetype)initWithFilename:(NSString *)filename hockeyAttachmentData:(NSData *)hockeyAttachmentData contentType:(NSString *)contentType;
        [Export ("initWithFilename:hockeyAttachmentData:contentType:")]
        IntPtr Constructor (string filename, NSData hockeyAttachmentData, string contentType);
    }

    // @interface BITHockeyBaseManager : NSObject
    [BaseType (typeof (NSObject), Name="BITHockeyBaseManager")]
    interface HockeyBaseManager
    {
        // @property (nonatomic, strong) NSString * serverURL;
        [Export ("serverURL", ArgumentSemantic.Strong)]
        string ServerUrl { get; set; }
    }

    // typedef void (^BITCustomCrashReportUIHandler)(NSString *, NSString *);
    delegate void CustomCrashReportUIHandler (string arg0, string arg1);

    // @interface BITCrashManager : BITHockeyBaseManager
    [BaseType (typeof (HockeyBaseManager), Name="BITCrashManager")]
    interface CrashManager
    {
        // @property (assign, nonatomic) BOOL askUserDetails;
        [Export ("askUserDetails")]
        bool AskUserDetails { get; set; }

        // @property (getter = isMachExceptionHandlerEnabled, assign, nonatomic) BOOL enableMachExceptionHandler __attribute__((deprecated("Mach Exceptions are now enabled by default. If you want to disable them, please use the new property disableMachExceptionHandler")));
        [Export ("enableMachExceptionHandler")]
        bool EnableMachExceptionHandler { [Bind ("isMachExceptionHandlerEnabled")] get; set; }

        // @property (getter = isMachExceptionHandlerDisabled, assign, nonatomic) BOOL disableMachExceptionHandler;
        [Export ("disableMachExceptionHandler")]
        bool DisableMachExceptionHandler { [Bind ("isMachExceptionHandlerDisabled")] get; set; }

        // @property (getter = isAutoSubmitCrashReport, assign, nonatomic) BOOL autoSubmitCrashReport;
        [Export ("autoSubmitCrashReport")]
        bool AutoSubmitCrashReport { [Bind ("isAutoSubmitCrashReport")] get; set; }

        // TODO:
        // -(void)setCrashCallbacks:(BITCrashManagerCallbacks *)callbacks;
        //[Export ("setCrashCallbacks:")]
        //unsafe void SetCrashCallbacks (CrashManagerCallbacks* callbacks);

        // @property (readonly, nonatomic) BOOL didCrashInLastSession;
        [Export ("didCrashInLastSession")]
        bool DidCrashInLastSession { get; }

        // -(BOOL)handleUserInput:(BITCrashManagerUserInput)userInput withUserProvidedMetaData:(BITCrashMetaData *)userProvidedMetaData;
        [Export ("handleUserInput:withUserProvidedMetaData:")]
        bool HandleUserInput (CrashManagerUserInput userInput, CrashMetaData userProvidedMetaData);

        // -(void)setCrashReportUIHandler:(BITCustomCrashReportUIHandler)crashReportUIHandler;
        [Export ("setCrashReportUIHandler:")]
        void SetCrashReportUIHandler (CustomCrashReportUIHandler crashReportUIHandler);

        // @property (readonly, nonatomic) BITCrashDetails * lastSessionCrashDetails;
        [Export ("lastSessionCrashDetails")]
        CrashDetails LastSessionCrashDetails { get; }

        // @property (readonly, nonatomic) NSTimeInterval timeintervalCrashInLastSessionOccured;
        [Export ("timeintervalCrashInLastSessionOccured")]
        double TimeintervalCrashInLastSessionOccured { get; }

        // -(void)generateTestCrash;
        [Export ("generateTestCrash")]
        void GenerateTestCrash ();
    }

    // @interface BITCrashDetails : NSObject
    [BaseType (typeof (NSObject), Name="BITCrashDetails")]
    interface CrashDetails
    {
        // @property (readonly, nonatomic, strong) NSString * incidentIdentifier;
        [Export ("incidentIdentifier", ArgumentSemantic.Strong)]
        string IncidentIdentifier { get; }

        // @property (readonly, nonatomic, strong) NSString * reporterKey;
        [Export ("reporterKey", ArgumentSemantic.Strong)]
        string ReporterKey { get; }

        // @property (readonly, nonatomic, strong) NSString * signal;
        [Export ("signal", ArgumentSemantic.Strong)]
        string Signal { get; }

        // @property (readonly, nonatomic, strong) NSString * exceptionName;
        [Export ("exceptionName", ArgumentSemantic.Strong)]
        string ExceptionName { get; }

        // @property (readonly, nonatomic, strong) NSString * exceptionReason;
        [Export ("exceptionReason", ArgumentSemantic.Strong)]
        string ExceptionReason { get; }

        // @property (readonly, nonatomic, strong) NSDate * appStartTime;
        [Export ("appStartTime", ArgumentSemantic.Strong)]
        NSDate AppStartTime { get; }

        // @property (readonly, nonatomic, strong) NSDate * crashTime;
        [Export ("crashTime", ArgumentSemantic.Strong)]
        NSDate CrashTime { get; }

        // @property (readonly, nonatomic, strong) NSString * osVersion;
        [Export ("osVersion", ArgumentSemantic.Strong)]
        string OsVersion { get; }

        // @property (readonly, nonatomic, strong) NSString * osBuild;
        [Export ("osBuild", ArgumentSemantic.Strong)]
        string OsBuild { get; }

        // @property (readonly, nonatomic, strong) NSString * appVersion;
        [Export ("appVersion", ArgumentSemantic.Strong)]
        string AppVersion { get; }

        // @property (readonly, nonatomic, strong) NSString * appBuild;
        [Export ("appBuild", ArgumentSemantic.Strong)]
        string AppBuild { get; }
    }

    // @interface BITCrashMetaData : NSObject
    [BaseType (typeof (NSObject), Name="BITCrashMetaData")]
    interface CrashMetaData
    {
        // @property (copy, nonatomic) NSString * userDescription;
        [Export ("userDescription")]
        string UserDescription { get; set; }

        // @property (copy, nonatomic) NSString * userName;
        [Export ("userName")]
        string UserName { get; set; }

        // @property (copy, nonatomic) NSString * userEmail;
        [Export ("userEmail")]
        string UserEmail { get; set; }

        // @property (copy, nonatomic) NSString * userID;
        [Export ("userID")]
        string UserID { get; set; }
    }

    // @interface BITCrashExceptionApplication : NSApplication
    [BaseType (typeof (NSApplication), Name="BITCrashExceptionApplication")]
    interface CrashExceptionApplication
    {
    }

    // @interface BITSystemProfile : NSObject
    [BaseType (typeof (NSObject), Name="BITSystemProfile")]
    interface SystemProfile
    {
        // +(BITSystemProfile *)sharedSystemProfile;
        [Static]
        [Export ("sharedSystemProfile")]
        SystemProfile SharedSystemProfile { get; }

        // +(NSString *)deviceIdentifier;
        [Static]
        [Export ("deviceIdentifier")]
        string DeviceIdentifier { get; }

        // +(NSString *)deviceModel;
        [Static]
        [Export ("deviceModel")]
        string DeviceModel { get; }

        // +(NSString *)systemVersionString;
        [Static]
        [Export ("systemVersionString")]
        string SystemVersionString { get; }

        // -(NSMutableArray *)systemDataForBundle:(NSBundle *)bundle;
        [Export ("systemDataForBundle:")]
        NSMutableArray GetSystemData (NSBundle bundle);

        // -(NSMutableArray *)systemData;
        [Export ("systemData")]
        NSMutableArray GetSystemData ();

        // -(NSMutableArray *)systemUsageDataForBundle:(NSBundle *)bundle;
        [Export ("systemUsageDataForBundle:")]
        NSMutableArray GetSystemUsageData (NSBundle bundle);

        // -(NSMutableArray *)systemUsageData;
        [Export ("systemUsageData")]
        NSMutableArray GetSystemUsageData ();

        // -(void)startUsageForBundle:(NSBundle *)bundle;
        [Export ("startUsageForBundle:")]
        void StartUsage (NSBundle bundle);

        // -(void)startUsage;
        [Export ("startUsage")]
        void StartUsage ();

        // -(void)stopUsage;
        [Export ("stopUsage")]
        void StopUsage ();
    }

    // @interface BITFeedbackManager : BITHockeyBaseManager
    [BaseType (typeof (HockeyBaseManager), Name="BITFeedbackManager")]
    interface FeedbackManager
    {
        // @property (readwrite, nonatomic) BITFeedbackUserDataElement requireUserName;
        [Export ("requireUserName", ArgumentSemantic.Assign)]
        FeedbackUserDataElement RequireUserName { get; set; }

        // @property (readwrite, nonatomic) BITFeedbackUserDataElement requireUserEmail;
        [Export ("requireUserEmail", ArgumentSemantic.Assign)]
        FeedbackUserDataElement RequireUserEmail { get; set; }

        // @property (readwrite, nonatomic) BOOL showAlertOnIncomingMessages;
        [Export ("showAlertOnIncomingMessages")]
        bool ShowAlertOnIncomingMessages { get; set; }

        // -(void)showFeedbackWindow;
        [Export ("showFeedbackWindow")]
        void ShowFeedbackWindow ();
    }

    // @interface BITFeedbackWindowController : NSWindowController
    [BaseType (typeof (NSWindowController), Name="FeedbackWindowController")]
    interface FeedbackWindowController
    {
        // -(id)initWithManager:(BITFeedbackManager *)feedbackManager;
        [Export ("initWithManager:")]
        IntPtr Constructor (FeedbackManager feedbackManager);
    }

    // @interface BITMetricsManager : BITHockeyBaseManager
    [BaseType (typeof (HockeyBaseManager), Name="BITMetricsManager")]
    interface MetricsManager
    {
        // -(void)trackEventWithName:(NSString * _Nonnull)eventName;
        [Export ("trackEventWithName:")]
        void TrackEvent (string eventName);
    }

    [Static]
    partial interface Constants
    {
        // extern NSString *const kBITDefaultUserID __attribute__((unused));
        [Field ("kBITDefaultUserID", "__Internal")]
        NSString DefaultUserID { get; }

        // extern NSString *const kBITDefaultUserName __attribute__((unused));
        [Field ("kBITDefaultUserName", "__Internal")]
        NSString DefaultUserName { get; }

        // extern NSString *const kBITDefaultUserEmail __attribute__((unused));
        [Field ("kBITDefaultUserEmail", "__Internal")]
        NSString DefaultUserEmail { get; }

        // extern NSString *const kBITCrashErrorDomain __attribute__((unused));
        [Field ("kBITCrashErrorDomain", "__Internal")]
        NSString CrashErrorDomain { get; }

        // extern NSString *const kBITFeedbackErrorDomain __attribute__((unused));
        [Field ("kBITFeedbackErrorDomain", "__Internal")]
        NSString FeedbackErrorDomain { get; }

        // extern NSString *const kBITHockeyErrorDomain __attribute__((unused));
        [Field ("kBITHockeyErrorDomain", "__Internal")]
        NSString HockeyErrorDomain { get; }
    }
}