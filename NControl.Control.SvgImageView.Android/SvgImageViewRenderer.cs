using System;
using System.ComponentModel;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NGraphics;
using NControl.Controls;
using NControl.Controls.Android;
using Size = NGraphics.Size;

[assembly: ExportRenderer(typeof(SvgImageView), typeof(SvgImageViewRenderer))]
namespace NControl.Controls.Android {
    public class SvgImageViewRenderer : ImageRenderer {
        SvgImageView FormsControl {
            get {
                return Element as SvgImageView;
            }
        }

        static Func<Size, double, IImageCanvas> CreatePlatformImageCanvas = (size, scale) => new AndroidPlatform().CreateImageCanvas(size, scale);

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            if (FormsControl != null)
            {
                const double screenScale = 1.0; // Don't need to deal with screen scaling on Android.

                var finalCanvas = FormsControl.RenderSvgToCanvas(new Size(canvas.Width, canvas.Height), screenScale, CreatePlatformImageCanvas);
                var image = (BitmapImage)finalCanvas.GetImage();

                Control.SetImageBitmap(image.Bitmap);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (FormsControl != null)
            {
                FormsControl.LoadSvgFromResource();
                Invalidate();
            }
        }

        protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);

            if (e.PropertyName == SvgImageView.SvgPathProperty.PropertyName
                || e.PropertyName == SvgImageView.SvgAssemblyProperty.PropertyName) {
                FormsControl.LoadSvgFromResource();
                Invalidate();
            }
            else if (e.PropertyName == SvgImageView.SvgStretchableInsetsProperty.PropertyName) {
                Invalidate();
            }
        }
    }
}
