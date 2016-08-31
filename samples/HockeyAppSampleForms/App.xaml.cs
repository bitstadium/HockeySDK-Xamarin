using Xamarin.Forms;

namespace HockeyAppSampleForms
{
	public partial class App : Application
	{
		public App (params View[] children)
		{
			InitializeComponent ();

			MainPage = new HockeyAppSampleFormsPage (children);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

