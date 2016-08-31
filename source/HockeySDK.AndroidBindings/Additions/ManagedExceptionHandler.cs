using System;
using Android.App;

namespace HockeyApp
{
	public class ManagedExceptionHandler
	{
		public static void SaveException(Exception ex)
		{
			Save (ex.ToString ());
		}

		public static void SaveException(object exception)
		{
			Save (exception.ToString ());
		}

		static void Save(string message)
		{
			Application.SynchronizationContext.Send(new System.Threading.SendOrPostCallback(state => { 			
				HockeyApp.ExceptionHandler.SaveException(new Java.Lang.Throwable(message), null);
			}), null);
		}
	}
}

