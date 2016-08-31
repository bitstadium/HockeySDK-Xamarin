using System;
using HockeyAndroid = HockeyApp.Android;

namespace HockeyApp
{
	internal class PlatformCrashManagerListener : HockeyAndroid.CrashManagerListener
	{
		public string PlatformContact { get; set; } = null;
		public override string Contact { get { return PlatformContact ?? base.Contact; } }

		public string PlatformDescription { get; set; } = null;
		public override string Description { get { return PlatformDescription ?? base.Description; } }

		public int? PlatformMaxRetryAttempts { get; set; } = null;
		public override int MaxRetryAttempts { get { return PlatformMaxRetryAttempts ?? base.MaxRetryAttempts; } }

		public string PlatformUserID { get; set; } = null;
		public override string UserID { get { return PlatformUserID ?? UserID; } }

		public bool? PlatformIgnoreDefaultHandler { get; set; } = null;
		public override bool IgnoreDefaultHandler()
		{
			return null != PlatformIgnoreDefaultHandler ? PlatformIgnoreDefaultHandler.Value : base.IgnoreDefaultHandler();
		}

		public bool? PlatformIncludeDeviceData { get; set; } = null;
		public override bool IncludeDeviceData()
		{
			return null != PlatformIncludeDeviceData ? PlatformIncludeDeviceData.Value : base.IncludeDeviceData();
		}

		public bool? PlatformIncludeDeviceIdentifier { get; set; } = null;
		public override bool IncludeDeviceIdentifier()
		{
			return null != PlatformIncludeDeviceIdentifier ? PlatformIncludeDeviceIdentifier.Value : base.IncludeDeviceIdentifier();
		}

		public bool? PlatformIncludeThreadDetails { get; set; } = null;
		public override bool IncludeThreadDetails()
		{
			return null != PlatformIncludeThreadDetails ? PlatformIncludeThreadDetails.Value : base.IncludeThreadDetails();
		}

		public Action PlatformOnConfirmedCrashesFound { get; set; } = null;
		public override void OnConfirmedCrashesFound()
		{
			if (null != PlatformOnConfirmedCrashesFound) PlatformOnConfirmedCrashesFound();
			else base.OnConfirmedCrashesFound();
		}

		public Action PlatformOnCrashesNotSent { get; set; } = null;
		public override void OnCrashesNotSent()
		{
			if (null != PlatformOnCrashesNotSent) PlatformOnCrashesNotSent();
			else base.OnCrashesNotSent();
		}

		public Action PlatformOnCrashesSent { get; set; } = null;
		public override void OnCrashesSent()
		{
			if (null != PlatformOnCrashesSent) PlatformOnCrashesSent();
			else base.OnCrashesSent();
		}

		public delegate bool HandleAlertView();
		public HandleAlertView PlatformOnHandleAlertView { get; set; } = null;
		public override bool OnHandleAlertView()
		{
			return null != PlatformOnHandleAlertView ? PlatformOnHandleAlertView() : base.OnHandleAlertView();
		}

		public Action PlatformOnNewCrashesFound { get; set; } = null;
		public override void OnNewCrashesFound()
		{
			if (null != PlatformOnNewCrashesFound) PlatformOnNewCrashesFound();
			else base.OnNewCrashesFound();
		}

		public Action PlatformOnUserDeniedCrashes { get; set; } = null;
		public override void OnUserDeniedCrashes()
		{
			if (null != PlatformOnUserDeniedCrashes) PlatformOnUserDeniedCrashes();
			else base.OnUserDeniedCrashes();
		}

		public delegate bool HandleShouldAutoUploadCrashes();
		public HandleShouldAutoUploadCrashes PlatformShouldAutoUploadCrashes { get; set; } = null;
		public override bool ShouldAutoUploadCrashes()
		{
			return null != PlatformShouldAutoUploadCrashes ? PlatformShouldAutoUploadCrashes() : base.ShouldAutoUploadCrashes();
		}

		public PlatformCrashManagerListener() {}
	}
}

