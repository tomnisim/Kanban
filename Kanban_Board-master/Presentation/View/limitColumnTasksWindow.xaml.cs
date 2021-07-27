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
    /// Interaction logic for limitColumnTasksWindow.xaml
    /// </summary>
    public partial class limitColumnTasksWindow : Window
    {
        private limitColumnTasksViewModel limitVM;
        private ColumnModel column;
        private UserModel user;
        private ColumnModel backupColumn;

        public limitColumnTasksWindow(UserModel user, ColumnModel column,ColumnModel backupColumn) //Constructor
        {
            InitializeComponent();
            this.user = user;
            this.column = column;
            this.backupColumn = backupColumn;
            limitVM = new limitColumnTasksViewModel(user.Controller, user, column,backupColumn);
            this.DataContext = limitVM;
        }

        private void OK_Click(object sender, RoutedEventArgs e) //Ok button was clicked
        {
            
            if(limitVM.limitColumn())
                this.Close();

        }
    }
}