namespace Bom.Core.Identity
{
    public class UserSession
    {
        public UserSession(string token, IUser user)
        {
            this.Token = token;
            this.User = user;
        }

        public string Token { get; }

        public IUser User { get; }

        public override string ToString()
        {
            return $"{nameof(UserSession)}: '{User.Username}'";
        }
    }
}
