using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Diagnostics;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Process> listProcess = new List<Process>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count < 5)
            {
                ProcessStartInfo ballonProcess = new ProcessStartInfo("C:\\Users\\jinha\\Dropbox\\2017-2018 Télécom Bretagne F2B\\F2B-205-NET\\Ballon.exe");
                Process p = Process.Start(ballonProcess);
                listProcess.Add(p);
            }
            else
            {
                MessageBox.Show("Cannot create more than 5 processes", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            
        }
    }
}
