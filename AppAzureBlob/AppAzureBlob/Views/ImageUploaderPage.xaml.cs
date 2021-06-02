using AppAzureBlob.Services;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAzureBlob.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageUploaderPage : ContentPage
    {
        byte[] ByteData;

        public ImageUploaderPage()
        {
            InitializeComponent();
        }

        private async void buttonTakePicture_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppAzureBlob", "No existe cámara disponible en el dispositivo", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "AppAzureBlob",
                    Name = "PetPicture.jpg"
                });

                if (file == null)
                    return;

                ByteData = await ConvertImageFilePathToByteArray(file.Path);
                ImagePicture.Source = ImageSource.FromStream(() => new MemoryStream(ByteData));

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppAzureBlob", $"Se generó un error al tomar la fotografía ({ex.Message})", "OK");
                throw;
            }
        }

        private async void buttonSelectPicture_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppAzureBlob", "No es posible seleccionar fotografía en el dispositivo", "OK");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                ByteData = await ConvertImageFilePathToByteArray(file.Path);
                ImagePicture.Source = ImageSource.FromStream(() => new MemoryStream(ByteData));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppAzureBlob", $"Se generó un error al seleccionar la fotografía ({ex.Message})", "OK");
                throw;
            }
        }

        private async void buttonUpload_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ImagePicture.Source != null && ByteData.Length > 0)
                {
                    buttonUpload.IsEnabled = false;
                    ActivityIndicator.IsRunning = true;

                    await new AzureService().UploadFileAsync(AzureContainer.Image, new MemoryStream(ByteData));
                    ImagePicture.Source = null;
                }
                else
                {
                    labelMessage.Text = "Debes capturar una imagen";
                    await Task.Delay(5000);
                }
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                buttonUpload.IsEnabled = true;
                ActivityIndicator.IsRunning = false;
            }
        }

        private async Task<byte[]> ConvertImageFilePathToByteArray(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return bytes;
            }
            else
            {
                return null;
            }
        }

        private async void buttonUpload12_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ImagePicture.Source != null && ByteData.Length > 0)
                {
                    buttonUpload12.IsEnabled = false;
                    ActivityIndicator.IsRunning = true;

                    await new AzureServices12().UploadFileAsync(AzureContainer.Image, new MemoryStream(ByteData));
                    ImagePicture.Source = null;
                }
                else
                {
                    labelMessage.Text = "Debes ingresar una imagen";
                    await Task.Delay(5000);
                }
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                buttonUpload12.IsEnabled = true;
                ActivityIndicator.IsRunning = false;
            }
        }

    }
}