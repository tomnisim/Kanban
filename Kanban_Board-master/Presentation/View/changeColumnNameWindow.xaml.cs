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
    /// Interaction logic for changeColumnNameWindow.xaml
    /// </summary>
    public partial class changeColumnNameWindow : Window
    {
        public string Name;
        private ColumnModel column;
        private BoardModel board;
        private UserModel user;
        private BackendController controller;
        private changeColumnNameViewModel changeColumnNameVM;
        private BoardModel backupBoard;
        public changeColumnNameWindow(UserModel user, ColumnModel column,BoardModel backupBoard) //Constructor
        {
            InitializeComponent();
            this.user = user;
            this.column = column;
            this.controller = user.Controller;
            this.changeColumnNameVM = new changeColumnNameViewModel(user.Controller);
            this.DataContext = changeColumnNameVM;
            this.backupBoard = backupBoard;
        }
        private void changeColumnName_Click(object sender, RoutedEventArgs e) //ChangeColumnName button was clicked
        {
            BoardModel board = new BoardModel(controller, user);
            if(backupBoard==null) //Currently there are no filtered tasks
            {
                if (changeColumnNameVM.changeColumnName(board, column,null)) //null because there is no backup board to update with changes
                    this.Close();
            }
            else
            {
                if (changeColumnNameVM.changeColumnName(board, column,backupBoard.getColumn(column.Ordinal))) //Send also the column from the backup board to update with changes
                    this.Close();
            }
           
        }
    }
}