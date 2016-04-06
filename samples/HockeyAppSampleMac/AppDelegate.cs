using AppKit;
using Foundation;

namespace HockeyAppSampleMac
{
    public class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate ()
        {
        }

        public override void DidFinishLaunching (NSNotification notification)
        {
            // Insert code here to initialize your application
            HockeyApp.HockeyManager.SharedHockeyManager.Configure ("YOUR-APP-KEY");

            HockeyApp.HockeyManager.SharedHockeyManager.StartManager ();
        }

        public override void WillTerminate (NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}

