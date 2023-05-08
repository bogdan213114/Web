using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CreateNewList.xaml
    /// </summary>
    public partial class CreateNewList : Window
    {
        public TaskGroup NewTaskGroup = new TaskGroup() { Tasks = new ObservableCollection<ToDoTask>() };
        public CreateNewList(User user)
        {
            InitializeComponent();
        
        DataContext = new CreateNewListViewModel(user, NewTaskGroup);
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SaveList_Click(object sender, RoutedEventArgs e)
        {
           

            TaskListAddWindowValidation taskListEditWindowValidation = new TaskListAddWindowValidation();
            if (taskListEditWindowValidation.Window_Validation(TaskListName, NewTaskGroup) == true)
            {
                NewTaskGroup.CreationDate = DateTime.Now;
                this.DialogResult = true;
            }
            else
            {
                return;
            }
        }

        
    }
}
