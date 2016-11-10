using System;

namespace NomadCode.iOS
{
	public class ExampleViewController : UIViewController
	{
		XamarinLaunchView logoView;


		public ExampleViewController() { }


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			setupLaunchView();
		}


		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			refreshData();
		}


		void setupLaunchView()
		{
			logoView = new XamarinLaunchView();

			logoView.Frame = NavigationController.View.Frame;
			logoView.LogoColor = UIColor.White;

			NavigationController.View.AddSubview(logoView);
		}


		async Task removeLaunchViewAsync()
		{
			if (!logoView.IsDescendantOfView(NavigationController?.View))
				return;

			await logoView.AnimateTransitionAsync();

			// to change the status bar color after
			SetNeedsStatusBarAppearanceUpdate();
		}


		void refreshData()
		{
			if (data == null)
			{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

				Task.Run(async () =>
				{
					await DataClient.GetData();

					BeginInvokeOnMainThread(async () =>
					{
						UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

						await removeLaunchViewAsync();
					});
				});
			}
		}
	}
}