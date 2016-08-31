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
                    new StringElement("Crashed Last Run:", hockey.CrashManager.DidCrashInLastSession.ToString())
                },
                #endif
                new Section {
                    new StringElement ("Throw Managed .NET Exception", () => {

                        throw new HockeyAppSampleException ("You intentionally caused a crash!");

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

