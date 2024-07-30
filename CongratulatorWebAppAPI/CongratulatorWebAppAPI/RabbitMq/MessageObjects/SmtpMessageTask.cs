using System;

namespace CongratulatorWebAppAPI.RabbitMq.MessageObjects
{
    [Serializable]
    public class SmtpMessageTask
    {
        #region Fields

        string _message = "";
        List<string> _to = new List<string>() { };
        string _subject = "";
        string _body = "";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public List<string> To
        {
            get { return _to; }
            set { _to = value; }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }
        #endregion

        #region Methods

        #endregion
    }
}
