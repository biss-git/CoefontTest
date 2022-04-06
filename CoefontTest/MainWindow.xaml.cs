using CoefontApi.v1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoefontTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CoefontClient client;

        public MainWindow()
        {
            InitializeComponent();
            client = new CoefontClient();
        }

        private void SetKey()
        {
            client.AccessKey = accessKey.Text;
            client.ClientSecret = clientSecret.Text;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            client?.Dispose();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SetKey();
            var json = await client.GetDict();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetKey();
            var dictItems = new DictItem[1]
            {
                new DictItem
                {
                    text = "こんにちは",
                    category = "挨拶",
                    yomi = "コンニチワ",
                    accent = "12222",
                }
            };

            await client.PostDict(dictItems);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SetKey();
            var dictItems = new DictItem[1]
            {
                new DictItem
                {
                    text = "こんにちは",
                    category = "挨拶",
                    yomi = "コンニチワ",
                    accent = "12222",
                }
            };

            await client.DeleteDict(dictItems);
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SetKey();
            var json = await client.GetPro();
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SetKey();
            var json = await client.GetEnterprise();
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SetKey();
            var text = new Text() { text = "こんにちは" };
            var response = await client.PostText2Speeach(text);
            File.WriteAllBytes("temp." + Enum.GetName(typeof(FileType), text.format), response.Wave);
        }
    }
}
