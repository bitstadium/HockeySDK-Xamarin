using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using QuickLook;

namespace HockeyApp.iOS
{
    // typedef NSString * (^BITLogMessageProvider)();
    public delegate NSString BITLogMessageProvider ();

    // typedef void (^BITLogHandler)(BITLogMessageProvider, BITLogLevel, const char *, const char *, uint);
    public unsafe delegate void BITLogHandler (BITLogMessageProvider messageProvider, BITLogLevel logLevel, IntPtr file, IntPtr function, uint line);

    [Static]
    partial interface Constants
    {
        // extern NSString *const kBITCrashErrorDomain;
        [Field ("kBITCrashErrorDomain", "__Internal")]
        NSString CrashErrorDomain { get; }
    
        // extern NSString *const kBITUpdateErrorDomain;
        [Field ("kBITUpdateErrorDomain", "__Internal")]
        NSString UpdateErrorDomain { get; }
    
        // extern NSString *const kBITFeedbackErrorDomain;
        [Field ("kBITFeedbackErrorDomain", "__Internal")]
        NSString FeedbackErrorDomain { get; }
    
        // extern NSString *const kBITAuthenticatorErrorDomain;
        [Field ("kBITAuthenticatorErrorDomain", "__Internal")]
        NSString AuthenticatorErrorDomain { get; }
    
        // extern NSString *const kBITHockeyErrorDomain;
        [Field ("kBITHockeyErrorDomain", "__Internal")]
        NSString HockeyErrorDomain { get; }
    }

    [BaseType (typeof (NSObject))]
    public partial interface BITHockeyBaseManager 
    {
        [Export("serverURL")]
        string ServerUrl { get;set; }

        // @property (assign, nonatomic) UIBarStyle barStyle;
        [Export ("barStyle", ArgumentSemantic.Assign)]
        UIBarStyle BarStyle { get; set; }

        // @property (nonatomic, strong) UIColor * navigationBarTintColor;
        [Export ("navigationBarTintColor", ArgumentSemantic.Strong)]
        UIColor NavigationBarTintColor { get; set; }

        // @property (assign, nonatomic) UIModalPresentationStyle modalPresentationStyle;
        [Export ("modalPresentationStyle", ArgumentSemantic.Assign)]
        UIModalPresentationStyle ModalPresentationStyle { get; set; }
    }

    [BaseType (typeof(NSObject))]
    [DisableDefaultCtor]
    public partial interface BITHockeyManager
    {
        [Static]
        [Export("sharedHockeyManager")]
        BITHockeyManager SharedHockeyManager { get;set; }

        [Wrap ("WeakDelegate")][NullAllowed]
        BITHockeyManagerDelegate Delegate { get; set; }

