using System;
using System.Collections.Generic;
using System.Text;

namespace dotRobot.Controls
{
    public partial class Joystick : ContentView
    {
        public Joystick()
        {
            InitializeComponent();

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);
        }

        public event EventHandler? PositionReset;
        public event EventHandler<Point>? PositionChanged;

        // To make the control square
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var side = Math.Min(width, height);

            WidthRequest = side;
            HeightRequest = side;
        }

        private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    double radius = this.Width / 2;
                    double x = Math.Clamp(e.TotalX, -radius, radius);
                    double y = Math.Clamp(e.TotalY, -radius, radius);
                    Thumb.TranslationX = x;
                    Thumb.TranslationY = y;
                    PositionChanged?.Invoke(this, new Point(x / radius, (-y) / radius));
                    break;

                case GestureStatus.Completed:
                    ResetThumb();
                    PositionReset?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private async void ResetThumb()
        {
            await Thumb.TranslateToAsync(0, 0, 100, Easing.CubicOut);
        }
    }
}
