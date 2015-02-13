using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Models;

namespace TestProject.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email адрес")]
        [Remote("CheckEmail", "Users", ErrorMessage = "Такой адрес уже существует")]
        [RegularExpression("^([A-Za-z0-9_\\-\\.])+\\@([A-Za-z0-9_\\-\\.])+\\.([A-Za-z]{2,4})$", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 50 символов")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Имя")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Фамилия")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Дата добавления")]
        [DataType(DataType.Date)]
        public DateTime? AddedDate { get; set; }

        [Display(Name = "Дата активации")]
        [DataType(DataType.Date)]
        public DateTime? ActivatedDate { get; set; }

        [Display(Name = "Строка Активации")]
        public string ActivatedLink { get; set; }

        [Display(Name = "Дата последнего визита")]
        public DateTime? LastVisitDate { get; set; }

        [Display(Name = "Аватар")]
        public string AvatarPath { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }


        public virtual ICollection<Role> Roles { get; set; }

        
    }

    public class UserDbContext : DbContext
    {
        public UserDbContext()
            : base("name=UserDbContext")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public User Login(string email, string password)
        {
            return Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0 && p.Password == password);
        }
    }
}