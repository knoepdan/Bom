namespace Bom.Core.Identity
{
    public interface IUser
    {
        public string Username { get; }

        public string Name { get; }

        public int UserId { get; }
    }
}