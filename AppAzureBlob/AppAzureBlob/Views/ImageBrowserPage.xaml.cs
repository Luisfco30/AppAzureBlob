using AppAzureBlob.Services;
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
    public partial class ImageBrowserPage : ContentPage
    {
        public ImageBrowserPage()
        {
            InitializeComponent();
        }

        string FileNameSelected = string.Empty;

        private async void GetFilesList_Clicked(object sender, EventArgs e)
        {
            try
            {
                var fileList = await new AzureService().GetFilesListAsync(AzureContainer.Image);
                listViewFiles.ItemsSource = fileList;
                imageDownloader.Source = null;
                buttonDelete.IsEnabled = false;
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                labelMessage.Text = string.Empty;
            }
        }

        private async void listViewFiles_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    FileNameSelected = e.SelectedItem.ToString();
                    var byteData = await new AzureService().GetFileAsync(AzureContainer.Image, FileNameSelected);
                    var image = ImageSource.FromStream(() => new MemoryStream(byteData));
                    imageDownloader.Source = image;
                    buttonDelete.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {

                labelMessage.Text = ex.Message;
                await Task.Delay(5000);
            }
            finally
            {
                labelMessage.Text = string.Empty;
            }
        }

        private async void buttonDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(FileNameSelected))
                {
                    if (await new AzureService().DeleteFileAsync(AzureContainer.Image, FileNameSelected))
                    {
                        GetFilesList_Clicked(sender, e);
                    }
                }

            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                labelMessage.Text = string.Empty;
            }
        }

        private async void GetFilesList12_Clicked(object sender, EventArgs e)
        {
            try
            {
                var fileList = await new AzureServices12().GetFilesListAsync(AzureContainer.Image);
                listViewFiles.ItemsSource = fileList;
                imageDownloader.Source = null;
                buttonDelete.IsEnabled = false;
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
                await Task.Delay(5000);
            }
            finally
            {
                labelMessage.Text = string.Empty;
            }
        }
    }
}