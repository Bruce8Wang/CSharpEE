using System;
using System.Windows;

namespace Wpf_Demo
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new Application {StartupUri = new Uri("MainWindow.xaml", UriKind.Relative)};
            app.Run();
        }
    }
}