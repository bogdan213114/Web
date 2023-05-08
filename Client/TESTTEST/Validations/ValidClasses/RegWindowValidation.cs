using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TESTTEST.MVVM.Model;
using TESTTEST.Validations.Interfaces;

namespace TESTTEST.Validations.ValidClasses
{
     class RegWindowValidation : IEnterWindowValidation
    {
        public bool EnterValidation(TextBox textBox, PasswordBox passBox, Account account)
        {
            int SwitherForName = 0;
            int SwitherForPassword = 0;
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(account);
            if (!Validator.TryValidateObject(account, context, results, true))
            {
                foreach (var error in results)
                {
                   
                    switch (error.ErrorMessage)
                    {
                        case "Non entered Name":
                            textBox.ToolTip = error.ErrorMessage;
                            textBox.Background = Brushes.RosyBrown;
                            break;
                        case "Non entered Password":
                            passBox.ToolTip = error.ErrorMessage;
                            passBox.Background = Brushes.RosyBrown;
                            break;
                        case "Name Lenght must be in space between 3-20":
                            textBox.ToolTip = error.ErrorMessage;
                            textBox.Background = Brushes.RosyBrown;
                            break;
                        case "PassWord Lenght must be in space between 6-16":
                            passBox.ToolTip = error.ErrorMessage;
                            passBox.Background = Brushes.RosyBrown;
                            break;
                        case "Name contains only letters, numbers, and _$!":
                            textBox.ToolTip = error.ErrorMessage;
                            textBox.Background = Brushes.RosyBrown;
                            break;
                        case "Password  contains only letters, numbers, and _$!":
                            passBox.ToolTip = error.ErrorMessage;
                            passBox.Background = Brushes.RosyBrown;
                            break;
                        case "Password  contains at least one capitalized letter":
                            passBox.ToolTip = error.ErrorMessage;
                            passBox.Background = Brushes.RosyBrown;
                            break;
                        case "Password  contains at least one number":
                            passBox.ToolTip = error.ErrorMessage;
                            passBox.Background = Brushes.RosyBrown;
                            break;
                    }
                    if (error.ErrorMessage.Contains("Name"))
                    {
                        SwitherForName = 1;
                    }
                    if (error.ErrorMessage.Contains("Password"))
                    {
                        SwitherForPassword = 1;
                    }
                }
                if (SwitherForName == 0)
                {
                    textBox.ToolTip = "";
                    textBox.Background = Brushes.Transparent;
                }
                if (SwitherForPassword == 0)
                {
                    passBox.ToolTip = "";
                    passBox.Background = Brushes.Transparent;
                }
                return false;
            }
            else
            {
                textBox.ToolTip = "";
                textBox.Background = Brushes.Transparent;
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;
                return true;
            }
        }
    }
}
