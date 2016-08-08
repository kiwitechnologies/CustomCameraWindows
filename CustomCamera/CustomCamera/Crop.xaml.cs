using CustomCamera.Services;
using System;
using System.Collections.Generic;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using CustomCamera.Helpers;
using System.Diagnostics;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.IO;

namespace CustomCamera
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Crop : Page
    {
        //private CameraService cameraService;
        StorageFile file;
        BitmapImage bitmapImage = new BitmapImage();

        public Crop()
        {
            this.InitializeComponent();
            //cameraService = new CameraService(CameraPreview);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //await cameraService.InitializeCameraAsync();
            base.OnNavigatedTo(e);
            //var result = (StorageFile)e.Parameter;

            //file = result;
            //Debug.WriteLine("Photo taken! Saving to " + file.Path);
            //FileRandomAccessStream imageStream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
            //bitmapImage.SetSource(imageStream);
            //var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(imageStream);
            var wb = new WriteableBitmap(1, 1);
            //await wb.LoadAsync(file);
            var rr = (InMemoryRandomAccessStream)e.Parameter;
            //rr.Seek(0);
            var memStream = new MemoryStream();
            await rr.AsStream().CopyToAsync(memStream);
            IRandomAccessStream _IRandomAccessStream = await ConvertToRandomAccessStream(memStream);
            wb.SetSource(_IRandomAccessStream);
            this.ImageCropper.SourceImage = wb;
            //this.CameraPreview.Visibility = Visibility.Collapsed;

        }

        public static async Task<IRandomAccessStream> ConvertToRandomAccessStream(MemoryStream memoryStream)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var dw = new DataWriter(outputStream);
            var task = Task.Factory.StartNew(() => dw.WriteBytes(memoryStream.ToArray()));
            await task;
            await dw.StoreAsync();
            await outputStream.FlushAsync();
            return randomAccessStream;
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            //StorageFolder temp = ApplicationData.Current.LocalCacheFolder;
            //StorageFile file = await temp.CreateFileAsync("current_image.png", CreationCollisionOption.ReplaceExisting);

            //await cameraService.MediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);

            //file = null;
            //file = await temp.GetFileAsync("current_image.png");


            //await cameraService.StopPreviewAsync();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("Bitmap", new List<string>() { ".bmp" });
            savePicker.FileTypeChoices.Add("Graphical Interchange Format", new List<string>() { ".gif" });
            savePicker.FileTypeChoices.Add("Joint Photographic Experts Group", new List<string>() { ".jpg" });
            savePicker.FileTypeChoices.Add("Portable Network Graphics", new List<string>() { ".png" });
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                await GlobalVariable.CroppedImage.SaveAsync(file);
                //await (this.CroppedImage.Source as WriteableBitmap).SaveAsync(file);
            }
        }
    }
}
