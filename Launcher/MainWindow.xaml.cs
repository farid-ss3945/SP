using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Launcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var processes = Process.GetProcessesByName("Launcher");
            if (processes.Length > 1)
            {
                MessageBox.Show("Лаунчер уже запущен!");
                Application.Current.Shutdown();
            }
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            string operation = OperationBox.Text;
            string a = Box_1.Text;
            string b = Box_2.Text;
            string debug = DebugCheck.IsChecked == true ? "true" : "false";
            string logfile = LogFileBox.Text;

            string consolePath = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"C:\Users\MSI1\RiderProjects\WpfApp4\ConsoleApp1\bin\Debug\net9.0\ConsoleApp1.exe")
            );

            if (!File.Exists(consolePath))
            {
                MessageBox.Show($"ConsoleApp.exe не найден по пути:\n{consolePath}");
                return;
            }

            Process.Start(consolePath, $"{operation} {a} {b} {debug} {logfile}");
            Application.Current.Shutdown();
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}