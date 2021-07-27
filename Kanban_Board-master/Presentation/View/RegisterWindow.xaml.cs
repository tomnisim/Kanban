using Presentation.Model;
using Presentation.ViewModel;
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
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        RegisterViewModel reg;
        MainWindowViewModel vm;
        public RegisterWindow(MainWindowViewModel vm) //Constructor
        {
            this.vm = vm;
            InitializeComponent();
            reg = new RegisterViewModel(vm.Controller);
            this.DataContext = reg;
        }

        private void Register_Click(object sender, RoutedEventArgs e) //Register button was clicked
        {
            UserModel user = reg.Register();
            if (user!=null) //Only open the board window if the Register was sucessful
            {
                ShowBoardWindow board = new ShowBoardWindow(user);
                board.Show();
                this.Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) //Back button was clicked - returing to the MainWindow (Login/Register)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
