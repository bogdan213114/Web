using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TESTTEST.MVVM.Model
{
    public class TaskGroup : BaseModel, INotifyPropertyChanged, IValidatableObject
    {
        private DateTime creationDate;
        private ObservableCollection<ToDoTask> tasks;
        private User user;
        private string title;
        private int userId;
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }
    //    [Required(ErrorMessage = "TaskGroup Title must containt at least 2 symbols")]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }
        public ObservableCollection<ToDoTask> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
                OnPropertyChanged("Tasks");
            }
        }
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (this.Title == null)
            {
                errors.Add(new ValidationResult("Title Can't be empty."));
            }
            if(this.Title != null)
            {
                if (!Regex.IsMatch(this.Title, @"^[\p{L}\p{N}!?.,:;\-\+\s]{2,}$"))
                {
                    errors.Add(new ValidationResult("Title contatins only unicode letters,numbers and !?.,:; characters.(At least 2)"));
                }
            }
            
            return errors;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