        [Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
        NSObject WeakDelegate { get; set; }

        [Export("configureWithIdentifier:")]
        void Configure(string appIdentifier);

        [Export("configureWithIdentifier:delegate:")]
        void Configure(string appIdentifier, [NullAllowed] NSObject managerDelegate);

        [Export("configureWithBetaIdentifier:liveIdentifier:delegate:")]
        void ConfigureWithIdentifier(string betaIdentifier, string liveIdentifier, [NullAllowed] NSObject managerDelegate);

        [Internal]
        [Export("startManager")]
        void DoStartManager();

        [Export ("serverURL", ArgumentSemantic.Retain)]
        string ServerURL { get; set; }

        [Export ("crashManager", ArgumentSemantic.Retain)]
        BITCrashManager CrashManager { get; }

        [Export ("disableCrashManager")]
        bool DisableCrashManager { [Bind ("isCrashManagerDisabled")] get; set; }

        #if !CRASHONLY
        [Export ("updateManager", ArgumentSemantic.Retain)]
        BITUpdateManager UpdateManager { get; }
        #endif

        [Export ("disableUpdateManager")]
        bool DisableUpdateManager { [Bind ("isUpdateManagerDisabled")] get; set; }

        #if !CRASHONLY
        [Export ("storeUpdateManager", ArgumentSemantic.Retain)]
        BITStoreUpdateManager StoreUpdateManager { get; }
        #endif

        [Export ("enableStoreUpdateManager")]
        bool EnableStoreUpdateManager { [Bind ("isStoreUpdateManagerEnabled")] get; set; }

        #if !CRASHONLY
        [Export ("feedbackManager", ArgumentSemantic.Retain)]
        BITFeedbackManager FeedbackManager { get; }
        #endif

        [Export ("disableFeedbackManager")]
        bool DisableFeedbackManager { [Bind ("isFeedbackManagerDisabled")] get; set; }

        #if !CRASHONLY
        [Export ("authenticator", ArgumentSemantic.Retain)]
        BITAuthenticator Authenticator { get; }
        #endif


        #if !CRASHONLY
        // @property (nonatomic, strong, readonly) BITMetricsManager* metricsManager;
        [Export ("metricsManager", ArgumentSemantic.Strong)]
        BITMetricsManager MetricsManager { get; }

        // @property (nonatomic, getter = isMetricsManagerDisabled) BOOL disableMetricsManager;
        /// <summary>
        /// Gets or sets the disable metrics manager.  Must be set before calling StartManager
        /// </summary>
        /// <value>If the MetricsManager is disabled.</value>
        [Export ("disableMetricsManager")]
        bool DisableMetricsManager {
            [Bind ("isMetricsManagerDisabled")] get;
            set;
        }
        #endif

        // Added in 3.8.4
        //@property (nonatomic, readonly) BITEnvironment appEnvironment;
        [Export ("appEnvironment")]
        BITEnvironment AppEnvironment { get; }

        [Export ("installString")]
        string InstallString { get; }

        [Export ("disableInstallTracking")]
        bool DisableInstallTracking { [Bind ("isInstallTrackingDisabled")] get; set; }

        // @property (assign, nonatomic) BITLogLevel logLevel;
        [Export ("logLevel", ArgumentSemantic.Retain)]
        BITLogLevel LogLevel { get; set; }

        // @property (getter = isDebugLogEnabled, assign, nonatomic) BOOL debugLogEnabled __attribute__((deprecated("Use logLevel instead!")));
        [Export ("debugLogEnabled")]
        bool DebugLogEnabled { [Bind ("isDebugLogEnabled")] get; set; }

        // -(void)setLogHandler:(BITLogHandler _Nonnull)logHandler;
        [Export ("setLogHandler:")]
        void SetLogHandler (BITLogHandler logHandler);

        [Export("userID", ArgumentSemantic.Retain)][NullAllowed]
        string UserId { get;set; }

        [Export("userName", ArgumentSemantic.Retain)][NullAllowed]
        string UserName { get;set; }

        [Export("userEmail", ArgumentSemantic.Retain)][NullAllowed]
        string UserEmail { get;set; }

        [Export("testIdentifier")]
        void TestIdentifier();

        [Export("version")]
        string Version();

        [Export("build")]
        string Build();       
    }

    #if !CRASHONLY
    public delegate void BITAuthenticatorIdentifyCallback (bool identified, NSError error);
    public delegate void BITAuthenticatorValidateCallback (bool validated, NSError error);
    #endif

    #if !CRASHONLY
//    [BaseType(typeof(BITHockeyBaseManager),
//        Delegates=new string [] {"WeakDelegate"}, 
//        Events=new Type [] { typeof (BITAuthenticatorDelegate) })]
    [BaseType (typeof (BITHockeyBaseManager))]
    public partial interface BITAuthenticator 
    {
        // Removed in 3.6.2
//        [Wrap ("WeakDelegate")]
//        BITAuthenticatorDelegate Delegate { get; set; }
//
//        [Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
//        NSObject WeakDelegate { get; set; }

        [Export("identificationType")]
        BITAuthenticatorIdentificationType IdentificationType { get;set; }

        [Export("restrictApplicationUsage")]
        bool RestrictApplicationUsage { get;set; }

        [Export("restrictionEnforcementFrequency")]
        BITAuthenticatorAppRestrictionEnforcementFrequency RestrictionEnforcementFrequency { get; set; }

        [Export("authenticationSecret")]
        string AuthenticationSecret { get;set; }

        [Export("webpageURL")]
        NSUrl WebpageUrl { get;set; }

        [Export("deviceAuthenticationURL")]
        NSUrl DeviceAuthenticationUrl { get; }

        [Export("urlScheme")]
        string UrlScheme { get;set; }

        [Export("handleOpenURL:sourceApplication:annotation:")]
        bool HandleOpenUrl(NSUrl url, string sourceApplication, [NullAllowed] NSObject annotation);

        [Export("authenticateInstallation")]
        void AuthenticateInstallation();

        [Export("identifyWithCompletion:")]
        void IdentifyWithCompletion (BITAuthenticatorIdentifyCallback completion);

        [Export ("identified")]
        bool Identified { [Bind ("isIdentified")] get; }

        [Export("validateWithCompletion:")]
        bool ValidateWithCompletion (BITAuthenticatorValidateCallback completion);

