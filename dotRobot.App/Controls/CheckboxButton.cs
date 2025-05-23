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
            }
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(CheckboxButton),
            false);

        public event EventHandler<bool>? CheckedChanged;

        private void GoToState(bool isChecked)
        {
            string visualState = isChecked ? "Checked" : "Unchecked";
            VisualStateManager.GoToState(this, visualState);
            var groups = VisualStateManager.GetVisualStateGroups(this);
        }
    }
}
