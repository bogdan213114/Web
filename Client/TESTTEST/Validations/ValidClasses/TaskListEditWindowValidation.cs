using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TESTTEST.MVVM.Model;

namespace TESTTEST.Validations.ValidClasses
{
  public  class TaskListEditWindowValidation : ITaskGroupWindowValidation
    {
      public bool Window_Validation(TextBox textBox, TaskGroup taskGroup)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(taskGroup);

            if (!Validator.TryValidateObject(taskGroup, context, results, true))
            {
                foreach (var error in results)
                {
                    if (error.ErrorMessage == "Title Can't be empty.")
                    {
                        textBox.ToolTip = error.ErrorMessage;
                        textBox.Background = Brushes.RosyBrown;
                    }
                    if (error.ErrorMessage == "Title contatins only unicode letters,numbers and !?.,:; characters.(At least 2)")
                    {
                        textBox.ToolTip = error.ErrorMessage;
                        textBox.Background = Brushes.RosyBrown;
                    }
                }
                return false;
            }
            else
            {
                textBox.ToolTip = "";
                textBox.Background = Brushes.Transparent;
                return true;
            }
        }
       
    }
}
