using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClassroomAlligators
{
    public partial class WindowMain : Window
    {
        private readonly string[] names = { "Abdhy", "Aryo", "Athalia", "Bella", "Daniel", "Delvin", "Dustin", "Evangeline", "Ferren", "Ghoran", "Gladys", "Grace",
                                            "Hans", "Yoga", "Ichsan", "Jacky", "Jacky S", "Jason", "Justin", "Jennifer", "Jesslyn", "Joel", "Karina", "Kezia",
                                            "Marlene", "Marvel", "Matthew", "Ruth", "Ryan", "Satmika", "Shamgar", "Stefanie", "Stephen", "Tristan", "Ziven" };
        public WindowMain() { InitializeComponent(); }
        private void Classroom_Loaded(object s, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized; ClassroomButtonEvent.IsEnabled = false;
            NetworkConnection();
        }
        private void Classroom_MouseLeftButtonDown(object s, MouseButtonEventArgs e) { DragMove(); }
        private void ClassroomButtonExit_Click(object s, RoutedEventArgs e) { Close(); }
        private void ClassroomButtonRandom_Click(object s, RoutedEventArgs e)
        {
            _ = Task.Run(() =>
            {
                int sleep = 1;
                Dispatcher.Invoke(() => { ClassroomButtonRandom.IsEnabled = false; });
                while (sleep < 1000)
                {
                    string[] random = names.OrderBy(x => new Random().Next()).ToArray();
                    int current = 0;
                    Dispatcher.Invoke(() =>
                    {
                        foreach (Control child in AllChildren(GridMain))
                        {
                            if (child is Label label && child.Name.Contains("Desk"))
                            {
                                label.Content = current > 34 ? "" : random[current]; current++;
                            }
                        }
                    });
                    Task.Delay(sleep).Wait();
                    sleep += 50;
                }
                Dispatcher.Invoke(() => { ClassroomButtonRandom.IsEnabled = true; });
            });
        }
        private void ClassroomButtonReset_Click(object s, RoutedEventArgs e)
        {
            int current = 1;
            foreach (Control child in AllChildren(GridMain))
            {
                if (child is Label label && child.Name.Contains("Desk"))
                {
                    label.Content = current.ToString("00"); current++;
                }
            }
        }
        private void ClassroomButtonGroup_Click(object s, RoutedEventArgs e)
        {
            _ = Task.Run(() =>
            {
                int sleep = 1;
                Dispatcher.Invoke(() => { ClassroomButtonGroup.IsEnabled = false; });
                while (sleep < 1000)
                {
                    string[] random = names.OrderBy(x => new Random().Next()).ToArray();
                    int current = 0, index = 0;
                    Dispatcher.Invoke(() =>
                    {
                        foreach (Control child in AllChildren(GridMain))
                        {
                            if (child is Label label && child.Name.Contains("Desk"))
                            {
                                label.Content = current is < 5 or (> 11 and < 17) ? "" : random[index++];
                                current++;
                            }
                        }
                    });
                    Task.Delay(sleep).Wait();
                    sleep += 50;
                }
                Dispatcher.Invoke(() => { ClassroomButtonGroup.IsEnabled = true; });
            });
        }
        private void ClassroomButtonEvent_Click(object s, RoutedEventArgs e)
        {
            _ = Task.Run(() =>
            {
                double values = 0;
                Dispatcher.Invoke(() => { GridEvents.Visibility = Visibility.Visible; values = GridEvents.Opacity; });
                while (values < 1)
                {
                    Dispatcher.Invoke(() => { GridEvents.Opacity += 0.03; values = GridEvents.Opacity; });
                    Task.Delay(30).Wait();
                }
                ReadDatabase();
            });
        }
        private void ButtonEventsClose_Click(object s, RoutedEventArgs e)
        {
            _ = Task.Run(() =>
            {
                double values = 0;
                Dispatcher.Invoke(() => { values = GridEvents.Opacity; });
                while (values > 0)
                {
                    Dispatcher.Invoke(() => { GridEvents.Opacity -= 0.03; values = GridEvents.Opacity; });
                    Task.Delay(30).Wait();
                }
                Dispatcher.Invoke(() => { GridEvents.Visibility = Visibility.Hidden; });
            });
        }
        private void NetworkConnection()
        {
            _ = Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(2000).Wait();
                    List<NetworkInterface> networks = NetworkInterface.GetAllNetworkInterfaces().Where(x => x.OperationalStatus == OperationalStatus.Up).ToList();
                    if (networks.Count > 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ClassroomButtonEvent.IsEnabled = true;
                            RegionNetworkConnection.Style = (Style)FindResource("LabelSuccess");
                            string ssid = "BLI Sentul";
                            RegionNetworkConnection.Content = $"Connected to {ssid}";
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ClassroomButtonEvent.IsEnabled = false;
                            RegionNetworkConnection.Style = (Style)FindResource("LabelDanger");
                            RegionNetworkConnection.Content = "No internet connection. Please check your network!";
                        });
                    }
                }
            });
        }
        private static void ReadDatabase()
        {
            _ = Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(10000).Wait();

                    return;
                }
            });
        }
        private List<Control> AllChildren(DependencyObject parent)
        {
            List<Control> list = new() { };
            for (int count = 0; count < VisualTreeHelper.GetChildrenCount(parent); count++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, count);
                if (child is Control control) { list.Add(control); }
                list.AddRange(AllChildren(child));
            }
            return list;
        }
    }
}