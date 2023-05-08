using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
 public class ViewModel : INotifyPropertyChanged
    {
        public AuthenticationResponse JwtHolder { get; set; }
        public  User User { get; set; }
        public MainWindow MainWindow;
        private ToDoTask selectedtask;
        private TaskGroup selectedTaskGroup;
        private RelayCommand addTaskCommand;
        private RelayCommand removeTaskCommand;
        private RelayCommand editTaskCommand;
        private RelayCommand createNewListCommand;
        private RelayCommand taskGroupWindowOpen;
        public RelayCommand TaskGroupWindowOpen
        {
            get
            {
                return taskGroupWindowOpen ??
                    (taskGroupWindowOpen = new RelayCommand((o) =>
                    {
                        TaskGroup taskGroup = o as TaskGroup;
                        if (taskGroup != null)
                        {
                            SelectedTaskGroupWindow selectedTaskGroupWindow = new SelectedTaskGroupWindow(MainWindow,User, taskGroup);
                            selectedTaskGroupWindow.Show();
                            MainWindow.Hide();
                        }         
                    }
                    ));
            }
        }
        public RelayCommand CreateNewListCommand
        {
            get
            {
                return createNewListCommand ??
                    (createNewListCommand = new RelayCommand(async (o) => 
                    {
                        CreateNewList createNewList = new CreateNewList(User);
                        if(createNewList.ShowDialog() == true)
                        {
                           try
                            {
                                createNewList.NewTaskGroup = await Requests.AddTaskGroupAsync(createNewList.NewTaskGroup, JwtHolder);
                                User.Groups.Add(createNewList.NewTaskGroup);
                            }
                            catch (Exception ex)
                            {
                               
                                if(ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                                {
                                    MessageBox.Show("Подключение не установлено");
                                    AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                                    authorizationWindow.Show();
                                    MainWindow.Close();
                                }


                            }
                          
                        }     
                    }
                    ));
             }
        }
        public RelayCommand EditTaskCommand
        {
            get
            {
                return editTaskCommand ??
                    (editTaskCommand = new RelayCommand(async (o) =>
                    {
                        if (SelectedTask == null)
                        {
                            return;
                        }
                        ToDoTask toDoTask = new ToDoTask()
                        {
                            Description = SelectedTask.Description,
                            Title = SelectedTask.Title,
                            IsDone = SelectedTask.IsDone,
                        };
                        EditWindow editWindow = new EditWindow(toDoTask);
                        if (editWindow.ShowDialog() == true)
                        {
                            try
                            {
                                UserDataGettingState userDataGettingState = await Requests.EditTaskAsync(selectedtask, toDoTask, JwtHolder);
                                if (userDataGettingState == UserDataGettingState.DataEdited)
                                {
                                    User.Tasks.FirstOrDefault(item => item.Id == SelectedTask.Id).Title = editWindow.ToDoTaskForEdition.Title;
                                    User.Tasks.FirstOrDefault(item => item.Id == SelectedTask.Id).IsDone = editWindow.ToDoTaskForEdition.IsDone;
                                    User.Tasks.FirstOrDefault(item => item.Id == SelectedTask.Id).Description = editWindow.ToDoTaskForEdition.Description;
                                }
                            }
                             catch (Exception ex)
                            {
                                
                                if (ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                                {
                                    MessageBox.Show("Подключение не установлено");
                                    AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                                    authorizationWindow.Show();
                                    MainWindow.Close();
                                }


                            }

                        }
                    }
                    ));
            }
        }
        public RelayCommand RemoveTaskCommand
        {
            get
            {
                return removeTaskCommand ??
                    (removeTaskCommand = new RelayCommand(async (selectedItem) =>
                    {
                        ToDoTask toDoTask = selectedItem as ToDoTask;
                       
                       
                        if(toDoTask != null)
                        {
                            DeleteWindow deleteWindow = new DeleteWindow(toDoTask);
                            if (deleteWindow.ShowDialog() == true)
                            {
                                try
                                {
                                    UserDataGettingState userDataGettingState = await Requests.DeleteTaskAsync(toDoTask, JwtHolder);
                                    if (userDataGettingState == UserDataGettingState.DataDeleted)
                                    {
                                        User.Tasks.Remove(toDoTask);
                                        var GroupsWithThisTask = User.Groups.Where(item => item.Tasks.Contains(toDoTask));
                                        foreach (var group in GroupsWithThisTask)
                                        {
                                            group.Tasks.Remove(toDoTask);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                    if (ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                                    {
                                        MessageBox.Show("Подключение не установлено");
                                        AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                                        authorizationWindow.Show();
                                        MainWindow.Close();
                                    }
                                    


                                }

                            }
                        }                
                    },
                 (selectedItem) => User.Tasks.Count >= 0));
            }
        }
        public RelayCommand AddTaskCommand
        {
            get
            {
                return addTaskCommand ??
                    (addTaskCommand = new RelayCommand(async (o) =>
                    {
                        TaskWindow taskWindow = new TaskWindow(new ToDoTask());
                        if(taskWindow.ShowDialog() == true)
                        {
                            ToDoTask todotask = taskWindow.TodoTaskToAdd;
                            try
                            {
                                
                                todotask = await Requests.AddTaskAsync(todotask, JwtHolder);
                                User.Tasks.Add(todotask);
                            }
                           
                            catch (Exception ex)
                            {
                                
                                if (ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                                {
                                    MessageBox.Show("Подключение не установлено");
                                    AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                                    authorizationWindow.Show();
                                    MainWindow.Close();
                                }
                                


                            }
                        }
                    } ))  ;
            }
        }

        public TaskGroup SelectedTaskGroup
        {
            get { return selectedTaskGroup; }
            set
            {
                selectedTaskGroup = value;
                OnPropertyChanged("SelectedTaskGroup");
            }
        }
        public ToDoTask SelectedTask
        {
            get { return selectedtask; }
            set { selectedtask = value;
                OnPropertyChanged("SelectedTask");
            }
        }
        public ViewModel(MainWindow mainWindow)
        {
            JwtHolder = mainWindow.JwtHolder;
            MainWindow = mainWindow;
            User = mainWindow.User;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
