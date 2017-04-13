using Firefly.Core.Data;
using Firefly.Core.Variant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firefly.Unity.Databind
{
    public delegate void DataCallback(PersistID self, VariantList args);

    public class DataBinder
    {
        private Dictionary<Group<DataType, PersistID, string>, HashSet<DataCallback>> _EventHandlers;

        public DataBinder()
        {
            _EventHandlers = new Dictionary<Group<DataType, PersistID, string>, HashSet<DataCallback>>();
        }

        public void Callback(DataType data_type, PersistID pid, string name, VariantList args)
        {
            HashSet<DataCallback> found = null;
            Group<DataType, PersistID, string> event_id 
                = new Group<DataType, PersistID, string>(data_type, pid, name);

            if (!_EventHandlers.TryGetValue(event_id, out found))
            {
                return;
            }

            foreach (DataCallback event_handler in found)
            {
                event_handler(pid, new VariantList(args));
            }
        }

        public void Register(DataType data_type, PersistID pid, string name, DataCallback event_handler)
        {
            Group<DataType, PersistID, string> event_id
               = new Group<DataType, PersistID, string>(data_type, pid, name);

            HashSet<DataCallback> found = null;
            if (!_EventHandlers.TryGetValue(event_id, out found))
            {
                found = new HashSet<DataCallback>();
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
    }
}
