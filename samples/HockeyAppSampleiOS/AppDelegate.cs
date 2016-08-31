using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using HockeyApp.iOS;
using UIKit;

namespace HockeyAppSampleiOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        const string HOCKEYAPP_APPID = "YOUR-APP-ID";

        UINavigationController navController;
        HomeViewController homeViewController;
        UIWindow window;

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            //Get the shared instance
            var manager = BITHockeyManager.SharedHockeyManager;

            //Configure it to use our APP_ID
            manager.Configure (HOCKEYAPP_APPID);

            //Start the manager
            manager.StartManager ();

            #if !CRASHONLY
            //Authenticate (there are other authentication options)
            manager.Authenticator.AuthenticateInstallation ();
            #endif

            // create a new window instance based on the screen size
            window = new UIWindow (UIScreen.MainScreen.Bounds);

            homeViewController = new HomeViewController ();
            navController = new UINavigationController (homeViewController);
            window.RootViewController = navController;

            // make the window visible
            window.MakeKeyAndVisible ();

            return true;
        }
    }
}

