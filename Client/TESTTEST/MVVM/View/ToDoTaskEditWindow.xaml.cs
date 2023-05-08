using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using TESTTEST.MVVM.Model;
using TESTTEST.Validations;

namespace TESTTEST.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public ToDoTask ToDoTaskForEdition { get; private set; }
        public EditWindow(ToDoTask toDoTask)
        {
            InitializeComponent();
            ToDoTaskForEdition = toDoTask;
            StatusButton.IsChecked = toDoTask.IsDone;
            this.DataContext = ToDoTaskForEdition;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ToDoTaskEditWindowValidation editWindowValidation = new ToDoTaskEditWindowValidation();
            if ( editWindowValidation.Window_Validation(Created_Content, Created_Name,ToDoTaskForEdition) == true)
            {
                this.DialogResult = true;
            }
            else
            {
                return;
            }
                 
        }
        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Status_Click(object sender, RoutedEventArgs e)
        {
            if (StatusButton.IsChecked == true)
            {
                ToDoTaskForEdition.IsDone = true;
            }
            else
            {
                ToDoTaskForEdition.IsDone = false;
            }
        }
    }
}
