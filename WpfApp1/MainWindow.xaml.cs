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
        // déclarer une liste pour le management des processus
        private LinkedList<ProcessNode> listProcess;

        public MainWindow()
        {
            InitializeComponent();
            // initialiser cette liste
            this.listProcess = new LinkedList<ProcessNode>();
        }
        // mise à jour les comptuers
        private void setViewCounters(int ballonCount, int premierCount)
        {
            ballonCountView.Text = ballonCount.ToString();
            premierCountView.Text = premierCount.ToString();
            countView.Text = (ballonCount + premierCount).ToString();
        }
        // arrêter tous les processus
        private void stopAllProcess()
        {
            foreach (ProcessNode pN in listProcess)
            {
                pN.process.Kill();
                lvProcess.Items.Remove(pN.processViewItem);
            }
            // initialiser les compteurs
            ProcessNode.setBallonCount(0);
            ProcessNode.setPremierCount(0);
            listProcess = new LinkedList<ProcessNode>();
            setViewCounters(0, 0);
        }
        // handler du éventement lorsque l'utilisateur clique le croix rouge
        private void process_Exited(object sender, System.EventArgs e)
        {
            // modifier l'état d'un autre processus en utilisant Dispatcher.Invoke
            this.Dispatcher.Invoke(() =>
            {
                checkClosedProcess();
            });

        }
        // mise à jour la listView lorsque l'utilisateur arrête un fênetre
        private void checkClosedProcess()
        {
            foreach (ProcessNode pN in listProcess)
            {
                if (pN.process.HasExited)
                {
                    lvProcess.Items.Remove(pN.processViewItem);
                    // mise à jour les compteurs correspondants
                    if (pN.name == "ballon")
                        ProcessNode.setBallonCount(ProcessNode.getBallonCount() - 1);
                    else
                        ProcessNode.setPremierCount(ProcessNode.getPremierCount() - 1);
                    setViewCounters(ProcessNode.getBallonCount(), ProcessNode.getPremierCount());
                    listProcess.Remove(pN);
                    break;
                }
                
            }
        }
        // fonction qui va être appelé lorsque l'utilisateur clique start ballon
        private void startBallon_Click(object sender, RoutedEventArgs e)
        {
            // if linkedlist is vide, then avoid check the class variable of count
            if (listProcess.Count == 0 || ProcessNode.getBallonCount() < 5)
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("Ballon.exe");
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(process_Exited);
                p.Start();
                int pid = p.Id;
                ProcessItem item = new ProcessItem("ballon.exe", pid);
                listProcess.AddLast(new ProcessNode(p, "ballon", item));
                lvProcess.Items.Add(item);
                setViewCounters(ProcessNode.getBallonCount(), ProcessNode.getPremierCount());
            }
            else
                MessageBox.Show("Cannot create more than 5 processes", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // fonction qui va être appelé lorsque l'utilisateur clique start premier
        private void startPremier_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count == 0 || ProcessNode.getPremierCount() < 5)
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("premier.exe");
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(process_Exited);
                p.Start();
                int pid = p.Id;
                ProcessItem item = new ProcessItem("premier.exe", pid);
                listProcess.AddLast(new ProcessNode(p, "premier", item));
                lvProcess.Items.Add(item);
                setViewCounters(ProcessNode.getBallonCount(), ProcessNode.getPremierCount());
            }
            else
                MessageBox.Show("Cannot create more than 5 processes", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // fonction qui va être appelé lorsque l'utilisateur clique stop all process
        private void StopAllProcess_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0)
                MessageBox.Show("No running process", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                stopAllProcess();
            
        }
        // fonction qui va être appelé lorsque l'utilisateur clique stop last process
        private void StopLastProcess_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0)
                MessageBox.Show("No running process", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                listProcess.Last().process.Kill();
                lvProcess.Items.Remove(listProcess.Last().processViewItem);
                if (listProcess.Last().name == "ballon")
                    ProcessNode.setBallonCount(ProcessNode.getBallonCount() - 1);
                else
                    ProcessNode.setPremierCount(ProcessNode.getPremierCount() - 1);
                setViewCounters(ProcessNode.getBallonCount(), ProcessNode.getPremierCount());
                listProcess.RemoveLast();
            }
        }
        // fonction qui va être appelé lorsque l'utilisateur clique stop last premier
        private void StopLastPremier_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0 || ProcessNode.getPremierCount() <= 0)
                MessageBox.Show("No running process for premier.exe", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                for (int i = listProcess.Count -1; i >=0; i--)
                {
                    if (listProcess.ElementAt(i).name == "premier")
                    {
                        // obtenir le dernier élément de type premier de la liste listProcess
                        ProcessNode toRemove = listProcess.ElementAt(i);
                        toRemove.process.Kill();
                        lvProcess.Items.Remove(toRemove.processViewItem);
                        ProcessNode.setPremierCount(ProcessNode.getPremierCount() - 1);
                        setViewCounters(ProcessNode.getBallonCount(), ProcessNode.getPremierCount());
                        listProcess.Remove(toRemove);
                        break;
                    }
                }
            }
        }
        // fonction qui va être appelé lorsque l'utilisateur clique stop last ballon
        private void stopLastBallon_Click(object sender, RoutedEventArgs e)
        {
            if (listProcess.Count <= 0 || ProcessNode.getBallonCount() <= 0)
                MessageBox.Show("No running process for ballon.exe", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                for (int i = listProcess.Count - 1; i >= 0; i--)
                {
                    if (listProcess.ElementAt(i).name == "ballon")
                    {
                        // obtenir le dernier élément de type ballon de la liste listProcess
                        ProcessNode toRemove = listProcess.ElementAt(i);
                        toRemove.process.Kill();
                        lvProcess.Items.Remove(toRemove.processViewItem);
                        ProcessNode.setBallonCount(ProcessNode.getBallonCount() - 1);
                        setViewCounters(ProcessNode.getBallonCount(), ProcessNode.getPremierCount());
                        listProcess.Remove(toRemove);
                        break;
                    }
                }
            }
        }
        // fonction qui va être appelé lorsque l'utilisateur clique le croix rouge de la fênetre principale
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want quit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                stopAllProcess();
            else
                e.Cancel = true;
        }
        // fonction qui va être appelé lorsque l'utilisateur clique le menu quit
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    // la classe définit l'élément stocké dans la listView
    public class ProcessItem
    {
        public ProcessItem(string ProcessName, int PID)
        {
            this.ProcessName = ProcessName;
            this.PID = PID;
        }
        public string ProcessName { get; set; }
        public int PID { get; set; }
    }
    // la classe définit l'élément stocké dans la liste listProcess
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
                ballonCount += 1;
            else
                premierCount += 1;
        }

        public static int getBallonCount()
        {
            return ballonCount;
        }
        public static int getPremierCount()
        {
            return premierCount;
        }
        public static void setBallonCount(int count)
        {
            ballonCount = count;
        }
        public static void setPremierCount(int count)
        {
            premierCount = count;
        }
    }
}

