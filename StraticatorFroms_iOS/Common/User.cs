
namespace Straticator.Common
{
    class User
    {
        internal string Name { get; private set; }
        internal string Password { get; private set; }

        public User(string name, string pwd)
        {
            Name = name;
            Password = pwd;
        }
    }
}
