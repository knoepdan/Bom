using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom.Web.Identity.Mvc
{
    public class TempDataHelper
    {
        private readonly IDictionary<string, object> _data;

        /// <summary>
        /// ctor: Helper for TempData, provides typed access
        /// </summary>
        /// <param name="data">TempData (or possibly other dic)</param>
        public TempDataHelper(IDictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            };
            _data = data;
        }

        public void AddMessasge(UserMessage message)
        {
            List<UserMessage> messages;
            if (this._data.TryGetValue("Msg", out object? ob))
            {
                messages = (List<UserMessage>)ob;
            }
            else
            {
                messages = new List<UserMessage>();
                _data["Msg"] = messages;
            }
            messages.Add(message);
        }

        public IList<UserMessage> GetMessages()
        {
            List<UserMessage> messages;
            if (this._data.TryGetValue("Msg", out object? ob))
            {
                messages = (List<UserMessage>)ob;
            }
            else
            {
                messages = new List<UserMessage>();
            }
            return messages;
        }
    }
}