        [Export ("validated")]
        bool Validated { [Bind ("isValidated")] get; }

        [Export("cleanupInternalStorage")]
        void CleanupInternalStorage();

        [Export("publicInstallationIdentifier")]
        string PublicInstallationIdentifier { get; }
    }
    #endif

    #if !CRASHONLY
    [BaseType (typeof (BITHockeyBaseManager))]
    public partial interface BITMetricsManager
    {
        // @property (nonatomic, assign) BOOL disabled;

        [Export ("disabled", ArgumentSemantic.Assign)]
        bool Disabled { get; set; }

        // - (void)trackEventWithName:(NSString*)eventName;
        [Export ("trackEventWithName:")]
        void TrackEvent (string eventName);

		// - (void)trackEventWithName:(nonnull NSString *)eventName properties:(nullable NSDictionary<NSString *, NSString *> *)properties measurements:(nullable NSDictionary<NSString *, NSNumber *> *)measurements;
		[Export("trackEventWithName:properties:measurements:")]
		void TrackEvent(string eventName, [NullAllowed] NSDictionary properties, [NullAllowed] NSDictionary measurements);

		[Export ("serverURL", ArgumentSemantic.Retain)]
		string ServerURL { get; set; }
    }
    #endif

    #if !CRASHONLY
    [BaseType(typeof(BITHockeyBaseManager))]
    public partial interface BITFeedbackManager
    {
        // Removed in 3.6.2
//        [Wrap ("WeakDelegate")]
//        BITFeedbackManagerDelegate Delegate { get; set; }
//
//        [Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
//        NSObject WeakDelegate { get; set; }

        [Export("requireUserName")]
        BITFeedbackUserDataElement RequireUserName { get;set; }

        [Export("requireUserEmail")]
        BITFeedbackUserDataElement RequireUserEmail { get;set; }

        [Export("showAlertOnIncomingMessages")]
        bool ShowAlertOnIncomingMessages { get;set; }

        // @property (readwrite, nonatomic) BITFeedbackObservationMode feedbackObservationMode;
        [Export ("feedbackObservationMode")]
        BITFeedbackObservationMode FeedbackObservationMode { get; set; }

        [Export("showFirstRequiredPresentationModal")]
        bool ShowFirstRequiredPresentationModel { get;set; }

        // -(UIImage *)screenshot;
        [Export ("screenshot")]
        UIImage Screenshot ();

        [Export("showFeedbackListView")]
        void ShowFeedbackListView();

        [Export("feedbackListViewController:")]
        UITableViewController FeedbackListViewController(bool modal);

        [Export("showFeedbackComposeView")]
        void ShowFeedbackComposeView();

        // -(void)showFeedbackComposeViewWithPreparedItems:(NSArray *)items;
        [Export ("showFeedbackComposeViewWithPreparedItems:")]
        void ShowFeedbackComposeViewWithPreparedItems (NSObject [] items);

        // -(void)showFeedbackComposeViewWithGeneratedScreenshot;
        [Export ("showFeedbackComposeViewWithGeneratedScreenshot")]
        void ShowFeedbackComposeViewWithGeneratedScreenshot ();

        [Export("feedbackComposeViewController")]
        BITFeedbackComposeViewController FeedbackComposeViewController();

        // @property (copy, nonatomic) NSArray * _Nullable feedbackComposerPreparedItems __attribute__((deprecated("Use -preparedItemsForFeedbackManager: delegate method instead.")));
        [Obsolete ("Use -preparedItemsForFeedbackManager: delegate method instead.")]
        [Export("feedbackComposerPreparedItems")]
        NSArray FeedbackComposerPreparedItems { get;set; }

        // Added in 3.7.1
        [Export ("feedbackComposeHideImageAttachmentButton")]
        bool FeedbackComposeHideImageAttachmentButton { get; set; }
    }
    #endif

    // @interface BITCrashDetails : NSObject
    [BaseType (typeof (NSObject))]
    public partial interface BITCrashDetails {

        // @property (readonly, nonatomic, strong) NSString * incidentIdentifier;
        [Export ("incidentIdentifier", ArgumentSemantic.Retain)]
        string IncidentIdentifier { get; }

        // @property (readonly, nonatomic, strong) NSString * reporterKey;
        [Export ("reporterKey", ArgumentSemantic.Retain)]
        string ReporterKey { get; }

        // @property (readonly, nonatomic, strong) NSString * signal;
        [Export ("signal", ArgumentSemantic.Retain)]
        string Signal { get; }

