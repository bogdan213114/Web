using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TESTTEST.Core;
using TESTTEST.MVVM.Model;
using TESTTEST.MVVM.View;
using TESTTEST.Request;
using TESTTEST.Response;

namespace TESTTEST.MVVM.ViewModel
{
 public class SelectedTaskGroupViewModel : INotifyPropertyChanged
    {
        MainWindow Mainwindow { get; set; }
        public User User { get; set; }
        public TaskGroup SelectedTaskGroup { get;  set; }
        SelectedTaskGroupWindow SelectedTaskGroupWindow;
        private ToDoTask selectedtask;
        private RelayCommand backToMainWindow;
        private RelayCommand delete;
        private RelayCommand editList;
        public RelayCommand EditTaskList
        {
            get
            {
                return editList ??
                    (editList = new RelayCommand(async (o) =>
                    {
                        TaskGroup TaskGroupTemplate = new TaskGroup() { Title = SelectedTaskGroup.Title, Tasks = new ObservableCollection<ToDoTask>(SelectedTaskGroup.Tasks) };

                        TaskListEditWindow EditTaskListWindow = new TaskListEditWindow(User, TaskGroupTemplate);

                        EditTaskListWindow.ShowDialog();
                        if (EditTaskListWindow.DialogResult == true)
                        {
                            try
                            {
                                UserDataGettingState userDataGettingState = await Requests.EditTaskGroupAsync(SelectedTaskGroup, TaskGroupTemplate, Mainwindow.JwtHolder);
                                if (userDataGettingState == UserDataGettingState.DataEdited)
                                {
                                    User.Groups.FirstOrDefault(item => item.Id == SelectedTaskGroup.Id).Tasks = EditTaskListWindow.TaskGroupForEdition.Tasks;
                                    User.Groups.FirstOrDefault(item => item.Id == SelectedTaskGroup.Id).Title = EditTaskListWindow.TaskGroupForEdition.Title;
                                }
                            }
                            catch (Exception ex)
                            {
                                
                                if (ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                                {
                                    MessageBox.Show("Подключение не установлено");
                                    AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                                    authorizationWindow.Show();
                                    SelectedTaskGroupWindow.Close();
                                    Mainwindow.Close();
                                }
                            }
                        }
                    }
                    ));
            }
        }
        public RelayCommand Delete
        {
            get
            {
                return delete ??
                    (delete = new RelayCommand(async (o) =>
                    {
                        TaskGroup taskGroup = User.Groups.FirstOrDefault(item => item.Id == SelectedTaskGroup.Id);
                        try
                        {
                            UserDataGettingState userDataGettingState = await Requests.DeleteTaskGroupAsync(taskGroup, Mainwindow.JwtHolder);
                            if (userDataGettingState == UserDataGettingState.DataDeleted)
                            {
                                User.Groups.Remove(taskGroup);
                                Mainwindow.Show();
                                SelectedTaskGroupWindow.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            
                            if (ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                            {
                                MessageBox.Show("Подключение не установлено");
                                AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                                authorizationWindow.Show();
                                SelectedTaskGroupWindow.Close();
                                Mainwindow.Close();
                            }
                        }

                    }));
            }
        }
        public RelayCommand BackToMainWindow
        {
            get
            {   
                return backToMainWindow ?? (backToMainWindow = new RelayCommand((o) =>
                {
                    Mainwindow.Show();
                    SelectedTaskGroupWindow.Close();
                }
                ));
            }
        }
        public ToDoTask SelectedTask
        {
            get { return selectedtask; }
            set
            {
                selectedtask = value;
                OnPropertyChanged("SelectedTask");
            }
        }
        public SelectedTaskGroupViewModel(MainWindow windoW, SelectedTaskGroupWindow window, User user, TaskGroup taskGroup)
        {
            Mainwindow = windoW;
            SelectedTaskGroupWindow = window;
            User = user;
            SelectedTaskGroup = taskGroup;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
