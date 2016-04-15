using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;

[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]

namespace HockeyAppSampleAndroid
{
	[Activity (Label = "HockeyApp Sample", MainLauncher = true, Theme="@style/Theme.AppCompat.Light")]
	public class MainActivity : Activity
	{
		public const string HOCKEYAPP_APPID = "YOUR-APP-ID";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Register the crash manager before Initializing the trace writer
			HockeyApp.CrashManager.Register (this, HOCKEYAPP_APPID); 

			//Register to with the Update Manager
			HockeyApp.UpdateManager.Register (this, HOCKEYAPP_APPID);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			FindViewById<Button> (Resource.Id.buttonShowFeedback).Click += delegate {

				//Register with the feedback manager
				HockeyApp.FeedbackManager.Register(this, HOCKEYAPP_APPID, null);

				//Show the feedback screen
				HockeyApp.FeedbackManager.ShowFeedbackActivity(this);
			};

			FindViewById<Button>(Resource.Id.buttonCauseCrash).Click += delegate {
                // Throw a deliberate sample crash
				throw new HockeyAppSampleException("You intentionally caused a crash!");
			};
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			//Start Tracking usage in this activity
			HockeyApp.Tracking.StartUsage (this);
		}

		protected override void OnPause ()
		{
			//Stop Tracking usage in this activity
			HockeyApp.Tracking.StopUsage (this);

			base.OnPause ();
		}
	}

	public class HockeyAppSampleException : System.Exception
	{
		public HockeyAppSampleException(string msg) : base(msg)
		{
		}
	}
}