        // @property (readonly, nonatomic, strong) NSString * exceptionName;
        [Export ("exceptionName", ArgumentSemantic.Retain)]
        string ExceptionName { get; }

        // @property (readonly, nonatomic, strong) NSString * exceptionReason;
        [Export ("exceptionReason", ArgumentSemantic.Retain)]
        string ExceptionReason { get; }

        // @property (readonly, nonatomic, strong) NSDate * appStartTime;
        [Export ("appStartTime", ArgumentSemantic.Retain)]
        NSDate AppStartTime { get; }

        // @property (readonly, nonatomic, strong) NSDate * crashTime;
        [Export ("crashTime", ArgumentSemantic.Retain)]
        NSDate CrashTime { get; }

        // @property (readonly, nonatomic, strong) NSString * osVersion;
        [Export ("osVersion", ArgumentSemantic.Retain)]
        string OsVersion { get; }

        // @property (readonly, nonatomic, strong) NSString * osBuild;
        [Export ("osBuild", ArgumentSemantic.Retain)]
        string OsBuild { get; }

        // Added in 3.7.1
        [Export ("appVersion", ArgumentSemantic.Strong)]
        string AppVersion { get; }

        // Added in 3.7.1
        [Export ("appProcessIdentifier", ArgumentSemantic.Assign)]
        nuint AppProcessIdentifier { get; }

        // @property (readonly, nonatomic, strong) NSString * appBuild;
        [Export ("appBuild", ArgumentSemantic.Retain)]
        string AppBuild { get; }

        // -(BOOL)isAppKill;
        [Export ("isAppKill")]
        bool IsAppKill { get; }
    }

    #if !CRASHONLY
    [BaseType(typeof(NSObject))]
    public partial interface BITUpdateManager
    {
        // Removed in 3.6.2
//        [Wrap ("WeakDelegate")]
//        BITUpdateManagerDelegate Delegate { get; set; }
//
//        [Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
//        NSObject WeakDelegate { get; set; }

        [Export("updateSetting", ArgumentSemantic.Assign)]
        BITUpdateSetting UpdateSetting { get;set; }

        [Export("checkForUpdateOnLaunch")]
        bool CheckForUpdateOnLaunch { [Bind("isCheckForUpdateOnLaunch")]get;set; }

        [Export("checkForUpdate")]
        void CheckForUpdate();

        [Export("alwaysShowUpdateReminder")]
        bool AlwaysShowUpdateReminder { get;set; }

        [Export("showDirectInstallOption")]
        bool ShowDirectInstallOption { [Bind("isShowingDirectInstallOption")]get; set; }

        [Export("expiryDate")]
        NSDate ExpiryDate { get;set; }

        [Export("disableUpdateCheckOptionWhenExpired")]
        bool DisableUpdateCheckOptionWhenExpired { get;set; }

        [Export("hockeyViewController:")]
        UIViewController HockeyViewController(bool modal);

        [Export("showUpdateView")]
        void ShowUpdateView();
    }
    #endif

    #if !CRASHONLY
    // @interface BITHockeyBaseViewController : UITableViewController
    [BaseType (typeof (UITableViewController))]
    interface BITHockeyBaseViewController {

        // -(instancetype)initWithModalStyle:(BOOL)modal;
        [Export ("initWithModalStyle:")]
        IntPtr Constructor (bool modal);

        // -(instancetype)initWithStyle:(UITableViewStyle)style modal:(BOOL)modal;
        [Export ("initWithStyle:modal:")]
        IntPtr Constructor (UITableViewStyle style, bool modal);

        // @property (readwrite, nonatomic) BOOL modalAnimated;
        [Export ("modalAnimated")]
        bool ModalAnimated { get; set; }
    }
    #endif

    // @interface BITCrashMetaData : NSObject
    [BaseType (typeof (NSObject))]
    public partial interface BITCrashMetaData {

        // @property (copy, nonatomic) NSString * userDescription;
        [Export ("userProvidedDescription")]
        string UserProvidedDescription { get; set; }

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

    [BaseType(typeof(BITHockeyBaseManager))]
    public partial interface BITCrashManager 
    {
        // Removed in 3.6.2
//        [Wrap ("WeakDelegate")]
//        BITCrashManagerDelegate Delegate { get; set; }
//
//        [Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
//        NSObject WeakDelegate { get; set; }

        [Export("crashManagerStatus")]
        BITCrashManagerStatus CrashManagerStatus { get;set; }

        [Export("enableMachExceptionHandler")]
        bool EnableMachExceptionHandler { [Bind("isMachExceptionHandlerEnabled")]get; set; } 

