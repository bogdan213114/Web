using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TESTTEST.MVVM.Model
{
    public class Account : BaseModel, INotifyPropertyChanged , IValidatableObject
    {
        private User user;
        private string passWord;
        private string name;
        [Required(ErrorMessage = "Non entered Name")] //"Non entered Name"
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(" Name");
            }
        }
        [Required(ErrorMessage = "Non entered Password")] //"Non entered Password"
        public string PassWord
        {
            get
            {
                return passWord;
            }
            set
            {
                passWord = value;
                OnPropertyChanged("PassWord");
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this.Name.Length<3 || this.Name.Length>20)
            {
                errors.Add(new ValidationResult("Name Lenght must be in space between 3-20"));//"Name Lenght must be in space between 3-20")
            }    
               

            if (this.PassWord.Length < 6 || this.PassWord.Length > 16)
            {
                errors.Add(new ValidationResult("Password Lenght must be in space between 6-16"));//"PassWord Lenght must be in space between 6-16"
            }
                
            if(!Regex.IsMatch(this.Name, @"[A-Za-z0-9_$!]"))
            {
                errors.Add(new ValidationResult("Name contains only letters, numbers, and _$!"));//"Name contains only letters, numbers, and _$!"
            }
            if (!Regex.IsMatch(this.PassWord, @"[A-Za-z0-9_$!]"))
            {
                errors.Add(new ValidationResult("Password contains only letters, numbers, and _$!"));//"Password  contains only letters, numbers, and _$!"
            }
            if (!Regex.IsMatch(this.PassWord, @".*[A-Z].*"))
            {
                errors.Add(new ValidationResult("Password contains at least one capitalized letter"));//"Password  contains at least one capitalized letter"
            }
            if (!Regex.IsMatch(this.PassWord, @".*[0-9].*"))
            {
                errors.Add(new ValidationResult("Password contains at least one number"));//"Password  contains at least one number"
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
