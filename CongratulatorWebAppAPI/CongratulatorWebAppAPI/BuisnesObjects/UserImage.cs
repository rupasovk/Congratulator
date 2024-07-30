namespace CongratulatorWebAppAPI.BuisnesObjects
{
    public class UserImage
    {
        #region Fields

        Guid _id;
        byte[] _imageBytes;
        Guid _userId;
        User _user;

        #endregion

        #region Properties

        public Guid Id { get { return _id; } set { _id = value; } }
        public byte[] ImageBytes { get { return _imageBytes; } set { _imageBytes = value; } }

        // Внешний ключ для связи с пользователем
        public Guid UserId { get { return _userId; } set { _userId = value; } }

        // Навигационное свойство для связи с пользователем
        public User User { get { return _user; } set { _user = value; } }

        #endregion

        #region Constructors

        public UserImage() { }

        public UserImage(Byte[] _imageBytes, User _user)
        {
            Id = Guid.NewGuid();
            ImageBytes = _imageBytes;
            User = _user;
            UserId = _user.Id;
        }

        #endregion

    }
}
