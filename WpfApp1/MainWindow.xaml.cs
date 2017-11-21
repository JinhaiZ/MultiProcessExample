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
using System.Collections.ObjectModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LinkedList<ProcessNode> listProcess = new LinkedList<ProcessNode>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void startBallon_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count == 0 || listProcess.First().getBallonCount() < 5)
            {
                ProcessStartInfo ballonProcess = new ProcessStartInfo("Ballon.exe");

                Process p = Process.Start(ballonProcess);
                int pid = p.Id;
                listProcess.AddLast(new ProcessNode(p, "ballon"));
                lvProcess.Items.Add(new ProcessItem() { ProcessName = "ballon.exe", PID = pid });
                ballonCountView.Text = listProcess.First().getBallonCount().ToString();
                countView.Text = (listProcess.First().getBallonCount() + listProcess.First().getPremierCount()).ToString();
            }
            else
            {
                MessageBox.Show("Cannot create more than 5 processes", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void startPremier_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count == 0 || listProcess.First().getPremierCount() < 5)
            {
                ProcessStartInfo premierProcess = new ProcessStartInfo("premier.exe");

                Process p = Process.Start(premierProcess);
                int pid = p.Id;
                listProcess.AddLast(new ProcessNode(p, "premier"));
                lvProcess.Items.Add(new ProcessItem() { ProcessName = "premier.exe", PID = pid });
                premierCountView.Text = listProcess.First().getPremierCount().ToString();
                countView.Text = (listProcess.First().getBallonCount() + listProcess.First().getPremierCount()).ToString();
            }
            else
            {
                MessageBox.Show("Cannot create more than 5 processes", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void lvProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }

    public class ProcessItem
    {
        public string ProcessName { get; set; }
        public int PID { get; set; }
    }

    public class ProcessNode
    {
        public Process process;
        public string name;
        public static int ballonCount = 0;
        public static int premierCount = 0;

        public ProcessNode(Process process, string name)
        {
            this.process = process;
            this.name = name;
            if (name == "ballon")
            {
                ballonCount += 1;
            }
            else
            {
                premierCount += 1;
            }
        }

        public int getBallonCount()
        {
            return ballonCount;
        }
        public int getPremierCount()
        {
            return premierCount;
        }
    }

}