        [Export("enableOnDeviceSymbolication")]
        bool EnableOnDeviceSymbolication { [Bind("isOnDeviceSymbolicationEnabled")]get; set; }

        // Added in 3.6.2
        // @property (assign, nonatomic, getter = isAppNotTerminatingCleanlyDetectionEnabled) BOOL enableAppNotTerminatingCleanlyDetection;
        [Export ("enableAppNotTerminatingCleanlyDetection", ArgumentSemantic.UnsafeUnretained)]
        bool EnableAppNotTerminatingCleanlyDetection { [Bind ("isAppNotTerminatingCleanlyDetectionEnabled")] get; set; }

        // -(void)setCrashCallbacks:(BITCrashManagerCallbacks *)callbacks;
        //[Export ("setCrashCallbacks:")]
        //unsafe void SetCrashCallbacks (BITCrashManagerCallbacks* callbacks);


        [Export("showAlwaysButton")]
        bool ShowAlwaysButton { [Bind("shouldShowAlwaysButton")]get; set; }

        [Export("didCrashInLastSession")]
        bool DidCrashInLastSession { get; }

        // @property (readonly, nonatomic) BITCrashDetails * lastSessionCrashDetails;
        [Export ("lastSessionCrashDetails")]
        BITCrashDetails LastSessionCrashDetails { get; }

        [Export("isDebuggerAttached")]
        bool IsDebuggerAttached();

        [Export("generateTestCrash")]
        void GenerateTestCrash();

        // -(BOOL)handleUserInput:(BITCrashManagerUserInput)userInput withUserProvidedMetaData:(BITCrashMetaData *)userProvidedMetaData;
        [Export ("handleUserInput:withUserProvidedMetaData:")]
        bool HandleUserInput (BITCrashManagerUserInput userInput, BITCrashMetaData userProvidedMetaData);

        // -(void)setAlertViewHandler:(BITCustomAlertViewHandler)alertViewHandler;
        [Export ("setAlertViewHandler:")]
        void SetAlertViewHandler (Action alertViewHandler);

        // @property (readonly, nonatomic) BOOL didReceiveMemoryWarningInLastSession;
        [Export ("didReceiveMemoryWarningInLastSession")]
        bool DidReceiveMemoryWarningInLastSession { get; }

        // @property (readonly, nonatomic) NSTimeInterval timeIntervalCrashInLastSessionOccurred;
        [Export ("timeIntervalCrashInLastSessionOccurred")]
        double TimeIntervalCrashInLastSessionOccurred { get; }
    }

    #if !CRASHONLY
    [BaseType(typeof(BITHockeyBaseManager))]
    public partial interface BITStoreUpdateManager
    {
        // Removed in 3.6.2
//        [Wrap ("WeakDelegate")]
//        BITStoreUpdateManagerDelegate Delegate { get; set; }
//
//        [Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
//        NSObject WeakDelegate { get; set; }

        [Export("updateSetting", ArgumentSemantic.Assign)]
        BITUpdateSetting UpdateSetting { get;set; }

        [Export("countryCode")]
        string CountryCode { get;set; }

        [Export("checkForUpdateOnLaunch")]
        bool CheckForUpdateOnLaunch { [Bind("isCheckingForUpdateOnLaunch")]get;set; }

        [Export("updateUIEnabled")]
        bool UpdateUIEnabled { [Bind("isUpdateUIEnabled")]get;set; }

        [Export("checkForUpdate")]
        void CheckForUpdate();
    }
    #endif

    #if !CRASHONLY
    [BaseType(typeof(UIViewController))]
    public partial interface BITFeedbackComposeViewController : IUITextViewDelegate
    {
        [Wrap ("WeakDelegate")]
        BITFeedbackComposeViewControllerDelegate Delegate { get; set; }

        // Added in 3.7.1
        [Export ("hideImageAttachmentButton")]
        bool HideImageAttachmentButton { get;set; }

        [Export ("delegate", ArgumentSemantic.Weak)][NullAllowed]
        NSObject WeakDelegate { get; set; }

        [Export("prepareWithItems:")]
        void PrepareWithItems(NSArray items);
    }
    #endif

    #if !CRASHONLY
    // @interface BITUpdateViewController : BITHockeyBaseViewController
    [BaseType (typeof (BITHockeyBaseViewController))]
    public partial interface BITUpdateViewController {

    }
    #endif

    [BaseType (typeof (BITHockeyAttachment))]
    public partial interface BITCrashAttachment {

