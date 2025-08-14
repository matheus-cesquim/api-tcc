namespace Api.Domain.Models.User
{
    public class UserModel : BaseModel
    {
        
        private string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}