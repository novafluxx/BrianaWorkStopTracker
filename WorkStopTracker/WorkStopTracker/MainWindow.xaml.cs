using System;
using System.Collections.Generic;
using System.IO;
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

namespace WorkStopTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private DateTime startTime = new DateTime();
        private string filename = @"timelogfile.csv";

        private async void StopLogBtn_Click(object sender, RoutedEventArgs e)
        {
            UnicodeEncoding uniencoding = new UnicodeEncoding();

            var endTime = DateTime.Now;

            TimeSpan span = endTime.Subtract(startTime);

            var stringToWrite = $"{startTime.ToString()},{endTime.ToString()},{span.Seconds}\n";

            byte[] bytesToWrite = uniencoding.GetBytes(stringToWrite);

            using (FileStream fs = File.Open(filename, FileMode.OpenOrCreate))
            {
                fs.Seek(0, SeekOrigin.End);
                await fs.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
            }

            StopLogBtn.IsEnabled = false;
            startLogBtn.IsEnabled = true;
        }

        private async void StartLogBtn_Click(object sender, RoutedEventArgs e)
        {
            // Check if the log file already exists.
            // If it does not create it and write the headers.
            if (!File.Exists(filename))
            {
                UnicodeEncoding uniencoding = new UnicodeEncoding();
                var stringToWrite = $"Start Time,Stop Time,Time Spent (Seconds)\n";

                byte[] bytesToWrite = uniencoding.GetBytes(stringToWrite);

                using (FileStream fs = File.Open(@"timelogfile.csv", FileMode.OpenOrCreate))
                {
                    await fs.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
                }
            }

            startTime = DateTime.Now;
            startLogBtn.IsEnabled = false;
            StopLogBtn.IsEnabled = true;
        }
    }
}
