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
using TESTTEST.MVVM.Model;
using TESTTEST.Validations;

namespace TESTTEST.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public ToDoTask TodoTaskToAdd { get; private set; }
        public TaskWindow(ToDoTask toDoTask)
        {
            InitializeComponent();
            TodoTaskToAdd = toDoTask;
            this.DataContext = TodoTaskToAdd;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ToDoTaskAddWindowValidation toDoTaskAddWindowValidation = new ToDoTaskAddWindowValidation();
            if(toDoTaskAddWindowValidation.Window_Validation(Created_Content,Created_Name, TodoTaskToAdd)  == true)
            {
                TodoTaskToAdd.CreationDate = DateTime.Now;
                TodoTaskToAdd.IsDone = false;
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
    }
}