        // Removed in 3.6.2
//        [Export ("filename", ArgumentSemantic.Retain)]
//        string Filename { get; }
//
//        [Export ("crashAttachmentData", ArgumentSemantic.Retain)]
//        NSData AttachmentData { get; }
//
//        [Export ("contentType", ArgumentSemantic.Retain)]
//        string ContentType { get; }

        [Export ("initWithFilename:crashAttachmentData:contentType:")]
        IntPtr Constructor (string filename, NSData crashAttachmentData, string contentType);
    }

    #if !CRASHONLY
    [BaseType(typeof(NSObject))]
    [Model][Protocol]
    public partial interface BITAuthenticatorDelegate
    {
        [Export("authenticator:willShowAuthenticationController:")]
        [EventArgs("WillShowAuthenticationController")]
        void WillShowAuthenticationController(BITAuthenticator authenticator, UIViewController viewController);
    }
    #endif

    [BaseType(typeof(NSObject))]
    [Model][Protocol]
    public partial interface BITCrashManagerDelegate
    {
        [Export ("applicationLogForCrashManager:")]
        string ApplicationLogForCrashManager (BITCrashManager crashManager);

        [Export ("attachmentForCrashManager:")]
        BITHockeyAttachment AttachmentForCrashManager (BITCrashManager crashManager);

        [Export ("crashManagerWillShowSubmitCrashReportAlert:")]
        void WillShowSubmitCrashReportAlert (BITCrashManager crashManager);

        [Export ("crashManagerWillCancelSendingCrashReport:")]
        void WillCancelSendingCrashReport (BITCrashManager crashManager);

        [Export ("crashManagerWillSendCrashReportsAlways:")]
        void WillSendCrashReportsAlways (BITCrashManager crashManager);

        [Export ("crashManagerWillSendCrashReport:")]
        void WillSendCrashReport (BITCrashManager crashManager);

        [Export ("crashManager:didFailWithError:")]
        void DidFailWithError (BITCrashManager crashManager, NSError error);

        [Export ("crashManagerDidFinishSendingCrashReport:")]
        void DidFinishSendingCrashReport (BITCrashManager crashManager);

        // @optional -(BOOL)considerAppNotTerminatedCleanlyReportForCrashManager:(BITCrashManager *)crashManager;
        [Export ("considerAppNotTerminatedCleanlyReportForCrashManager:")]
        bool ConsiderAppNotTerminatedCleanlyReportForCrashManager (BITCrashManager crashManager);
    }

    #if !CRASHONLY
    [BaseType (typeof (NSObject))]
    [Model][Protocol]
    public partial interface BITUpdateManagerDelegate 
    {
		[Export("shouldDisplayUpdateAlertForUpdateManager:forShortVersion:forVersion:")]
		bool ShouldDisplayUpdateAlertForUpdateManager(BITUpdateManager updateManager, NSString shortVersion, NSString version);

		[Export ("shouldDisplayExpiryAlertForUpdateManager:")]
        bool ShouldDisplayExpiryAlertForUpdateManager (BITUpdateManager updateManager);

        [Export ("didDisplayExpiryAlertForUpdateManager:")]
        void DidDisplayExpiryAlertForUpdateManager (BITUpdateManager updateManager);

        [Export ("updateManagerShouldSendUsageData:")]
        bool UpdateManagerShouldSendUsageData (BITUpdateManager updateManager);

        // @optional -(void)updateManagerWillExitApp:(BITUpdateManager *)updateManager;
        [Export ("updateManagerWillExitApp:")]
        void UpdateManagerWillExitApp (BITUpdateManager updateManager);

        //(BOOL)willStartDownloadAndUpdate:(BITUpdateManager *)updateManager;
        [Export ("willStartDownloadAndUpdate:")]
        bool WillStartDownloadAndUpdated (BITUpdateManager updateManager);
    }
    #endif

    #if !CRASHONLY
    [BaseType (typeof (BITFeedbackComposeViewControllerDelegate))]
    [Model][Protocol]
    public partial interface BITFeedbackManagerDelegate //: BITFeedbackComposeViewControllerDelegate 
    {
        [Export ("feedbackManagerDidReceiveNewFeedback:")]
        void DidReceiveNewFeedback (BITFeedbackManager feedbackManager);

        // Added in 3.7.1
        //- (BOOL) allowAutomaticFetchingForNewFeedbackForManager:(BITFeedbackManager *)feedbackManager;
        [Export ("allowAutomaticFetchingForNewFeedbackForManager:")]
        bool AllowAutomaticFetching (BITFeedbackManager feedbackManager);

