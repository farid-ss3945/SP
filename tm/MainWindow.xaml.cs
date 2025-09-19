﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WpfApp1;

public partial class MainWindow : Window
{
        public MainWindow()
        {
                InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
                ProcessIds.Items.Clear();
                ProcessNames.Items.Clear();
                foreach (var i in Process.GetProcesses())
                {
                        try
                        {
                                var proc = new ProcessData();
                                proc.Id = i.Id;
                                proc.Name = i.ProcessName;
                                ProcessIds.Items.Add(proc.Id);
                                ProcessNames.Items.Add(proc.Name);
                        }
                        catch (UnauthorizedAccessException)
                        {
                        }
                }


        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
                string a = new string("");
                string b = new string("");
                a += Process_name.Text;
                b += Process_priority.Text;
                foreach (var i in Process.GetProcesses())
                {
                        if (i.ProcessName == a)
                        {
                                if (b.Trim() == "realtime")
                                {
                                        i.PriorityClass = ProcessPriorityClass.RealTime;
                                }
                                else if (b.Trim() == "high")
                                {
                                        i.PriorityClass = ProcessPriorityClass.High;
                                }
                                else if (b.Trim() == "above normal")
                                {
                                        i.PriorityClass = ProcessPriorityClass.AboveNormal;
                                }
                                else if (b.Trim() == "normal")
                                {
                                        i.PriorityClass = ProcessPriorityClass.Normal;
                                }
                                else if (b.Trim() == "below normal")
                                {
                                        i.PriorityClass = ProcessPriorityClass.BelowNormal;
                                }

                                // else if (b=="low")
                                // {
                                //         i.PriorityClass = ProcessPriorityClass.
                                // }
                                // no Low was found;
                                break;
                        }
                }


        }

        private void Button_OnClick1(object sender, RoutedEventArgs e)
        {
                string a = new string("");
                string b = new string("");
                a += Process_name.Text;
                b += Process_priority.Text;
                foreach (var i in Process.GetProcesses())
                {
                        if (i.ProcessName == a)
                        {
                                i.Kill();
                                
                                break;
                        }
                }
                
                //throw new NotImplementedException();
        }

        public Exception CancellationToken { get; set; }
}