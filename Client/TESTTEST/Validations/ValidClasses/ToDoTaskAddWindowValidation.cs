using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TESTTEST.MVVM.Model;

namespace TESTTEST.Validations
{
    public class ToDoTaskAddWindowValidation : IToDoTaskWindowValidation
    {
        public  bool Window_Validation(TextBox DescBox, TextBox TitleBox, ToDoTask toDoTask)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(toDoTask);
           
            if (!Validator.TryValidateObject(toDoTask, context, results, true))
            {
                foreach (var error in results)
                {
                 
                    if (error.ErrorMessage == "Title contatins only unicode letters,numbers and !?.,:; characters.(At least 2)")
                    {
                        TitleBox.ToolTip = error.ErrorMessage;
                        TitleBox.Background = Brushes.RosyBrown;
                    }
                    if(error.ErrorMessage == "Title Can't be empty.")
                    {
                        TitleBox.ToolTip = error.ErrorMessage;
                        TitleBox.Background = Brushes.RosyBrown;
                    }
                    if(error.ErrorMessage == "Description is empty")
                    {
                        toDoTask.Description = " ";
                    }
                    if(error.ErrorMessage == "Description contatins only unicode letters,numbers,line breaks and !?.,:; characters.")
                    {
                        DescBox.ToolTip = error.ErrorMessage;
                        DescBox.Background = Brushes.RosyBrown;
                    }
                }
                if(results.Count == 1 && results[0].ErrorMessage == "Description is empty")
                {
                    return true;
                }
               
                return false;
            }
            else
            {
                TitleBox.ToolTip = "";
                TitleBox.Background = Brushes.Transparent;
                DescBox.ToolTip = "";
                DescBox.Background = Brushes.Transparent;
                return true;
            }

        }
    }
}
