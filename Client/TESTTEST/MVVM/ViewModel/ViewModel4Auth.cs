using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TESTTEST.Core;
using TESTTEST.MVVM.Model;
using TESTTEST.MVVM.View;
using TESTTEST.Request;
using TESTTEST.Response;
using TESTTEST.Validations.ValidClasses;

namespace TESTTEST.MVVM.ViewModel
{
    class ViewModel4Auth : INotifyPropertyChanged
    {
        private AuthorizationWindow AuthorizationWindow;

        private RelayCommand regCommand;
        private RelayCommand enterToAccount;
        public RelayCommand EnterToAccount
        {
            get
            {
                return enterToAccount ??
                    (enterToAccount = new RelayCommand(async (o) =>
                    {
                        AuthorizationWindow.AuthButton.IsEnabled = false;
                        string Login = AuthorizationWindow.TextBoxLogin.Text;
                        string Passwd = AuthorizationWindow.Passbox1.Password;
                        Account account = new Account { Name = Login, PassWord = Passwd };
                        AuthWindowValidation authWindowValidation = new AuthWindowValidation();
                        if (authWindowValidation.EnterValidation(AuthorizationWindow.TextBoxLogin, AuthorizationWindow.Passbox1, account) == true)
                        {
                            try
                            {
                                AuthenticationResponse JwtHolder = await Requests.AuthentificateAsync(Login, Passwd);
                                if (JwtHolder.State == AuthState.AuthenticationSuccesfull)
                                {
                                    AuthorizationWindow.Passbox1.ToolTip = "";
                                    AuthorizationWindow.Passbox1.Background = Brushes.Transparent;
                                    User user = await Requests.Get_User(JwtHolder);
                                    if (user != null)
                                    {
                                        MainWindow mainWindow = new MainWindow(user, JwtHolder);
                                        mainWindow.Show();
                                        AuthorizationWindow.Close();
                                    }
                                    else
                                    {

                                    }


                                }
                                else if (JwtHolder.State == AuthState.AuthenticationFailedWrongName)
                                {
                                    AuthorizationWindow.AuthButton.IsEnabled = true;
                                    AuthorizationWindow.TextBoxLogin.Background = Brushes.RosyBrown;
                                    AuthorizationWindow.TextBoxLogin.ToolTip = "AuthenticationFailedWrongName";
                                }
                                else if (JwtHolder.State == AuthState.AuthenticationFailedWrongPasswd)
                                {
                                    AuthorizationWindow.AuthButton.IsEnabled = true;
                                    AuthorizationWindow.Passbox1.ToolTip = "AuthenticationFailedWrongPasswd";
                                    AuthorizationWindow.Passbox1.Background = Brushes.RosyBrown;
                                }  
                                else
                                {
                                    MessageBox.Show("Something went wrong");
                                }
                            }            
                               
                         catch (Exception ex)
                            {
                                AuthorizationWindow.AuthButton.IsEnabled = true;
                                if (ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                                {
                                    MessageBox.Show("Подключение не установлено");
                                }
                                return;
                            }



                        }

                    }
                    ));
            }
        }
      
        public RelayCommand RegCommand
        {
            get
            {
                return regCommand ??
                    (regCommand = new RelayCommand((o) =>
                    {
                        RegistrationWindow regWindow = new RegistrationWindow();
                        regWindow.Show();
                        AuthorizationWindow.Close();
                    }
                    ));
            }
        }
        public ViewModel4Auth(AuthorizationWindow AuthWindow)
        {
            AuthorizationWindow = AuthWindow;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
