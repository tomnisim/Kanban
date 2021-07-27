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
    /// Interaction logic for addTaskWindow.xaml
    /// </summary>
    public partial class addTaskWindow : Window
    {
        private addTaskViewModel addTaskVM ;
        private BoardModel board;
        private BoardModel BackupBoard;

        public addTaskWindow(BoardModel board,BoardModel BackupBoard) //Constructor
        {
            InitializeComponent();
            addTaskVM = new addTaskViewModel(board);
            this.DataContext = addTaskVM;
            this.board = board;
            this.BackupBoard = BackupBoard;
        }
        private void addTask_Click(object sender, RoutedEventArgs e) //Add Task button clicked
        {
            bool isAdded; 
            if (BackupBoard!=null) //if currently tasks are filtered
            {
                
                isAdded=addTaskVM.addTask(board.getColumn(0), BackupBoard.getColumn(0)); // Send columns from both board and backupboard
            }
            else
            {
                isAdded=addTaskVM.addTask(board.getColumn(0), null); //send only column from board and not from backup board because currently tasks are not filtered
            }
            if(isAdded)    // only close the window if action was succesfull
                this.Close();

        }
    }
}
