#region License
/*****************************************************************************
 *MIT License
 *
 *Copyright (c) 2017 cathy33

 *Permission is hereby granted, free of charge, to any person obtaining a copy
 *of this software and associated documentation files (the "Software"), to deal
 *in the Software without restriction, including without limitation the rights
 *to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *copies of the Software, and to permit persons to whom the Software is
 *furnished to do so, subject to the following conditions:

 *The above copyright notice and this permission notice shall be included in all
 *copies or substantial portions of the Software.

 *THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *SOFTWARE.
 *****************************************************************************/
#endregion

using Firefly.Core.Data;
using Firefly.Core.Interface;
using Firefly.Core.Variant;
using System.Collections.Generic;
using System.Text;

namespace Firefly.Core.Event
{
    public delegate void EventCallback(IKernel kernel, PersistID self, VariantList args);

    public class EventManager<EVENT_ID>
    {
        private Dictionary<EVENT_ID, HashSet<EventCallback>> _EventHandlers;

        public EventManager()
        {
            _EventHandlers = new Dictionary<EVENT_ID, HashSet<EventCallback>>();
        }

        public void Callback(EVENT_ID event_id, IKernel kernel, PersistID self, VariantList args)
        {
            HashSet<EventCallback> found = null;
            if (!_EventHandlers.TryGetValue(event_id, out found))
            {
                return;
            }

            foreach (EventCallback event_handler in found)
            {
                event_handler(kernel, self, new VariantList(args));
            }
        }

        public void Register(EVENT_ID event_id, EventCallback event_handler)
        {
            HashSet<EventCallback> found = null;
            if (!_EventHandlers.TryGetValue(event_id, out found))
            {
                found = new HashSet<EventCallback>();
                _EventHandlers.Add(event_id, found);
            }

            if (found.Contains(event_handler))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Cant Add Event Because {0} Has Exist in Event {1}",
                    event_handler.Method.Name, event_id.ToString());

                throw new EventException(sb.ToString());
            }
            else
            {
                found.Add(event_handler);
            }
        }
    }
}
