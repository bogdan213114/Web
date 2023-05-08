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
    public class ToDoTask : BaseModel, INotifyPropertyChanged, IValidatableObject
    {
        private int userId;
        private string title;
        private string description;
        private bool isDone;
        private User user;
        private ObservableCollection<TaskGroup> groups;
        private DateTime creationDate;
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
    //    [Required(ErrorMessage = "Task Title must containt at least 2 symbols")]
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
    //    [Required(ErrorMessage = "NullDescription")]
        public string Description {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public bool IsDone
        {
            get
            {
                return isDone;
            }
            set
            {
                isDone = value;
                OnPropertyChanged("IsDone");
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
        public ObservableCollection<TaskGroup> Groups
        {
            get
            {
                return groups;
            }
            set
            {
                groups = value;
                OnPropertyChanged("Groups");
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
            if(this.Title == null)
            {
                errors.Add(new ValidationResult("Title Can't be empty."));
            }
            if(this.Description == null)
            {
                errors.Add(new ValidationResult("Description is empty"));
            }
            if(this.Title != null)
            {
                if (!Regex.IsMatch(this.Title, @"^[\p{L}\p{N}!?.,:;\-\+\s]{2,}$"))
                {
                    errors.Add(new ValidationResult("Title contatins only unicode letters,numbers and !?.,:; characters.(At least 2)"));
                }
            }
            if(this.Description != null)
            {
                if (!Regex.IsMatch(this.Description, @"^[\p{L}\p{N}!?.,:;\-\+\n\r\s]*$"))
                {
                    errors.Add(new ValidationResult("Description contatins only unicode letters,numbers,line breaks and !?.,:; characters."));
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
