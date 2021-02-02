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
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
                    };
            _data = data;
        }

        public void AddMessage(string message)
        {
            List<string> messages;
            if (this._data.TryGetValue("Msg", out object? ob))
            {
                messages = (List<string>)ob;
            }
            else
            {
                messages = new List<string>();
                _data["Msg"] = messages;
            }
            messages.Add(message);
        }


        public void AddMessages(IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                AddMessage(message);
            }
        }

        public IList<string> GetMessages()
        {
            List<string> messages;
            if (this._data.TryGetValue("Msg", out object? ob))
            {
                messages = (List<string>)ob;
            }
            else
            {
                messages = new List<string>();
            }
            return messages;
        }

    }
}
