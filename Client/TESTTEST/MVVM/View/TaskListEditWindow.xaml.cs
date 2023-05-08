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
using TESTTEST.MVVM.ViewModel;
using TESTTEST.Validations.ValidClasses;

namespace TESTTEST.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для EditTaskListWindow.xaml
    /// </summary>
    public partial class TaskListEditWindow : Window
    {
      public  TaskGroup TaskGroupForEdition { get; set;}
        public TaskListEditWindow(User user, TaskGroup taskGroup)
        {
            InitializeComponent();
            TaskGroupForEdition = taskGroup;
            DataContext = new CreateNewListViewModel(this, user);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SaveList_Click(object sender, RoutedEventArgs e)
        {
            TaskListEditWindowValidation taskListEditWindowValidation = new TaskListEditWindowValidation();
            if (taskListEditWindowValidation.Window_Validation(TaskListName, TaskGroupForEdition) == true)
            {
                this.DialogResult = true;
            }
            else
            {
                return;
            }
            
        }
    }
}
