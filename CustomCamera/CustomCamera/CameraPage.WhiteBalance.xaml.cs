using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Devices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace CustomCamera
{
    public sealed partial class CameraPage : Page
    {
        private void UpdateWbControlCapabilities()
        {
            var whiteBalanceControl = _mediaCapture.VideoDeviceController.WhiteBalanceControl;

            if (whiteBalanceControl.Supported)
            {
                WbButton.Tag = Visibility.Visible;

                if (WbComboBox.ItemsSource == null)
                {
                    WbComboBox.ItemsSource = Enum.GetValues(typeof(ColorTemperaturePreset)).Cast<ColorTemperaturePreset>();
                }

                WbComboBox.SelectedItem = whiteBalanceControl.Preset;

                if (whiteBalanceControl.Max - whiteBalanceControl.Min > whiteBalanceControl.Step)
                {
                    // Unhook the event handler, so that changing properties on the slider won't trigger an API call
                    WbSlider.ValueChanged -= WbSlider_ValueChanged;

                    var value = whiteBalanceControl.Value;
                    WbSlider.Minimum = whiteBalanceControl.Min;
                    WbSlider.Maximum = whiteBalanceControl.Max;
                    WbSlider.StepFrequency = whiteBalanceControl.Step;
                    WbSlider.Value = value;

                    WbSlider.ValueChanged += WbSlider_ValueChanged;
                }
                else
                {
                    WbSlider.Visibility = Visibility.Collapsed;
                    WbTextBox.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                WbButton.Visibility = Visibility.Collapsed;
                WbButton.Tag = Visibility.Collapsed;
            }
        }

        private async void WbComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (ColorTemperaturePreset)WbComboBox.SelectedItem;

            WbSlider.IsEnabled = (selected == ColorTemperaturePreset.Manual);

            await _mediaCapture.VideoDeviceController.WhiteBalanceControl.SetPresetAsync(selected);

            // Note: Automatic white balance can be locked to the current value by setting the preset to manual and not passing in a value to the control
        }

        private async void WbSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (_settingUpUi) return;

            var value = (sender as Slider).Value;

            await _mediaCapture.VideoDeviceController.WhiteBalanceControl.SetValueAsync((uint)value);
        }
    }
}
