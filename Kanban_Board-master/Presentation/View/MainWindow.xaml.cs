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
using System.Data.SQLite;
using log4net;
using Presentation.ViewModel;
using Presentation.View;
using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainWindowViewModel vm;
        public MainWindow() //Constructor
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            this.DataContext = vm;
        }
        private void Login_Click(object sender, RoutedEventArgs e) //Login button was clicked
        {
            LoginWindow login = new LoginWindow(vm.Controller);
            login.Show();
            this.Close();
  
        }
        private void Register_Click(object sender, RoutedEventArgs e) //Register button was clicked
        {
            RegisterWindow reg = new RegisterWindow(vm);
            reg.Show();
            this.Close();

        }
    }
}
