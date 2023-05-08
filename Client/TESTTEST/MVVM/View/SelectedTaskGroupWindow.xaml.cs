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
using TESTTEST.MVVM.ViewModel;

namespace TESTTEST.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для SelectedTaskGroupWindow.xaml
    /// </summary>
    public partial class SelectedTaskGroupWindow : Window
    {
        
        public SelectedTaskGroupWindow(MainWindow window, User user, TaskGroup taskGroup)
        {
           
            InitializeComponent();
           
            DataContext = new SelectedTaskGroupViewModel(window,this,user, taskGroup);         
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;    
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
