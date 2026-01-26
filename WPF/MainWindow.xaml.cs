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
using WPF.User_Control;

namespace WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var home = new Home();
            LoadPanel.Child = home;
        }


        private void ClickHomeBtn(object sender, RoutedEventArgs e)
        {
            var home = new Home();
            LoadPanel.Child = home;
        }

        private void ClickBookBtn(object sender, RoutedEventArgs e)
        {
            var book = new Books();
            LoadPanel.Child = book;
        }

        private void ClickMemberBtn(object sender, RoutedEventArgs e)
        {
            var member = new Member();
            LoadPanel.Child = member;
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e) { 
            WindowState = WindowState.Minimized; 
        }
        private void CloseWindow(object sender, RoutedEventArgs e) { 
            Close(); 
        }

        private void ClickRecordsBtn(object sender, RoutedEventArgs e)
        {
            var record = new Reccords();
            LoadPanel.Child = record;
        }
    }
}