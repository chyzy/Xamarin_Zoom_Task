using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Zoom_Task.Core.Controls
{	
	public partial class ScaleView : ContentView
	{
        #region Bindable Properties
        public static readonly BindableProperty ZoomInTitleProperty = BindableProperty.Create(nameof(ZoomInTitle), typeof(string), typeof(ScaleView), "Zoom In", BindingMode.TwoWay);

        public static readonly BindableProperty CancelTitleProperty = BindableProperty.Create(nameof(CancelTitle), typeof(string), typeof(ScaleView), "Cancel", BindingMode.TwoWay);

        public static readonly BindableProperty AcceptTitleProperty = BindableProperty.Create(nameof(AcceptTitle), typeof(string), typeof(ScaleView), "Accept", BindingMode.TwoWay);

        public static readonly BindableProperty ZoomInViewProperty = BindableProperty.Create(nameof(ZoomInView), typeof(View), typeof(ScaleView), null, BindingMode.TwoWay, propertyChanged: (sender, oldVal, newVal) =>
        {
            ScaleView scaleView = sender as ScaleView;
            scaleView.ShowHideZoomInView();
        });

        public static readonly BindableProperty MaximumScaleProperty = BindableProperty.Create(nameof(MaximumScale), typeof(double), typeof(ScaleView), null, BindingMode.TwoWay);
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

        public string CancelTitle
        {
            get { return (string)GetValue(CancelTitleProperty); }
            set { SetValue(CancelTitleProperty, value); }
        }

        public string AcceptTitle
        {
            get { return (string)GetValue(AcceptTitleProperty); }
            set { SetValue(AcceptTitleProperty, value); }
        }

        public View ZoomInView
        {
            get { return (View)GetValue(ZoomInViewProperty); }
            set { SetValue(ZoomInViewProperty, value); }
        }

        public double MaximumScale
        {
            get { return (double)GetValue(MaximumScaleProperty); }
            set { SetValue(MaximumScaleProperty, value); }
        }
        #endregion

        public ScaleView ()
		{
			InitializeComponent ();
            BtnAccept.BindingContext = this;
            BtnAccept.SetBinding(Button.TextProperty, "AcceptTitle");

            BtnScaleOut.BindingContext = this;
            BtnScaleOut.SetBinding(Button.TextProperty, "CancelTitle");

            BtnScaleView.BindingContext = this;
            BtnScaleView.SetBinding(Button.TextProperty, "ZoomInTitle");
		}

        void BtnScaleView_Clicked(System.Object sender, System.EventArgs e)
        {
			System.Diagnostics.Debug.WriteLine($"Button Clicked : {this.Scale}");
			this.ScaleTo(2, 100, Easing.Linear);
            ZoomedIn?.Invoke(this, e);
        }

        void BtnScaleOut_Clicked(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Button Clicked : {this.Scale}");
            this.ScaleTo(1, 100, Easing.Linear);
            ZoomedOut?.Invoke(this, e);
        }

        void BtnAccept_Clicked(System.Object sender, System.EventArgs e)
        {
            ZoomedOut?.Invoke(this, e);
        }

        void ShowHideZoomInView()
        {
            if (ZoomInView == null)
                return;

            Grid.SetRow(ZoomInView, 1);
            Grid.SetColumn(ZoomInView, 0);
            Grid.SetColumnSpan(ZoomInView, 2);

            MainGrid.Children.Add(ZoomInView);
        }
    }
}

