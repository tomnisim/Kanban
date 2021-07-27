using Presentation.Model;
using Presentation.ViewModel;
using IntroSE.Kanban.Backend.ServiceLayer;
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
    /// Interaction logic for addColumnWindow.xaml
    /// </summary>
    public partial class addColumnWindow : Window
    {

        private addColumnViewModel addColumnVM;
        private BoardModel board;
        private BoardModel BackupBoard;

        public addColumnWindow(BoardModel board,BoardModel BackupBoard) //Constructor
        {
            InitializeComponent();
            addColumnVM = new addColumnViewModel(board);
            this.DataContext = addColumnVM;
            this.board = board;
            this.BackupBoard = BackupBoard;
        }


        private void addColumn_Click(object sender, RoutedEventArgs e) //Add Column button clicked
        {
            ColumnModel column = addColumnVM.addColumn();
            if (column != null) //if addColumn was succesfull
            {
                this.board.Columns.Insert(column.Ordinal, column); //Updating the Presentation board
                if (this.BackupBoard != null) //Only update the backup board if it exists - meaning currently tasks are filtered
                    this.BackupBoard.Columns.Insert(column.Ordinal, column);
                this.Close();
            }


        }
    }
}