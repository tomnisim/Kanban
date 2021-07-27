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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel loginVM;
        BackendController controller;
        public LoginWindow(BackendController controller) //Constructor
        {
            this.controller = controller;
            InitializeComponent();
            loginVM = new LoginViewModel(controller);
            this.DataContext = loginVM;
        }
       
        private void Login_Click(object sender, RoutedEventArgs e) //Login button was clicked
        {
            UserModel user = loginVM.Login();
            if (user != null) //Only change windows if login was succesfull
            {

                ShowBoardWindow board = new ShowBoardWindow(user);
                board.Show();
                this.Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) //Back button was clicked - returing to the MainWindow (Login/Register)
        {
            MainWindow main = new MainWindow(); //Open the main window of the app (register/login)
            main.Show();
            this.Close();
        }
    }
}
