using System;
using MonoTouch.Dialog;
using UIKit;
using HockeyApp.iOS;

namespace HockeyAppSampleiOS
{
    public class HomeViewController : DialogViewController
    {
        readonly BITHockeyManager hockey = BITHockeyManager.SharedHockeyManager;
        StringElement authorized;

        public HomeViewController () : base (UITableViewStyle.Plain, new RootElement (""), false)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            authorized = new StringElement("Authorized:", String.Format("{0} ({1})", hockey.Authenticator.Identified, hockey.Authenticator.IdentificationType));

            Root = new RootElement ("HockeyApp Sample") {
                #if !CRASHONLY
                new Section {
                    new StringElement("Check for Updates", () => {
                        hockey.UpdateManager.CheckForUpdate();
                    }),
                    new StringElement("Show Feedback", () => {
                        hockey.FeedbackManager.ShowFeedbackListView();
                    }),
                    new StringElement("Submit New Feedback", () => {
                        hockey.FeedbackManager.ShowFeedbackComposeView();
                    }),
                    new StringElement("Track Event", () => {
                        hockey.MetricsManager.TrackEvent("My Sample Event");
                    }),

                    new StringElement("Crashed Last Run:", hockey.CrashManager.DidCrashInLastSession.ToString())
                },
                new Section {
                    authorized,
                    new StringElement("Test Anonymous Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        hockey.Authenticator.IdentificationType = BITAuthenticatorIdentificationType.Anonymous;
                        hockey.Authenticator.IdentifyWithCompletion(HandleBITAuthenticatorIdentifyCallback);
                    }),
                    new StringElement("Test Device Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        hockey.Authenticator.IdentificationType = BITAuthenticatorIdentificationType.Device;
                        hockey.Authenticator.AuthenticateInstallation();
                    }),
                    new StringElement("Test Email Address Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        hockey.Authenticator.AuthenticationSecret = "YOUR-APP-SECRET";
                        hockey.Authenticator.IdentificationType = BITAuthenticatorIdentificationType.HockeyAppEmail;
                        hockey.Authenticator.IdentifyWithCompletion(HandleBITAuthenticatorIdentifyCallback);
                    }),
                    new StringElement("Test User Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        hockey.Authenticator.IdentificationType = BITAuthenticatorIdentificationType.HockeyAppUser;
                        hockey.Authenticator.IdentifyWithCompletion(HandleBITAuthenticatorIdentifyCallback);
                    }),
                    new StringElement("Test Web Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        hockey.Authenticator.IdentificationType = BITAuthenticatorIdentificationType.WebAuth;
                        hockey.Authenticator.AuthenticateInstallation();
                    }),
                    new StringElement("Reset Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        authorized.Value = String.Format("{0} ({1})", hockey.Authenticator.Identified, "None");
                        authorized.GetContainerTableView().ReloadData();
                    })
                },
                #endif
                new Section {
                    new StringElement ("Throw Managed .NET Exception", () => {

                        throw new HockeyAppSampleException ("You intentionally caused a crash!");

                    }),

                    new StringElement ("Throw NSException", () => {

                        var storyboard = UIStoryboard.FromName("Main", null);
                        var vc = storyboard.InstantiateViewController("SomeViewControllerWithNoStoryboardID");

                    })
                }
            };
        }

        public void HandleBITAuthenticatorIdentifyCallback(bool identified, Foundation.NSError error)
        {
            if (authorized != null)
            {
                // Execute operation in UI thread
                BeginInvokeOnMainThread(() =>
                {
                    authorized.Value = String.Format("{0} ({1})", hockey.Authenticator.Identified, hockey.Authenticator.IdentificationType);
                    authorized.GetContainerTableView().ReloadData();
                });
            }
        }

    }

    public class HockeyAppSampleException : System.Exception
    {
        public HockeyAppSampleException (string msg) : base (msg)
        {
        }
    }
}

