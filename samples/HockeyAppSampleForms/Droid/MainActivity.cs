using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using HockeyApp.Android.Utils;

namespace HockeyAppSampleForms.Droid
{
	[Activity (Label = "HockeyAppSampleForms.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		const string HOCKEYAPP_APPID = "YOUR-APP-ID";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			HockeyLog.LogLevel = 3;

			// Register the crash manager before Initializing the trace writer
			CrashManager.Register (this, HOCKEYAPP_APPID);

			//Register to with the Update Manager
			UpdateManager.Register (this, HOCKEYAPP_APPID);

			MetricsManager.Register (Application, HOCKEYAPP_APPID);

			var showFeedbackButton = new Xamarin.Forms.Button {
				Text = "Show Feedback"
			};
			showFeedbackButton.Clicked += (sender, e) => {
				//Register with the feedback manager
				FeedbackManager.Register (this, HOCKEYAPP_APPID, null);

				//Show the feedback screen
				FeedbackManager.ShowFeedbackActivity (this);
			};

			var causeCrashButton = new Xamarin.Forms.Button {
				Text = "Cause Crash"
			};
			causeCrashButton.Clicked += (sender, e) => {
				// Throw a deliberate sample crash
				throw new HockeyAppSampleException ("You intentionally caused a crash!");
			};

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App (showFeedbackButton, causeCrashButton));
		}
	}
}

