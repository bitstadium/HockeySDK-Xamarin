using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using HockeyApp.iOS;
using UIKit;

namespace HockeyAppSampleForms.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		const string HOCKEYAPP_APPID = "YOUR-APP-ID";

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//Get the shared instance
			var manager = BITHockeyManager.SharedHockeyManager;

			//Configure it to use our APP_ID
			manager.Configure (HOCKEYAPP_APPID);

			manager.LogLevel = BITLogLevel.Debug;

			//Start the manager
			manager.StartManager ();

			//Authenticate (there are other authentication options)
			manager.Authenticator.AuthenticateInstallation ();

			var checkForUpdatesButton = new Xamarin.Forms.Button {
				Text = "Check for Updates"
			};
			checkForUpdatesButton.Clicked += (sender, e) => {
				manager.UpdateManager.CheckForUpdate ();
			};

			var showFeedbackButton = new Xamarin.Forms.Button {
				Text = "Show Feedback"
			};
			showFeedbackButton.Clicked += (sender, e) => {
				manager.FeedbackManager.ShowFeedbackListView ();
			};

			var submitNewFeedbackButton = new Xamarin.Forms.Button {
				Text = "Submit New Feedback"
			};
			submitNewFeedbackButton.Clicked += (sender, e) => {
				manager.FeedbackManager.ShowFeedbackComposeView ();
			};

			var throwExceptionButton = new Xamarin.Forms.Button {
				Text = "Throw Managed .NET Exception"
			};
			throwExceptionButton.Clicked += (sender, e) => {
				throw new HockeyAppSampleException ("You intentionally caused a crash!");
			};

			global::Xamarin.Forms.Forms.Init ();

			LoadApplication (new App (checkForUpdatesButton, showFeedbackButton, submitNewFeedbackButton, throwExceptionButton));

			return base.FinishedLaunching (app, options);
		}
	}
}