        // @optional -(NSArray * _Nullable)preparedItemsForFeedbackManager:(BITFeedbackManager * _Nonnull)feedbackManager;
        [Export ("preparedItemsForFeedbackManager:")]
        NSArray PreparedItemsForFeedbackManager (BITFeedbackManager feedbackManager);

		// Added in 4.1.3
		[Export("forceNewFeedbackThreadForFeedbackManager:")]
		bool ForceNewFeedbackThreadForFeedbackManager (BITFeedbackManager feedbackManager);
    }
    #endif

    [BaseType(typeof(NSObject))]
    [Model][Protocol]
    public partial interface BITHockeyManagerDelegate
    {	
		// BITHockeyManagerDelegate

        [Export("shouldUseLiveIdentifierForHockeyManager:")]
        bool ShouldUseLiveIdentifierForHockeyManager(BITHockeyManager manager);

        [Export("viewControllerForHockeyManager:componentManager:")]
        UIViewController ViewControllerForHockeyManager(BITHockeyManager hockeyManager, BITHockeyBaseManager componentManager);

        [Export("userIDForHockeyManager:componentManager:")]
        string UserIdForHockeyManager(BITHockeyManager hockeyManager, BITHockeyBaseManager componentManager);

        [Export("userNameForHockeyManager:componentManager:")]
        string UserNameForHockeyManager(BITHockeyManager hockeyManager, BITHockeyBaseManager componentManager);

        [Export("userEmailForHockeyManager:componentManager:")]
        string UserEmailForHockeyManager(BITHockeyManager hockeyManager, BITHockeyBaseManager componentManager);

		/*
		 * We need to bind all the different *ManagerDelegate-methods again here as Xamarin doesn't pick up
		 * that BITHockeyManagerDelegate includes the other protocols, too.
		 */

		// BITCrashManagerDelegate

		[Export("applicationLogForCrashManager:")]
		string ApplicationLogForCrashManager(BITCrashManager crashManager);

		[Export("attachmentForCrashManager:")]
		BITHockeyAttachment AttachmentForCrashManager(BITCrashManager crashManager);

		[Export("crashManagerWillShowSubmitCrashReportAlert:")]
		void WillShowSubmitCrashReportAlert(BITCrashManager crashManager);

		[Export("crashManagerWillCancelSendingCrashReport:")]
		void WillCancelSendingCrashReport(BITCrashManager crashManager);

		[Export("crashManagerWillSendCrashReportsAlways:")]
		void WillSendCrashReportsAlways(BITCrashManager crashManager);

		[Export("crashManagerWillSendCrashReport:")]
		void WillSendCrashReport(BITCrashManager crashManager);

		[Export("crashManager:didFailWithError:")]
		void DidFailWithError(BITCrashManager crashManager, NSError error);

		[Export("crashManagerDidFinishSendingCrashReport:")]
		void DidFinishSendingCrashReport(BITCrashManager crashManager);

		// @optional -(BOOL)considerAppNotTerminatedCleanlyReportForCrashManager:(BITCrashManager *)crashManager;
		[Export("considerAppNotTerminatedCleanlyReportForCrashManager:")]
		bool ConsiderAppNotTerminatedCleanlyReportForCrashManager(BITCrashManager crashManager);

		// Wrap all other bindings in a macro to make sure they don't get included in crash only build.
		#if !CRASHONLY
			
			// BITUpdateManagerDelegate

			[Export("shouldDisplayUpdateAlertForUpdateManager:forShortVersion:forVersion:")]
			bool ShouldDisplayUpdateAlertForUpdateManager(BITUpdateManager updateManager, NSString shortVersion, NSString version);

			[Export("shouldDisplayExpiryAlertForUpdateManager:")]
			bool ShouldDisplayExpiryAlertForUpdateManager(BITUpdateManager updateManager);

			[Export("didDisplayExpiryAlertForUpdateManager:")]
			void DidDisplayExpiryAlertForUpdateManager(BITUpdateManager updateManager);

			[Export("updateManagerShouldSendUsageData:")]
			bool UpdateManagerShouldSendUsageData(BITUpdateManager updateManager);

			// @optional -(void)updateManagerWillExitApp:(BITUpdateManager *)updateManager;
			[Export("updateManagerWillExitApp:")]
			void UpdateManagerWillExitApp(BITUpdateManager updateManager);

			//(BOOL)willStartDownloadAndUpdate:(BITUpdateManager *)updateManager;
			[Export("willStartDownloadAndUpdate:")]
			bool WillStartDownloadAndUpdate(BITUpdateManager updateManager);
			
