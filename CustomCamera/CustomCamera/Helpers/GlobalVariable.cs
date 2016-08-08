using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace CustomCamera.Helpers
{
    public static class GlobalVariable
    {
        private static WriteableBitmap croppedImage;
        public static WriteableBitmap CroppedImage
        {
            get { return croppedImage; }
            set
            {
                croppedImage = value;
            }
        }

        private static InMemoryRandomAccessStream imageStream;
        public static InMemoryRandomAccessStream ImageStream
        {
            get { return imageStream; }
            set
            {
                imageStream = value;
            }
        }
    }
}
