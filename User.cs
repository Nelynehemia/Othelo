using System.Drawing;

namespace othello
{
    public class User
    {
        public enum UserOption
        {
            Human,
            Computer,
        }

        private readonly UserOption m_UserType;
        private eCoinColor m_CoinColor;
        private string m_Name;

        /*get set*/
        public eCoinColor CoinColor
        {
            get { return m_CoinColor; }
            set { m_CoinColor = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public UserOption UserType
        {
            get { return m_UserType; }
        }

        /*ctor*/
        public User(UserOption i_UserType, eCoinColor i_CoinColor, string i_Name)
        {
            m_Name = i_Name;
            CoinColor = i_CoinColor;
            m_UserType = i_UserType;
        }
    }
}