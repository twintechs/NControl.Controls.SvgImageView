using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using NGraphics;
using NControl.Controls;
using NControl.Controls.iOS;
using Size = NGraphics.Size;

[assembly: ExportRenderer(typeof(SvgImageView), typeof(SvgImageViewRenderer))]
namespace NControl.Controls.iOS {
    public class SvgImageViewRenderer : ImageRenderer {
        SvgImageView FormsControl {
            get {
                return Element as SvgImageView;
            }
        }

        static Func<Size, double, IImageCanvas> CreatePlatformImageCanvas = (size, scale) => new ApplePlatform().CreateImageCanvas(size, scale);

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            if (FormsControl != null)
            {
                using (CGContext context = UIGraphics.GetCurrentContext ()) 
                {
                    context.SetAllowsAntialiasing(true);
                    context.SetShouldAntialias(true);
                    context.SetShouldSmoothFonts(true);

                    var finalCanvas = FormsControl.RenderSvgToCanvas(new Size(rect.Width, rect.Height), UIScreen.MainScreen.Scale, CreatePlatformImageCanvas);
                    var image = finalCanvas.GetImage();
                    var uiImage = image.GetUIImage();
                    Control.Image = uiImage;
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (FormsControl != null)
            {
                FormsControl.LoadSvgFromResource();
                SetNeedsDisplay();
            }
        }

        protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);

            if (e.PropertyName == SvgImageView.SvgPathProperty.PropertyName
                || e.PropertyName == SvgImageView.SvgAssemblyProperty.PropertyName) {
                FormsControl.LoadSvgFromResource();
                SetNeedsDisplay();
            }
            else if (e.PropertyName == SvgImageView.SvgStretchableInsetsProperty.PropertyName) {
                SetNeedsDisplay();
            }
        }
    }
}
