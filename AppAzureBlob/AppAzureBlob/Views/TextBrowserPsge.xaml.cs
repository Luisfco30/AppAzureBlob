﻿using AppAzureBlob.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAzureBlob.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextBrowserPsge : ContentPage
    {
        string fileNameSelected = String.Empty;


        public TextBrowserPsge()
        {
            InitializeComponent();
        }

        private async void buttonGetTextFileList_Clicked(object sender, EventArgs e)
        {
            try
            {
                var fileList = await new AzureService().GetFilesListAsync(AzureContainer.Text);
                listViewFiles.ItemsSource = fileList;
                editorPreview.Text = string.Empty;
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
                    fileNameSelected = e.SelectedItem.ToString();
                    var byteData = await new AzureService().GetFileAsync(AzureContainer.Text, fileNameSelected);
                    var text = Encoding.UTF8.GetString(byteData);

                    editorPreview.Text = text;
                    buttonDelete.IsEnabled = true;
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

        private async void buttonDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileNameSelected))
                {
                    if (await new AzureService().DeleteFileAsync(AzureContainer.Text, fileNameSelected))
                    {
                        buttonGetTextFileList_Clicked(sender, e);
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

        private async void buttonGetTextFileList12_Clicked(object sender, EventArgs e)
        {

            try
            {
                var fileList = await new AzureServices12().GetFilesListAsync(AzureContainer.Text);
                listViewFiles.ItemsSource = fileList;
                editorPreview.Text = string.Empty;
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