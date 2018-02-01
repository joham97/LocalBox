using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace LocalBox
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<String> files;

        private Client client;

        private string currentDirectory;

        public MainWindow()
        {
            InitializeComponent();
            files = new List<string>();
            client = new Client();

            currentDirectory = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(currentDirectory + "/tmp");

            loadList();
        }
        
        private void loadList()
        {
            listBox.Items.Clear();
            foreach (String file in client.GetList())
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = file;
                listBox.Items.Add(item);
            }
        }

        private void List_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (String file in files)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = file;
                    listBox.Items.Add(item);
                    client.SendFile(file);
                }

                loadList();
            }
        }

        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            loadList();
        }

        private void Button_Click_Save_All(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in this.listBox.Items)
            {
                client.LoadFile(item.Content.ToString(), currentDirectory + "/tmp");
            }
        }
    }
}
