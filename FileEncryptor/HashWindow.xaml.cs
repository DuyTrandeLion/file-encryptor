using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace FileEncryptor
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class HashWindow : Window, INotifyPropertyChanged
    {

        public HashWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private string publicKey;
        private string product;
        private string productVersion;
        private string publicKeyID;

        public string PublicKey
        {
            get
            {
                return this.publicKey;
            }
            set
            {
                if (value != this.publicKey)
                {
                    this.publicKey = value;
                    Notify("PublicKey");
                }
            }
        }

        public string Product
        {
            get
            {
                return this.product;
            }
            set
            {
                if (value != this.product)
                {
                    this.product = value;
                    Notify("Product");
                }
            }
        }

        public string ProductVersion
        {
            get
            {
                return this.productVersion;
            }
            set
            {
                if (value != this.productVersion)
                {
                    this.productVersion = value;
                    Notify("ProductVersion");
                }
            }
        }

        public string PublicKeyID
        {
            get
            {
                return this.publicKeyID;
            }
            set
            {
                if (value != this.publicKeyID)
                {
                    this.publicKeyID = value;
                    Notify("PublicKeyID");
                }
            }
        }

        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private void bt_Hash_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateOutput(this.tb_output, CalculateMD5(this.tb_plainFilePath.Text), true);
        }

        private void tb_output_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.tb_output.ScrollToEnd();
        }

        private void bt_selPlain_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".*";
            ofd.Filter = FileEncryptor.Properties.Resources.All_File_Type;
            ofd.Title = FileEncryptor.Properties.Resources.DialogTitle_SelectPlain;
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                this.tb_plainFilePath.Text = ofd.FileName;
            }
        }

        private void UpdateOutput(TextBox tb, string message, bool append)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                string newMessage = append ? tb.Text + "\r\n" + message : message;
                tb.Text = newMessage;
            }), null);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

    }
}
