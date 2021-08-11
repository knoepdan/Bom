using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bom.Core.Identity.DbModels;

namespace Bom.Core.Identity
{
    public class TokenService
    {
        static TokenService()
        {
            Bom.Utils.Dev.Todo("Save token to db etc.", Bom.Utils.Dev.Urgency.Low);
            Bom.Utils.Dev.Todo("Not that elegant, especially cleaning. Better use .Net cache?", Bom.Utils.Dev.Urgency.Low);
        }

        private static ConcurrentDictionary<string, UserAuthTimeStamp> CurrentUserDic = new ConcurrentDictionary<string, UserAuthTimeStamp>();

        private static int SecondsToCacheUser = 60 * 60 * 4;

        public string CreateUserSession(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = GenerateNewToken();
            var info = new UserAuthTimeStamp(user);
            if (CurrentUserDic.TryAdd(token, info))
            {
                return token;
            }
            throw new Exception("Failed to authenticate user");
        }

        public IUser? GetUser(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            UserAuthTimeStamp? user;
            if (CurrentUserDic.TryGetValue(token, out user))
            {
                if (user != null && user.Created < DateTime.Now.AddSeconds(SecondsToCacheUser))
                {
                    return user;
                }
                else
                {
                    CleanOldEntries();
                }
            }
            return null;
        }

        public UserSession? GetUserAndToken(string username)
        {
            string? token = GetCurrentTokenforUser(username);
            IUser? user = GetUser(token);
            if(user != null && token != null)
            {
                return new UserSession(token, user);
            }
            return null;
        }

        public IUser? GetUserByUsername(string username)
        {
            string? token = GetCurrentTokenforUser(username);
            IUser? user = GetUser(token);
            return user;
        }

        public bool LogOut(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            UserAuthTimeStamp? old;
            bool foundAndRemoved = CurrentUserDic.TryRemove(token, out old);
            return foundAndRemoved;
        }

        public bool LogOutByUsername(string username)
        {
            string? token = GetCurrentTokenforUser(username);
            bool foundAndRemoved = LogOut(token);
            return foundAndRemoved;
        }

        private string? GetCurrentTokenforUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            // possible improvment: performance
            string? token = null;
            foreach (var keyVal in CurrentUserDic)
            {
                if (keyVal.Value.Username == username)
                {
                    token = keyVal.Key;
                    break;
                }
            }
            return token;
        }


        private static string GenerateNewToken()
        {
            var token = Guid.NewGuid() + "";
            return token;
        }

        private void CleanOldEntries()
        {
            var toRemove = new List<string>();
            foreach (var keyVal in CurrentUserDic)
            {
                if (keyVal.Value.Created < DateTime.Now.AddSeconds(-SecondsToCacheUser))
                {
                    toRemove.Add(keyVal.Key);
                }
            }
            foreach (var oldToken in toRemove)
            {
                UserAuthTimeStamp? old;
                CurrentUserDic.TryRemove(oldToken, out old);
            }
        }

        internal class UserAuthTimeStamp : BasicUser
        {
            public UserAuthTimeStamp(User user) : base(user)
            {
            }
            public DateTime Created { get; } = DateTime.Now;
        }

    }
}
