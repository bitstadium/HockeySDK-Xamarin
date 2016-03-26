using System;

using AppKit;
using Foundation;

namespace HockeyAppSampleMac
{
    public partial class ViewController : NSViewController
    {
        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            // Do any additional setup after loading the view.
            buttonCrash.Activated += delegate {
                HockeyApp.HockeyManager.SharedHockeyManager.CrashManager.GenerateTestCrash ();
            };


        }

        public override NSObject RepresentedObject {
            get {
                return base.RepresentedObject;
            }
            set {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
