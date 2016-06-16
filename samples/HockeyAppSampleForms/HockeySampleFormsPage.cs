using System;
using System.ComponentModel;
using HockeyApp;
using Xamarin.Forms;

namespace HockeyAppSampleForms
{
	public class HockeyAppSampleFormsPage : ContentPage
	{
		public HockeyAppSampleFormsPage (params View [] children)
		{
			StackLayout ContentLayout;

			var pclFeaturesLabel = new Label {
				Text = "PCL Supported Features",
				HorizontalOptions = LayoutOptions.Center
			};

			var didCrashLabel = new Label {
				Text = string.Format ("Did Crash in Last Session: {0}", CrashManager.DidCrashInLastSession),
			};

			var metricsManagerDisabledFormat = "MetricsManager Disabled: {0}";
			var metricsManagerDisabledButton = new Button {
				Text = string.Format (metricsManagerDisabledFormat, MetricsManager.Disabled)
			};
			metricsManagerDisabledButton.Clicked += (sender, e) => {
				metricsManagerDisabledButton.Text = string.Format (metricsManagerDisabledFormat, MetricsManager.Disabled);
			};

			var toggleMetricsManagerButton = new Button {
				Text = "Toggle MetricsManager"
			};
			toggleMetricsManagerButton.Clicked += (sender, e) => {
				MetricsManager.Disabled = !MetricsManager.Disabled;
			};

			var trackClickEventButton = new Button {
				Text = "Track Event"
			};
			trackClickEventButton.Clicked += (sender, e) => {
				MetricsManager.TrackEvent ("Track_Event_Clicked");
			};

			ContentLayout = new StackLayout {
				Children = {
					pclFeaturesLabel,
					didCrashLabel,
					metricsManagerDisabledButton,
					toggleMetricsManagerButton,
					trackClickEventButton
				}
			};

			ContentLayout.Children.Add (new Label {
				Text = "Platform Specific Features",
				HorizontalOptions = LayoutOptions.Center
			});

			foreach (var child in children) ContentLayout.Children.Add (child);

			Content = new ScrollView {
				Content = ContentLayout,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
		}
	}
}

