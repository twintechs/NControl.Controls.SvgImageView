using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using NControl.Controls;

namespace SvgImageView_Sample {
    public class TestModel : INotifyPropertyChanged {
        int _AllSidesInset;
        public int AllSidesInset {
            get { return _AllSidesInset; }
            set {
                if (value != _AllSidesInset) {
                    _AllSidesInset = value;
                    _SvgInsets = new ResizableSvgInsets(AllSidesInset);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllSidesInset)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SvgInsets)));
                }
            }
        }
        ResizableSvgInsets _SvgInsets;
        public ResizableSvgInsets SvgInsets {
            get {
                return _SvgInsets;
            }
        }
        int _SvgResourceIndex = 0;
        public int SvgResourceIndex {
            get { return _SvgResourceIndex; }
            set {
                if (value != _SvgResourceIndex) {
                    _SvgResourceIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SvgResourceIndex)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SvgResourcePath)));
                }
            }
        }
        public string SvgResourcePath {
            get {
                return AvailableResources[_SvgResourceIndex].Value;
            }
        }
        static public string[] AvailableResourceNames {
            get {
                return AvailableResources.Select(kvp => kvp.Key).ToArray();
            }
        }

        // Since PCLs don't get `Assembly.GetCallingAssembly`, we're doing this manually.
        public static readonly List<KeyValuePair<string, string>> AvailableResources = new[] {
            "twintechs-logo",
            "test-button",
            "ErulisseuiinSpaceshipPack",
            "MocastIcon",
            "repeat",
            "sliderThumb",
            "Smile",
            "SunAtNight",
            "TextVariations",
            "mozilla.BezierCurves1",
            "mozilla.BezierCurves2",
            "mozilla.ellipse",
            "mozilla.path",
            "mozilla.Text1",
            "mozilla.Text2",
            "mozilla.Text3",
            "mozilla.Text4",
            "mozilla.transform",
        }.ToDictionary(name => name, name => $"SvgImageView_Sample.Assets.{name}.svg").ToList();

        public TestModel() {
            AllSidesInset = 0;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
    }
    public class App : Application {
        readonly TestModel _ViewModel = new TestModel();
        public App() {
            var resourcePicker = new Picker() {
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            foreach (var resourceName in TestModel.AvailableResourceNames) {
                resourcePicker.Items.Add(resourceName);
            }
            resourcePicker.SetBinding(Picker.SelectedIndexProperty, nameof(TestModel.SvgResourceIndex), BindingMode.TwoWay);
            var insetSlider = new Slider() {
                Minimum = 0,
                Maximum = 35,
                Value = _ViewModel.AllSidesInset,
            };
            insetSlider.SetBinding(Slider.ValueProperty, nameof(TestModel.AllSidesInset), BindingMode.TwoWay);
            var slicingSvg = new SvgImageView() {
                SvgAssembly = typeof(App).GetTypeInfo().Assembly,
                SvgPath = _ViewModel.SvgResourcePath,
                WidthRequest = 300,
                HeightRequest = 300,
            };
            slicingSvg.SetBinding(SvgImageView.SvgStretchableInsetsProperty, nameof(TestModel.SvgInsets));
            slicingSvg.SetBinding(SvgImageView.SvgPathProperty, nameof(TestModel.SvgResourcePath));

            // The root page of your application
            MainPage = new ContentPage {
                Content = new StackLayout {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = {
                        resourcePicker,
                        insetSlider,
                        slicingSvg,
                    },
                    BindingContext = _ViewModel,
                },
            };
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}

