using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot.Controls
{
    public class CheckboxButton : Button
    {
        public CheckboxButton()
        {
            this.IsChecked = false;
            Pressed += (s, e) =>
            {
                IsChecked = !IsChecked;
                GoToState(IsChecked);
            };
        }

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set
            {
                SetValue(IsCheckedProperty, value);
                CheckedChanged?.Invoke(this, value);
                if (value)
                {
                    Checked?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Unchecked?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(CheckboxButton),
            false);

        public event EventHandler<bool>? CheckedChanged;
        public event EventHandler? Checked;
        public event EventHandler? Unchecked;

        private void GoToState(bool isChecked)
        {
            string visualState = isChecked ? "Checked" : "Unchecked";
            VisualStateManager.GoToState(this, visualState);
            var groups = VisualStateManager.GetVisualStateGroups(this);
        }
    }
}
