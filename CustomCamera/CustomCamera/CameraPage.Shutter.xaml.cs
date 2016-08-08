using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CustomCamera
{
    public sealed partial class CameraPage : Page
    {
        private void UpdateExposureControlCapabilities()
        {
            var exposureControl = _mediaCapture.VideoDeviceController.ExposureControl;

            if (exposureControl.Supported)
            {
                ShutterButton.Tag = Visibility.Visible;

                ShutterAutoCheckBox.IsChecked = exposureControl.Auto;

                // Unhook the event handler, so that changing properties on the slider won't trigger an API call
                ShutterSlider.ValueChanged -= ShutterSlider_ValueChanged;

                ShutterSlider.Minimum = exposureControl.Min.Ticks;
                ShutterSlider.Maximum = exposureControl.Max.Ticks;
                ShutterSlider.StepFrequency = exposureControl.Step.Ticks;

                var value = exposureControl.Value;
                ShutterTextBlock.Text = value.TotalMilliseconds + " ms";
                ShutterSlider.Value = value.Ticks;

                ShutterSlider.ValueChanged += ShutterSlider_ValueChanged;
            }
            else
            {
                ShutterButton.Visibility = Visibility.Collapsed;
                ShutterButton.Tag = Visibility.Collapsed;
            }
        }

        private async void ShutterSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (_settingUpUi) return;

            var value = TimeSpan.FromTicks((long)(sender as Slider).Value);

            ShutterTextBlock.Text = value.TotalMilliseconds + " ms";

            await _mediaCapture.VideoDeviceController.ExposureControl.SetValueAsync(value);
        }

        private async void ShutterCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var autoShutter = ((sender as CheckBox).IsChecked == true);

            await _mediaCapture.VideoDeviceController.ExposureControl.SetAutoAsync(autoShutter);
        }
    }
}
