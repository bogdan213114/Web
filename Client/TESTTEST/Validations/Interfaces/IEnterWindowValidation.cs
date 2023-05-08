using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TESTTEST.MVVM.Model;

namespace TESTTEST.Validations.Interfaces
{
    public interface IEnterWindowValidation
    {
        bool EnterValidation(TextBox textBox, PasswordBox passBox, Account account);

    }
}
