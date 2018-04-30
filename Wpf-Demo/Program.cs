using System;
using System.Windows;

namespace Wpf_Demo
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var app = new Application {StartupUri = new Uri("MainWindow.xaml", UriKind.Relative)};
            app.Run();
        }
    }
}