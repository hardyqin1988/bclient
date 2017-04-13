using Firefly.Core.Data;
using Firefly.Core.Variant;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Firefly.Unity.Databind
{
    public delegate void MsgCallback(int msg_id, VariantList args);

    public class MsgBinder
    {
        private Dictionary<int, HashSet<MsgCallback>> _EventHandlers;

        public MsgBinder()
        {
            _EventHandlers = new Dictionary<int, HashSet<MsgCallback>>();
        }

        public void Callback(int msg_id, VariantList args)
        {
            HashSet<MsgCallback> found = null;
            if (!_EventHandlers.TryGetValue(msg_id, out found))
            {
                return;
            }

            foreach (MsgCallback event_handler in found)
            {
                event_handler(msg_id, new VariantList(args));
            }
        }

        public void Register(int event_id, MsgCallback event_handler)
        {
            HashSet<MsgCallback> found = null;
            if (!_EventHandlers.TryGetValue(event_id, out found))
            {
                found = new HashSet<MsgCallback>();
                _EventHandlers.Add(event_id, found);
            }

            try
            {
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
            catch (Exception ex)
            {
                throw new EventException("EventExpection ", ex);
            }
        }

        public void Cancel(int event_id, MsgCallback event_handler)
        {
            HashSet<MsgCallback> found = null;
            if (!_EventHandlers.TryGetValue(event_id, out found))
            {
                return;
            }

            if (found.Contains(event_handler))
            {
                found.Remove(event_handler);
            }
        }
    }
}
