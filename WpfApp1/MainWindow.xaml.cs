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
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LinkedList<ProcessNode> listProcess;

        public MainWindow()
        {
            InitializeComponent();
            this.listProcess = new LinkedList<ProcessNode>();
        }


        private void setViewCounters(int ballonCount, int premierCount)
        {
            ballonCountView.Text = ballonCount.ToString();
            premierCountView.Text = premierCount.ToString();
            countView.Text = (ballonCount + premierCount).ToString();
        }

        private void stopAllProcess(LinkedList<ProcessNode> listProcess)
        {
            foreach (ProcessNode pN in listProcess)
            {
                pN.process.Kill();
                lvProcess.Items.Remove(pN.processViewItem);
            }
            listProcess = new LinkedList<ProcessNode>();
            setViewCounters(0, 0);
        }
        private void process_Exited(object sender, System.EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                checkClosedProcess();
            });
            // inspired by this post: https://stackoverflow.com/questions/9732709/the-calling-thread-cannot-access-this-object-because-a-different-thread-owns-it

        }

        private void checkClosedProcess()
        {
            foreach (ProcessNode pN in listProcess)
            {
                if (pN.process.HasExited)
                {
                    lvProcess.Items.Remove(pN.processViewItem);
                    if (pN.name == "ballon")
                    {
                        pN.setBallonCount(pN.getBallonCount() - 1);
                    }
                    else
                    {
                        pN.setPremierCount(pN.getPremierCount() - 1);
                    }
                    setViewCounters(pN.getBallonCount(), pN.getPremierCount());
                    listProcess.Remove(pN);
                    break;
                }
                
            }
        }
        private void startBallon_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count == 0 || listProcess.First().getBallonCount() < 5)
            {

                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("Ballon.exe");
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(process_Exited);
                p.Start();
                int pid = p.Id;
                ProcessItem item = new ProcessItem() { ProcessName = "ballon.exe", PID = pid };
                listProcess.AddLast(new ProcessNode(p, "ballon", item));
                lvProcess.Items.Add(item);
                setViewCounters(listProcess.First().getBallonCount(), listProcess.First().getPremierCount());
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
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("premier.exe");
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(process_Exited);
                p.Start();
                int pid = p.Id;
                ProcessItem item = new ProcessItem() { ProcessName = "premier.exe", PID = pid };
                listProcess.AddLast(new ProcessNode(p, "premier", item));
                lvProcess.Items.Add(item);
                setViewCounters(listProcess.First().getBallonCount(), listProcess.First().getPremierCount());
            }
            else
            {
                MessageBox.Show("Cannot create more than 5 processes", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void lvProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StopAllProcess_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0)
            {
                MessageBox.Show("No running process", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                stopAllProcess(listProcess);
            }
            
        }

        private void StopLastProcess_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0)
            {
                MessageBox.Show("No running process", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                listProcess.Last().process.Kill();
                lvProcess.Items.Remove(listProcess.Last().processViewItem);
                if (listProcess.Last().name == "ballon")
                {
                    listProcess.Last().setBallonCount(listProcess.Last().getBallonCount() - 1);
                }
                else
                {
                    listProcess.Last().setPremierCount(listProcess.Last().getPremierCount() - 1);
                }
                
                setViewCounters(listProcess.Last().getBallonCount(), listProcess.Last().getPremierCount());
                listProcess.RemoveLast();
            }
        }

        private void StopLastPremier_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0 || listProcess.First().getPremierCount() <= 0)
            {
                MessageBox.Show("No running process for premier.exe", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                for (int i = listProcess.Count -1; i >=0; i--)
                {
                    if (listProcess.ElementAt(i).name == "premier")
                    {
                        //get last node of type premier from the linkedlist
                        ProcessNode toRemove = listProcess.ElementAt(i);
                        toRemove.process.Kill();
                        lvProcess.Items.Remove(toRemove.processViewItem);
                        toRemove.setPremierCount(toRemove.getPremierCount() - 1);
                        setViewCounters(toRemove.getBallonCount(), toRemove.getPremierCount());
                        listProcess.Remove(toRemove);
                        break;
                    }
                }
            }
        }

        private void stopLastBallon_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0 || listProcess.First().getBallonCount() <= 0)
            {
                MessageBox.Show("No running process for ballon.exe", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                for (int i = listProcess.Count - 1; i >= 0; i--)
                {
                    if (listProcess.ElementAt(i).name == "ballon")
                    {
                        //get last node of type premier from the linkedlist
                        ProcessNode toRemove = listProcess.ElementAt(i);
                        toRemove.process.Kill();
                        lvProcess.Items.Remove(toRemove.processViewItem);
                        toRemove.setBallonCount(toRemove.getBallonCount() - 1);
                        setViewCounters(toRemove.getBallonCount(), toRemove.getPremierCount());
                        listProcess.Remove(toRemove);
                        break;
                    }
                }
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want quit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                stopAllProcess(listProcess);
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
        public ProcessItem processViewItem;
        public static int ballonCount = 0;
        public static int premierCount = 0;

        public ProcessNode(Process process, string name, ProcessItem processViewItem)
        {
            this.process = process;
            this.name = name;
            this.processViewItem = processViewItem;
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
        public void setBallonCount(int count)
        {
            ballonCount = count;
        }
        public void setPremierCount(int count)
        {
            premierCount = count;
        }
    }
}

