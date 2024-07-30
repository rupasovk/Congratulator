namespace CongratulatorWebAppAPI.BuisnesObjects
{
    public class UserCongratulation
    {
        #region Fields

        Guid _id;
        string _message;
        string _type;

        #endregion

        #region Constructors

        public UserCongratulation() { }

        public UserCongratulation(Guid id, string message, string type)
        {
            Id = id;
            Message = message;
            Type = type;
        }

        public UserCongratulation(string message, string type)
        {
            Message = message;
            Type = type;
        }

        #endregion

        #region Properties

        public Guid Id
        {
            get{ return _id; } 
            set{ _id = value; } 
        }

        public string Message
        { 
            get { return _message; } 
            set { _message = value; } 
        }

        public string Type
        { 
            get { return _type; } 
            set { _type = value; } 
        }

        #endregion
    }
}
