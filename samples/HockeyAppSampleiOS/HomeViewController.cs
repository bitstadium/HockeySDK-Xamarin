using System;
using MonoTouch.Dialog;
using UIKit;
using HockeyApp.iOS;

namespace HockeyAppSampleiOS
{
    public class HomeViewController : DialogViewController
    {
        public HomeViewController () : base (UITableViewStyle.Plain, new RootElement (""), false)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            var hockey = BITHockeyManager.SharedHockeyManager;
            var authorized = new StringElement("Authorized:", String.Format("{0} ({1})", hockey.Authenticator.Identified, hockey.Authenticator.IdentificationType));

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
                        hockey.Authenticator.AuthenticateInstallation();
                        authorized.Value = String.Format("{0} ({1})", hockey.Authenticator.Identified, hockey.Authenticator.IdentificationType);
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
                        hockey.Authenticator.AuthenticateInstallation();
                        authorized.Value = String.Format("{0} ({1})", hockey.Authenticator.Identified, hockey.Authenticator.IdentificationType);
                    }),
                    new StringElement("Test User Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        hockey.Authenticator.IdentificationType = BITAuthenticatorIdentificationType.HockeyAppUser;
                        hockey.Authenticator.AuthenticateInstallation();
                        authorized.Value = String.Format("{0} ({1})", hockey.Authenticator.Identified, hockey.Authenticator.IdentificationType);
                    }),
                    new StringElement("Test Web Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        hockey.Authenticator.IdentificationType = BITAuthenticatorIdentificationType.WebAuth;
                        hockey.Authenticator.AuthenticateInstallation();
                    }),
                    new StringElement("Reset Auth", () => {
                        hockey.Authenticator.CleanupInternalStorage();
                        authorized.Value = String.Format("{0} ({1})", hockey.Authenticator.Identified, "None");
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
    }

    public class HockeyAppSampleException : System.Exception
    {
        public HockeyAppSampleException (string msg) : base (msg)
        {
        }
    }
}

