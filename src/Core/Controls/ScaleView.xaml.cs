using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Zoom_Task.Core.Controls
{
    public partial class ScaleView : ContentView
    {
        #region Bindable Properties
        public static readonly BindableProperty ZoomInTitleProperty = BindableProperty.Create(nameof(ZoomInTitle), typeof(string), typeof(ScaleView), "Zoom In", BindingMode.TwoWay);

        public static readonly BindableProperty ZoomInTitleFontSizeProperty = BindableProperty.Create(nameof(ZoomInTitleFontSize), typeof(double), typeof(ScaleView), 10.0, BindingMode.TwoWay);

        public static readonly BindableProperty ZoomInTitleFontProperty = BindableProperty.Create(nameof(ZoomInTitleFont), typeof(Font), typeof(ScaleView), Font.Default, BindingMode.TwoWay);

        public static readonly BindableProperty ZoomInTitleTextColorProperty = BindableProperty.Create(nameof(ZoomInTitleTextColor), typeof(Color), typeof(ScaleView), Color.Black, BindingMode.TwoWay);


        public static readonly BindableProperty CancelTitleProperty = BindableProperty.Create(nameof(CancelTitle), typeof(string), typeof(ScaleView), "Cancel", BindingMode.TwoWay);

        public static readonly BindableProperty CancelTitleFontSizeProperty = BindableProperty.Create(nameof(CancelTitleFontSize), typeof(double), typeof(ScaleView), 10.0, BindingMode.TwoWay);

        public static readonly BindableProperty CancelTitleFontProperty = BindableProperty.Create(nameof(CancelTitleFont), typeof(Font), typeof(ScaleView), Font.Default, BindingMode.TwoWay);

        public static readonly BindableProperty CancelTitleTextColorProperty = BindableProperty.Create(nameof(CancelTitleTextColor), typeof(Color), typeof(ScaleView), Color.Red, BindingMode.TwoWay);


        public static readonly BindableProperty AcceptTitleProperty = BindableProperty.Create(nameof(AcceptTitle), typeof(string), typeof(ScaleView), "Accept", BindingMode.TwoWay);

        public static readonly BindableProperty AcceptTitleFontSizeProperty = BindableProperty.Create(nameof(AcceptTitleFontSize), typeof(double), typeof(ScaleView), 10.0, BindingMode.TwoWay);

        public static readonly BindableProperty AcceptTitleFontProperty = BindableProperty.Create(nameof(AcceptTitleFont), typeof(Font), typeof(ScaleView), Font.Default, BindingMode.TwoWay);

        public static readonly BindableProperty AcceptTitleTextColorProperty = BindableProperty.Create(nameof(AcceptTitleTextColor), typeof(Color), typeof(ScaleView), Color.Green, BindingMode.TwoWay);


        public static readonly BindableProperty ZoomInViewProperty = BindableProperty.Create(nameof(ZoomInView), typeof(View), typeof(ScaleView), null, BindingMode.TwoWay, propertyChanged: (sender, oldVal, newVal) =>
        {
            ScaleView scaleView = sender as ScaleView;
            scaleView.ShowHideZoomInView();
        });

        public static readonly BindableProperty ZoomOutViewProperty = BindableProperty.Create(nameof(ZoomOutView), typeof(View), typeof(ScaleView), null, BindingMode.TwoWay, propertyChanged: (sender, oldVal, newVal) =>
        {
            ScaleView scaleView = sender as ScaleView;
            scaleView.ShowHideZoomOutView();
        });

        public static readonly BindableProperty IsZoomedInProperty = BindableProperty.Create(nameof(IsZoomedIn), typeof(bool), typeof(ScaleView), false, BindingMode.OneWayToSource);
        #endregion

        #region Events
        public event EventHandler<EventArgs> ZoomedIn;

        public event EventHandler<EventArgs> ZoomedOut;
        #endregion

        #region Propeties
        public string ZoomInTitle
        {
            get { return (string)GetValue(ZoomInTitleProperty); }
            set { SetValue(ZoomInTitleProperty, value); }
        }

        public double ZoomInTitleFontSize
        {
            get { return (double)GetValue(ZoomInTitleFontSizeProperty); }
            set { SetValue(ZoomInTitleFontSizeProperty, value); }
        }

        public Font ZoomInTitleFont
        {
            get { return (Font)GetValue(ZoomInTitleFontProperty); }
            set { SetValue(ZoomInTitleFontProperty, value); }
        }

        public Color ZoomInTitleTextColor
        {
            get { return (Color)GetValue(ZoomInTitleTextColorProperty); }
            set { SetValue(ZoomInTitleTextColorProperty, value); }
        }

        public string CancelTitle
        {
            get { return (string)GetValue(CancelTitleProperty); }
            set { SetValue(CancelTitleProperty, value); }
        }

        public double CancelTitleFontSize
        {
            get { return (double)GetValue(CancelTitleFontSizeProperty); }
            set { SetValue(CancelTitleFontSizeProperty, value); }
        }

        public Font CancelTitleFont
        {
            get { return (Font)GetValue(CancelTitleFontProperty); }
            set { SetValue(CancelTitleFontProperty, value); }
        }

        public Color CancelTitleTextColor
        {
            get { return (Color)GetValue(CancelTitleTextColorProperty); }
            set { SetValue(CancelTitleTextColorProperty, value); }
        }

        public string AcceptTitle
        {
            get { return (string)GetValue(AcceptTitleProperty); }
            set { SetValue(AcceptTitleProperty, value); }
        }

        public double AcceptTitleFontSize
        {
            get { return (double)GetValue(AcceptTitleFontSizeProperty); }
            set { SetValue(AcceptTitleFontSizeProperty, value); }
        }

        public Font AcceptTitleFont
        {
            get { return (Font)GetValue(AcceptTitleFontProperty); }
            set { SetValue(AcceptTitleFontProperty, value); }
        }

        public Color AcceptTitleTextColor
        {
            get { return (Color)GetValue(AcceptTitleTextColorProperty); }
            set { SetValue(AcceptTitleTextColorProperty, value); }
        }

        public View ZoomInView
        {
            get { return (View)GetValue(ZoomInViewProperty); }
            set { SetValue(ZoomInViewProperty, value); }
        }

        public View ZoomOutView
        {
            get { return (View)GetValue(ZoomOutViewProperty); }
            set { SetValue(ZoomOutViewProperty, value); }
        }

        public bool IsZoomedIn
        {
            get { return (bool)GetValue(IsZoomedInProperty); }
            private set { SetValue(IsZoomedInProperty, value); }
        }
        #endregion

        #region Fields
        private Rectangle _orignalBounds;
        #endregion

        public ScaleView()
        {
            InitializeComponent();
            LblAccept.BindingContext = this;
            LblAccept.SetBinding(Label.TextProperty, "AcceptTitle");
            LblAccept.SetBinding(Label.FontProperty, "AcceptTitleFont");
            LblAccept.SetBinding(Label.FontSizeProperty, "AcceptTitleFontSize");
            LblAccept.SetBinding(Label.TextColorProperty, "AcceptTitleTextColor");

            LblCancel.BindingContext = this;
            LblCancel.SetBinding(Label.TextProperty, "CancelTitle");
            LblCancel.SetBinding(Label.FontProperty, "CancelTitleFont");
            LblCancel.SetBinding(Label.FontSizeProperty, "CancelTitleFontSize");
            LblCancel.SetBinding(Label.TextColorProperty, "CancelTitleTextColor");

            LblScaleView.BindingContext = this;
            LblScaleView.SetBinding(Label.TextProperty, "ZoomInTitle");
            LblScaleView.SetBinding(Label.FontProperty, "ZoomInTitleFont");
            LblScaleView.SetBinding(Label.FontSizeProperty, "ZoomInTitleFontSize");
            LblScaleView.SetBinding(Label.TextColorProperty, "ZoomInTitleTextColor");

            ButtonContainer.BindingContext = this;
        }

        async void BtnScaleView_Clicked(System.Object sender, System.EventArgs e)
        {
            await ZoomIn();
            ZoomedIn?.Invoke(this, e);
        }

        async void Cancel_Clicked(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Button Clicked : {this.Scale}");
            await ZoomOut();
            ZoomedOut?.Invoke(this, e);
        }

        async void Accept_Clicked(System.Object sender, System.EventArgs e)
        {
            await ZoomOut();
            ZoomedOut?.Invoke(this, e);
        }

        async Task ZoomIn()
        {
            //var width = Application.Current.MainPage.Width;
            //var height = Application.Current.MainPage.Height;

            //_orignalBounds = this.Bounds;
            //System.Diagnostics.Debug.WriteLine($"Button Clicked : {this.Scale}, Device Width : {width}, Height : {height}, Control Size : Width : {_orignalBounds.Width}, {_orignalBounds.X}, Height  : {_orignalBounds.Height}, {_orignalBounds.Y}");
            //double x = width - this.Bounds.Width;
            //double y = this.Bounds.Y - this.Bounds.Height;
            //await this.LayoutTo(new Rectangle(this.Bounds.X - x, y, width, this.Bounds.Height * 2), 100, Easing.Linear);

            //await this.ScaleTo(2, 100, Easing.Linear);
            if (ZoomOutView != null)
                ZoomOutView.IsVisible = false;

            if (ZoomInView != null)
                ZoomInView.IsVisible = true;

            IsZoomedIn = true;
        }

        async Task ZoomOut()
        {
            //await this.LayoutTo(_orignalBounds, 100, Easing.Linear);
            //await this.ScaleTo(1, 100, Easing.Linear);
            if (ZoomOutView != null)
                ZoomOutView.IsVisible = true;

            if (ZoomInView != null)
                ZoomInView.IsVisible = false;

            IsZoomedIn = false;
        }

        void ShowHideZoomInView()
        {
            if (ZoomInView == null)
                return;

            MainContainer.Children.Add(ZoomInView);
            ZoomInView.IsVisible = IsZoomedIn;
        }

        void ShowHideZoomOutView()
        {
            if (ZoomOutView == null)
                return;

            MainContainer.Children.Add(ZoomOutView);
            ZoomOutView.IsVisible = !IsZoomedIn;
        }
    }
}

