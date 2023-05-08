using System;
using System.Collections.Generic;
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

namespace TESTTEST.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
       
        public DeleteWindow(ToDoTask task)
        {
            
            InitializeComponent();
            LabelForTitle.Content = task.Title;

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {          
                this.DialogResult = true;
        }
    }
}
