using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Newtonsoft.Json;

namespace Bom.Web.Identity.Mvc
{
    public class TempDataHelper
    {
        private readonly ITempDataDictionary _data;

        /// <summary>
        /// ctor: Helper for TempData, provides typed access
        /// </summary>
        /// <param name="data">TempData (or possibly other dic)</param>
        public TempDataHelper(ITempDataDictionary data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            };
            _data = data;
        }

        public void AddMessasge(UserMessage message)
        {
            // get messages (has to be serialzed as standard serializer cannot handle collections
            IList<UserMessage>? messages = GetMessages();

            // add message
            messages.Add(message);

            // serialize it back
            var serializedString = JsonConvert.SerializeObject(messages);
            _data["Msg"] = serializedString;

        }

        public IList<UserMessage> GetMessages()
        {
            List<UserMessage>? messages = null;
            if (this._data.TryGetValue("Msg", out object? ob))
            {
                var serialized = ob?.ToString();
                if (!string.IsNullOrEmpty(serialized))
                {
                    messages = JsonConvert.DeserializeObject<List<UserMessage>>(serialized);
                }
            }
            if (messages == null)
            {
                messages = new List<UserMessage>();
            }
            return messages;
        }
    }
}