			// BITFeedbackManagerDelegate	

			[Export("feedbackManagerDidReceiveNewFeedback:")]
			void DidReceiveNewFeedback(BITFeedbackManager feedbackManager);

			// Added in 3.7.1
			//- (BOOL) allowAutomaticFetchingForNewFeedbackForManager:(BITFeedbackManager *)feedbackManager;
			[Export("allowAutomaticFetchingForNewFeedbackForManager:")]
			bool AllowAutomaticFetching(BITFeedbackManager feedbackManager);

			// @optional -(NSArray * _Nullable)preparedItemsForFeedbackManager:(BITFeedbackManager * _Nonnull)feedbackManager;
			[Export("preparedItemsForFeedbackManager:")]
			NSArray PreparedItemsForFeedbackManager(BITFeedbackManager feedbackManager);

			// Added in 4.1.3
			[Export("forceNewFeedbackThreadForFeedbackManager:")]
			bool ForceNewFeedbackThreadForFeedbackManager(BITFeedbackManager feedbackManager);
			
			// BITAuthenticatorDelegate
			[Export("authenticator:willShowAuthenticationController:")]
			[EventArgs("WillShowAuthenticationController")]
			void WillShowAuthenticationController(BITAuthenticator authenticator, UIViewController viewController);
			
		#endif
    }

    // @interface BITHockeyAttachment : NSObject <NSCoding>
    [BaseType (typeof (NSObject))]
    public partial interface BITHockeyAttachment : INSCoding {

        // -(instancetype)initWithFilename:(NSString *)filename hockeyAttachmentData:(NSData *)hockeyAttachmentData contentType:(NSString *)contentType;
        [Export ("initWithFilename:hockeyAttachmentData:contentType:")]
        IntPtr Constructor (string filename, NSData hockeyAttachmentData, string contentType);

        // @property (readonly, nonatomic, strong) NSString * filename;
        [Export ("filename", ArgumentSemantic.Retain)]
        string Filename { get; }

        // @property (readonly, nonatomic, strong) NSData * hockeyAttachmentData;
        [Export ("hockeyAttachmentData", ArgumentSemantic.Retain)]
        NSData HockeyAttachmentData { get; }

        // @property (readonly, nonatomic, strong) NSString * contentType;
        [Export ("contentType", ArgumentSemantic.Retain)]
        string ContentType { get; }
    }

    #if !CRASHONLY
    // @interface BITFeedbackListViewController : BITHockeyBaseViewController <UITableViewDelegate, UITableViewDataSource, UIActionSheetDelegate, UIAlertViewDelegate, QLPreviewControllerDataSource>
    [BaseType (typeof (BITHockeyBaseViewController))]
    interface BITFeedbackListViewController : IUITableViewDelegate, IUITableViewDataSource, IUIActionSheetDelegate, IUIAlertViewDelegate, IQLPreviewControllerDataSource {

    }
    #endif

    #if !CRASHONLY
    [BaseType(typeof(NSObject))]
    [Model][Protocol]
    public partial interface BITFeedbackComposeViewControllerDelegate
    {
        [Export("feedbackComposeViewController:didFinishWithResult:")]
        void DidFinishWithResult(BITFeedbackComposeViewController viewController, BITFeedbackComposeResult result);

        [Export("feedbackComposeViewControllerDidFinish:")]
        void DidFinish(BITFeedbackComposeViewController viewController);
    }
    #endif

    #if !CRASHONLY
    [BaseType(typeof(NSObject))]
    [Model][Protocol]
    public partial interface BITStoreUpdateManagerDelegate
    {
        [Export("detectedUpdateFromStoreUpdateManager:newVersion:storeURL:")]
        void DetectedUpdateFromStoreUpdateManager(BITStoreUpdateManager storeUpdateManager, string newVersion, NSUrl storeUrl);
    }
    #endif

    #if !CRASHONLY
    // @interface BITFeedbackActivity : UIActivity <BITFeedbackComposeViewControllerDelegate>
    [BaseType (typeof(UIActivity))]
    interface BITFeedbackActivity : BITFeedbackComposeViewControllerDelegate
    {
        // @property (nonatomic, strong) UIImage * customActivityImage;
        [Export ("customActivityImage", ArgumentSemantic.Strong)]
        UIImage CustomActivityImage { get; set; }

        // @property (nonatomic, strong) NSString * customActivityTitle;
        [Export ("customActivityTitle", ArgumentSemantic.Strong)]
        string CustomActivityTitle { get; set; }
    }
    #endif
}
