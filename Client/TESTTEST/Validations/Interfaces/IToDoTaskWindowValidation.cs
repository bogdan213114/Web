using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TESTTEST.MVVM.Model;

namespace TESTTEST.Validations
{
  public  interface IToDoTaskWindowValidation
    {
         bool Window_Validation(TextBox SomeDescription,TextBox SomeTitle, ToDoTask toDoTask);      
    }
}
