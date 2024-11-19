using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTDS.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int _userId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        private string _password { get; set; }

        public List<Contact> ContactList { get; set; } = new List<Contact>();

        [NotMapped]
        public int UserId
        {
            get { return _userId; }
            private set { _userId = value; }
        }

        [NotMapped]
        public string Password
        {
            get { return _password; }
            private set { _password = value; }
        }

        // Constructor
        public User(string username, string name, string email, string password)
        {
            Username = username;
            Name = name;
            Email = email;
            _password = password;
        }

        public void AddContact(string username)
        {
            if (ContactList.Exists(c => c.Username == username))
            {
                return;
            }
            ContactList.Add(new Contact { Username = username, UserId = _userId });
        }

        public void RemoveContact(string username)
        {
            var contact = ContactList.Find(c => c.Username == username);
            if (contact != null)
            {
                ContactList.Remove(contact);
            }
        }

        public void UpdatePassword(string oldPassword, string newPassword)
        {
            if (_password == oldPassword)
            {
                _password = newPassword;
            }
        }

        public void UpdatePassword(string newPassword)
        {
            _password = newPassword;
        }

        public bool AuthenticatePassword(string password)
        {
            return _password == password;
        }

    }
}
