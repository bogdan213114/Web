using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    class ViewModel4Reg : INotifyPropertyChanged
    {
        private RegistrationWindow RegistrationWindow;
        private RelayCommand enterCommand;
        private RelayCommand registrateAccount;
        public RelayCommand RegistrateAccount
        {
            get
            {
                return registrateAccount ??
                    (registrateAccount = new RelayCommand(async (o) =>
                    {
                        RegistrationWindow.RegButton.IsEnabled = false;
                        string Login = RegistrationWindow.TextBoxLogin.Text;
                        string Passwd = RegistrationWindow.Passbox1.Password;
                        string PasswdConf = RegistrationWindow.Passbox2.Password;
                       if(PasswdConf == Passwd)
                        {
                            RegistrationWindow.Passbox2.ToolTip = "";
                            RegistrationWindow.Passbox2.Background = Brushes.Transparent;
                            Account account = new Account { Name = Login, PassWord = Passwd };
                            
                            AuthWindowValidation authWindowValidation = new AuthWindowValidation();
                            if (authWindowValidation.EnterValidation(RegistrationWindow.TextBoxLogin, RegistrationWindow.Passbox1, account) == true)
                            {

                                try
                                {
                                    AuthenticationResponse JwtHolder = await Requests.RegistrateAsync(Login, Passwd);

                                    if (JwtHolder.State == AuthState.RegistrationSuccesfull)
                                    {
                                        RegistrationWindow.TextBoxLogin.Background = Brushes.Transparent;
                                        RegistrationWindow.TextBoxLogin.ToolTip = "";
                                        User user = await Requests.Get_User(JwtHolder);
                                        MainWindow mainWindow = new MainWindow(user, JwtHolder);
                                        mainWindow.Show();
                                        RegistrationWindow.Close();

                                    }
                                    else if (JwtHolder.State == AuthState.RegistrationFailedUsernameTaken)
                                    {
                                        RegistrationWindow.RegButton.IsEnabled = true;
                                        RegistrationWindow.TextBoxLogin.Background = Brushes.RosyBrown;
                                        RegistrationWindow.TextBoxLogin.ToolTip = "RegistrationFailedUsernameTaken";
                                    }
                                    else
                                    {
                                        RegistrationWindow.RegButton.IsEnabled = true;
                                        MessageBox.Show("Something went wrong");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    RegistrationWindow.RegButton.IsEnabled = true;
                                    if (ex.Message == "Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:44302)")
                                    {
                                        MessageBox.Show("Подключение не установлено");
                                    }
                                    return;
                                }
                               

                            }
                            else
                            {
                                RegistrationWindow.RegButton.IsEnabled = true;
                            }
                        }
                        else
                        {
                            RegistrationWindow.RegButton.IsEnabled = true;
                            RegistrationWindow.Passbox1.ToolTip = "";
                            RegistrationWindow.Passbox1.Background = Brushes.Transparent;
                            RegistrationWindow.Passbox2.ToolTip = "Passwords are not equal" ;
                            RegistrationWindow.Passbox2.Background = Brushes.RosyBrown;
                        }  
                    }
                    ));
            }
        }
        public RelayCommand EnterCommand
        {
            get
            {
                return enterCommand ??
                    (enterCommand = new RelayCommand((o) =>
                    {
                        AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                        authorizationWindow.Show();
                        RegistrationWindow.Close();
                    }
                    ));
            }
        }
    public  ViewModel4Reg(RegistrationWindow registrationWindow)
        {
            RegistrationWindow = registrationWindow;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
