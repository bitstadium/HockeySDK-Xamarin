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
            HockeyApp.HockeyManager.SharedHockeyManager.Configure ("d25ad82136f447d78dfda2b9ab899cbf");

            HockeyApp.HockeyManager.SharedHockeyManager.StartManager ();
        }

        public override void WillTerminate (NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}

