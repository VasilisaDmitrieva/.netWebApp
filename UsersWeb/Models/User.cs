using System.ComponentModel.DataAnnotations.Schema;

namespace UsersWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Mail { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public Status Status { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        [NotMapped]
        public bool IsChecked { get; set; }

        public User() {}

        public User(string? userName, string? mail, Status status, string? passwordSalt, string? passwordHash)
        {
            this.PasswordSalt = passwordSalt;
            this.PasswordHash = passwordHash;
            this.RegistrationDate = DateTime.Now;
            this.LastLoginDate = DateTime.Now;
            this.UserName = userName;
            this.Mail = mail;
            this.Status = status;
        }
    }
}
