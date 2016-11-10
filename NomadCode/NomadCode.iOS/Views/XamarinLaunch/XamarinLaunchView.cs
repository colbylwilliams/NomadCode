using System;
using System.Threading.Tasks;

using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;


namespace NomadCode.iOS
{
	public class XamarinLaunchView : UIView
	{
		UIColor bgColor;

		UIColor xamarinBlue = UIColor.FromRGBA(33f / 255f, 127f / 255f, 187f / 255f, 255f / 255f);

		CAShapeLayer bgLayer, logoLayer;

		TaskCompletionSource<bool> animateTcs;


		static NSNumber nsNumberZero = NSNumber.FromNFloat(0.0f);

		static NSNumber nsNumberOne = NSNumber.FromNFloat(1.0f);


		public double TotalDuration { get; set; } = 1;

		public UIColor LogoColor { get; set; } = UIColor.White;


		public XamarinLaunchView() { }

		public XamarinLaunchView(IntPtr handle) : base(handle) { }


		public override bool ClearsContextBeforeDrawing
		{
			get { return true; }
			set { base.ClearsContextBeforeDrawing = value; }
		}


		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			bgColor = bgColor.NotGonnaWork() ? BackgroundColor.NotGonnaWork() ? xamarinBlue : BackgroundColor : bgColor;

			BackgroundColor = UIColor.Clear;


			if (bgLayer == null)
			{
				var layer = new CAShapeLayer
				{
					Path = UIBezierPath.FromRect(Bounds).CGPath,
					FillColor = bgColor.CGColor,
					Frame = Bounds
				};

				Layer.AddSublayer(layer);

				bgLayer = layer;
			}


			if (logoLayer == null)
			{
				var baseLayer = new CAShapeLayer
				{
					Path = getMaskPath().CGPath,
					FillColor = bgColor.CGColor,
					StrokeColor = LogoColor.CGColor,
					FillRule = CAShapeLayer.FillRuleEvenOdd,
					Frame = Bounds,
					LineWidth = 0
				};


				var shapeLayer = new CAShapeLayer
				{
					Path = getLogoPath().CGPath,
					FillColor = LogoColor.CGColor,
					StrokeColor = LogoColor.CGColor,
					Frame = new CGRect(Bounds.GetMidX() - 45, Bounds.GetMidY() - 40, 90, 80),
					LineWidth = 0
				};


				baseLayer.AddSublayer(shapeLayer);

				Layer.AddSublayer(baseLayer);

				logoLayer = baseLayer;
			}
		}


		public Task<bool> AnimateTransitionAsync()
		{
			if (!animateTcs.IsNullFinishCanceledOrFaulted()) return animateTcs.Task;

			animateTcs = new TaskCompletionSource<bool>();

			if (logoLayer != null)
			{

				var scaleKeyFrameAnimation = CAKeyFrameAnimation.FromKeyPath(@"transform.scale");

				scaleKeyFrameAnimation.Values = new NSObject[] {
					NSValue.FromCATransform3D(CATransform3D.MakeScale(1, 1, 1)),
					NSValue.FromCATransform3D(CATransform3D.MakeScale(0.94f, 0.94f, 0.94f)),
					NSValue.FromCATransform3D(CATransform3D.MakeScale(40, 40, 40))
				};


				scaleKeyFrameAnimation.KeyTimes = new NSNumber[] {
					nsNumberZero,
					NSNumber.FromNFloat(0.5f),
					nsNumberOne
				};


				scaleKeyFrameAnimation.TimingFunctions = new CAMediaTimingFunction[] {
					CAMediaTimingFunction.FromName(CAMediaTimingFunction.Default),
					CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut),
					CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseIn)
				};

				scaleKeyFrameAnimation.Duration = TotalDuration;


				var layerOpacityAnimation = CABasicAnimation.FromKeyPath(@"opacity");

				layerOpacityAnimation.BeginTime = CAAnimation.CurrentMediaTime() + (TotalDuration * 0.7);
				layerOpacityAnimation.Duration = (TotalDuration * 0.1);


