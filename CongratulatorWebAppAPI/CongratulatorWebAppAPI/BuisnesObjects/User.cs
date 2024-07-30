using CongratulatorWebAppAPI.BuisnesObjects.DtoObjects;
using System.Runtime.Serialization;

namespace CongratulatorWebAppAPI.BuisnesObjects
{
    [Serializable]
    public class User
    {
        #region Fields

        Guid _id;
        string _name;
        string _surname;
        DateTime _birthDay;
        string _email;
        string _country;
        //List<UserImage> _userImages;
        UserImage _userImage;

        #endregion

        #region Constructors

        public User() { } //=> _userImages = new List<UserImage>();

        public User(Guid id, string name, string surname, DateTime birthDay, string email, string country)
        {
            Id = id;
            Name = name;
            SurName = surname;
            BirthDay = birthDay;
            Email = email;
            Country = country;
            //_userImages = new List<UserImage>();
        }

        public User(string name, string surname, DateTime birthDay, string email, string country)
        {
            Name = name;
            SurName = surname;
            BirthDay = birthDay;
            Email = email;
            Country = country;
            //_userImages = new List<UserImage>();
        }

        public User(UserDto userDto)
        {
            Name = userDto.UserName;
            SurName = userDto.Surname;
            BirthDay = userDto.Birthday;
            Email = userDto.Email;
            Country = userDto.Country;
        }

        #endregion

        #region Properties

        public Guid Id { get { return _id; } set { _id = value; } }

        public string Name { get { return _name; } set { _name = value; } }

        public string SurName { get { return _surname; } set { _surname = value; } }

        public DateTime BirthDay { get { return _birthDay; } set { _birthDay = value; } }

        public string Email { get { return _email; } set { _email = value; } }

        public string Country { get { return _country; } set { _country = value; } }
        
        public UserImage UserImage { get { return _userImage; } set { _userImage = value; } }

        //public List<UserImage> UserImages { get { return _userImages; } }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"User: {Name} {SurName}, {BirthDay.ToString("dd.MM.yyyy HH:mm:ss")}, {Email}, {Country}";
        }

        #endregion
    }
}
