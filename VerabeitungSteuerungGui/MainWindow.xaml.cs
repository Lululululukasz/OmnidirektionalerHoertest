using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Rug.Osc;
using System.ComponentModel;

namespace VerabeitungSteuerungGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OscSender sender;
        Thread netThread;
        private static Rug.Osc.OscReceiver receiver;
        private static Object _lock;

        public MainWindow()
        {
            sender = new OscSender(IPAddress.Parse("127.0.0.1"), 9000);
            sender.Connect();
            netThread = new Thread(new ThreadStart(OscReceiverThread));
            _lock = new Object();
            receiver = new Rug.Osc.OscReceiver(10000);
            receiver.Connect();
            netThread.Start();
            Trace.WriteLine("Thread Started!");
            InitializeComponent();       
        }

        private void Button_Click_StartTest(object sender, RoutedEventArgs e)
        {
            this.sender.Send(new OscMessage("/I1-2/mouse/click", 1));
        }

        private void Button_Click_StartCallibration(object sender, RoutedEventArgs e)
        {
            this.sender.Send(new OscMessage("/I1-2/mouse/click", 2));
        }

        private void Button_Click_StopTest(object sender, RoutedEventArgs e)
        {
            this.sender.Send(new OscMessage("/I1-2/mouse/click", 3));
        }
        private void Button_ClearTestLog(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Text = "";
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.sender.Close();
            receiver.Close();
            
        }
        private void OscReceiverThread()
        {
            try
            {
                while (receiver.State != OscSocketState.Closed)
                {

                    if (receiver.State == OscSocketState.Connected)
                    {

                        OscPacket packet = receiver.Receive();
                        //ThreadSafe packet zur liste hinzufügen
                        lock (_lock)
                        {
                            string data = packet.ToString();
                            string adress = data.Split(",")[0];
                            string value = data.Split(",")[1];
                            string testId = adress.Split("/")[3];
                            Trace.WriteLine("Got Data!");
                            this.Dispatcher.Invoke(() => {
                                OutputTextBox.Text += testId + "  " + value + "\n";
                            });
                            
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