				var bgOpacityAnimation = CABasicAnimation.FromKeyPath(@"opacity");

				bgOpacityAnimation.BeginTime = CAAnimation.CurrentMediaTime() + (TotalDuration * 0.55);
				bgOpacityAnimation.Duration = (TotalDuration * 0.20);


				layerOpacityAnimation.From = bgOpacityAnimation.From = nsNumberOne;
				layerOpacityAnimation.To = bgOpacityAnimation.To = nsNumberZero;


				scaleKeyFrameAnimation.FillMode = layerOpacityAnimation.FillMode = bgOpacityAnimation.FillMode = CAFillMode.Forwards;
				scaleKeyFrameAnimation.RemovedOnCompletion = layerOpacityAnimation.RemovedOnCompletion = bgOpacityAnimation.RemovedOnCompletion = false;


				scaleKeyFrameAnimation.AnimationStopped += handlePathAnimationStopped;


				bgLayer.AddAnimation(bgOpacityAnimation, @"opacity");

				Layer.AddAnimation(layerOpacityAnimation, @"opacity");

				logoLayer.AddAnimation(scaleKeyFrameAnimation, @"transform.scale");
			}

			return animateTcs.Task;
		}


		void handlePathAnimationStopped(object sender, CAAnimationStateEventArgs e)
		{
			var animation = sender as CAKeyFrameAnimation;

			if (animation != null)
			{
				animation.AnimationStopped -= handlePathAnimationStopped;
			}

			if (!animateTcs.TrySetResult(e.Finished))
			{
				System.Diagnostics.Debug.WriteLine("[XamarinLaunchView] Failed to set TaskCompletionSource result for the CoreAnimations");
			}

			RemoveFromSuperview();
		}


		#region getLogoPath

		UIBezierPath getLogoPath()
		{
			var path = UIBezierPath.Create();

			path.MoveTo(new CGPoint(25.9686283, 0));

			path.AddCurveToPoint(new CGPoint(19.9348989, 3.48635520000),
								 new CGPoint(23.5379212, 0.00514944604),
								 new CGPoint(21.1585327, 1.38355316000));

			path.AddLineTo(new CGPoint(0.903434221, 36.5136319));

			path.AddCurveToPoint(new CGPoint(0.903434221, 43.4863681),
								 new CGPoint(-0.30114474, 38.6220640),
								 new CGPoint(-0.30114474, 41.3779360));

			path.AddLineTo(new CGPoint(19.9348989, 76.5136448));

			path.AddCurveToPoint(new CGPoint(25.9686283, 80.0000000),
								 new CGPoint(21.1586613, 78.6164640),
								 new CGPoint(23.5379212, 79.9952368));

			path.AddLineTo(new CGPoint(64.0314232, 80));

			path.AddCurveToPoint(new CGPoint(70.0651517, 76.5136448),
								 new CGPoint(66.4621303, 79.9948506),
								 new CGPoint(68.8413894, 78.6164640));

			path.AddLineTo(new CGPoint(89.096613, 43.4863681));

			path.AddCurveToPoint(new CGPoint(89.096613, 36.5136319),
								 new CGPoint(90.3011911, 41.377936),
								 new CGPoint(90.3010668, 38.622064));

			path.AddLineTo(new CGPoint(70.0651517, 3.4863552));

			path.AddCurveToPoint(new CGPoint(64.0314232, 0.00000000000),
								 new CGPoint(68.8413894, 1.38355316000),
								 new CGPoint(66.4621303, 0.00476323759));

			path.AddLineTo(new CGPoint(25.9686283, 0));

			path.ClosePath();


			path.UsesEvenOddFillRule = true;


			path.MoveTo(new CGPoint(26.3140501, 19.3213567));

			path.AddCurveToPoint(new CGPoint(26.4736292, 19.3213567),
								 new CGPoint(26.3663374, 19.3162072),
								 new CGPoint(26.4211791, 19.3162072));

			path.AddLineTo(new CGPoint(33.0388888, 19.3213567));

			path.AddCurveToPoint(new CGPoint(33.7565275, 19.7471643),
								 new CGPoint(33.3295033, 19.3273643),
								 new CGPoint(33.6114003, 19.4951505));

			path.AddLineTo(new CGPoint(44.8937013, 39.574188));

			path.AddCurveToPoint(new CGPoint(44.9995873, 39.8935395),
								 new CGPoint(44.9497601, 39.6722163),
								 new CGPoint(44.9858812, 39.7814361));

			path.AddCurveToPoint(new CGPoint(45.1054734, 39.5741880),
								 new CGPoint(45.0133020, 39.7814103),
								 new CGPoint(45.0493888, 39.6721820));

			path.AddLineTo(new CGPoint(56.2159909, 19.7471643));

			path.AddCurveToPoint(new CGPoint(56.9601641, 19.3213567),
								 new CGPoint(56.3657229, 19.4875121),
								 new CGPoint(56.6608254, 19.3187991));

			path.AddLineTo(new CGPoint(63.5254263, 19.3213567));

			path.AddCurveToPoint(new CGPoint(64.2697229, 20.5721914),
								 new CGPoint(64.1067872, 19.3265061),
								 new CGPoint(64.5421542, 20.0580278));

			path.AddLineTo(new CGPoint(53.3984993, 40.0000043));

			path.AddLineTo(new CGPoint(64.2697229, 59.4012031));

			path.AddCurveToPoint(new CGPoint(63.5254263, 60.6786433),
								 new CGPoint(64.5679404, 59.9183191),
								 new CGPoint(64.1218605, 60.6837585));

			path.AddLineTo(new CGPoint(56.9601641, 60.6786433));

			path.AddCurveToPoint(new CGPoint(56.2159909, 60.2262216),
								 new CGPoint(56.6555941, 60.6760686),
								 new CGPoint(56.3587472, 60.4956835));

			path.AddLineTo(new CGPoint(45.1054734, 40.3991979));

			path.AddCurveToPoint(new CGPoint(44.9995873, 40.0798465),
								 new CGPoint(45.0494145, 40.3011696),
								 new CGPoint(45.0132935, 40.1919413));

			path.AddCurveToPoint(new CGPoint(44.8937013, 40.3991979),
								 new CGPoint(44.9858726, 40.1919671),
								 new CGPoint(44.9497858, 40.3012126));

			path.AddLineTo(new CGPoint(33.7565275, 60.2262216));

			path.AddCurveToPoint(new CGPoint(33.0388888, 60.6786433),
								 new CGPoint(33.6180090, 60.4880624),
								 new CGPoint(33.3346120, 60.6667224));

			path.AddLineTo(new CGPoint(26.4736292, 60.6786433));

			path.AddCurveToPoint(new CGPoint(25.7294526, 59.4012031),
								 new CGPoint(25.8771942, 60.6837928),
								 new CGPoint(25.4312351, 59.9183191));

			path.AddLineTo(new CGPoint(36.6006727, 40.0000043));

			path.AddLineTo(new CGPoint(25.7294526, 20.5721914));

			path.AddCurveToPoint(new CGPoint(26.3140501, 19.3213567),
								 new CGPoint(25.4688562, 20.1030769),
								 new CGPoint(25.7867972, 19.4214876));

			path.AddLineTo(new CGPoint(26.3140501, 19.3213567));

			path.ClosePath();


			return path;
		}

		#endregion


		#region getMaskPath

		UIBezierPath getMaskPath()
		{
			var path = UIBezierPath.FromRect(Bounds);

			path.UsesEvenOddFillRule = true;

			path.MoveTo(new CGPoint(24.8929951, 18.0041782).Fix());

			path.AddLineTo(new CGPoint(32.0343805, 18.0041782).Fix());

			path.AddCurveToPoint(new CGPoint(34.4199219, 18.5986328).Fix(),
								 new CGPoint(32.3504974, 18.0106776).Fix(),
								 new CGPoint(34.2620592, 18.3259925).Fix());

			path.AddLineTo(new CGPoint(44.9294964, 37.2226562).Fix());

			path.AddCurveToPoint(new CGPoint(55.6556095, 18.0041782).Fix(),
								 new CGPoint(44.9904746, 37.3287078).Fix(),
								 new CGPoint(55.6556095, 18.0041782).Fix());

			path.AddCurveToPoint(new CGPoint(58.0548359, 18.0041782).Fix(),
								 new CGPoint(55.8184810, 17.7232744).Fix(),
								 new CGPoint(57.7292291, 18.0014113).Fix());

			path.AddLineTo(new CGPoint(65.1962241, 18.0041782).Fix());

			path.AddCurveToPoint(new CGPoint(66.0058354, 20.2304688).Fix(),
								 new CGPoint(65.8286015, 18.0097491).Fix(),
								 new CGPoint(66.3021736, 19.6742228).Fix());

			path.AddLineTo(new CGPoint(54.777306, 40.2601173).Fix());

			path.AddLineTo(new CGPoint(66.0058354, 60.1386719).Fix());

			path.AddCurveToPoint(new CGPoint(65.1962241, 62.7464041).Fix(),
								 new CGPoint(66.3302226, 60.6981118).Fix(),
								 new CGPoint(65.8449975, 62.7519379).Fix());

			path.AddLineTo(new CGPoint(58.0548359, 62.7464041).Fix());

			path.AddCurveToPoint(new CGPoint(56.2165631, 62.1015625).Fix(),
								 new CGPoint(57.7235387, 62.7436187).Fix(),
								 new CGPoint(56.3718467, 62.3930789).Fix());

			path.AddCurveToPoint(new CGPoint(44.9294964, 42.5458984).Fix(),
								 new CGPoint(56.2165631, 62.1015625).Fix(),
								 new CGPoint(44.9905026, 42.4398933).Fix());

			path.AddLineTo(new CGPoint(33.7359255, 62.7464041).Fix());

			path.AddCurveToPoint(new CGPoint(32.0343805, 62.7464041).Fix(),
								 new CGPoint(33.5852515, 63.0296756).Fix(),
								 new CGPoint(32.3560544, 62.7335075).Fix());

			path.AddLineTo(new CGPoint(24.8929951, 62.7464041).Fix());

			path.AddCurveToPoint(new CGPoint(24.0835142, 60.1386719).Fix(),
								 new CGPoint(24.2442207, 62.7519750).Fix(),
								 new CGPoint(23.7591270, 60.6981118).Fix());

			path.AddLineTo(new CGPoint(35.105957, 40.2601173).Fix());

			path.AddLineTo(new CGPoint(24.0835142, 19.7089844).Fix());

			path.AddCurveToPoint(new CGPoint(24.7194124, 18.0041782).Fix(),
								 new CGPoint(23.8000496, 19.2014746).Fix(),
								 new CGPoint(24.1458910, 18.1125045).Fix());

			path.AddCurveToPoint(new CGPoint(24.8929951, 18.0041782).Fix(),
								 new CGPoint(24.7762880, 17.9986073).Fix(),
								 new CGPoint(24.8359422, 17.9986073).Fix());

			path.ClosePath();

			return path;
		}

		#endregion
	}


	public static class XamarinLaunchViewExtensions
	{
		static CGPoint pointOffset => new CGPoint(UIScreen.MainScreen.Bounds.GetMidX() - 45, UIScreen.MainScreen.Bounds.GetMidY() - 40);


		public static CGPoint Fix(this CGPoint point) => new CGPoint(point.X + pointOffset.X, point.Y + pointOffset.Y);


		public static bool NotGonnaWork(this UIColor color) => (color == null || color == UIColor.Clear || color == UIColor.White);


		/* TaskStatus enum
		 * Created, = 0
		 * WaitingForActivation, = 1
		 * WaitingToRun, = 2
		 * Running, = 3
		 * WaitingForChildrenToComplete, = 4
		 * RanToCompletion, = 5
		 * Canceled, = 6
		 * Faulted = 7  */
		public static bool IsNullFinishCanceledOrFaulted<T>(this TaskCompletionSource<T> tcs) => (tcs == null || (int)tcs.Task.Status >= 5);
	}
}
