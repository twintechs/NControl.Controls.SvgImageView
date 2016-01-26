using Foundation;
using UIKit;
using NControl.Controls.iOS;

namespace SvgImageView_Sample.iOS {
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());
            SvgImageViewRenderer.Init();

            return base.FinishedLaunching(app, options);
        }
    }

    public class Application {
        static void Main(string[] args) {
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
