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

using Firefly.Core.Config;
using Firefly.Core.Data;
using Firefly.Core.Event;
using Firefly.Core.Interface;
using Firefly.Core.Variant;

namespace Firefly.Core.Kernel
{
    public class DataCallbackManager
    {
        private DataEventManager<PropertyEvent> _PropertyCallback   = null;

        private DataEventManager<RecordEvent>   _RecordCallback     = null;

        private EntityEventManager              _EntityCallback     = null;

        private IKernel                         _IKernel            = null;

        public DataCallbackManager(IKernel kernel)
        {
            _IKernel          = kernel;
            _PropertyCallback = new DataEventManager<PropertyEvent>();
            _RecordCallback   = new DataEventManager<RecordEvent>();
            _EntityCallback   = new EntityEventManager();
        }

        public void AddEntityCallback(string entity_type, EntityEvent entity_event, EventCallback event_handler)
        {
            _EntityCallback.Register(entity_type, entity_event, event_handler);
        }

        public void CallEntityHandler(PersistID pid, string entity_type, EntityEvent entity_event, VariantList args)
        {
            EntityDef entity_def = _IKernel.GetEntityDef(entity_type);

            if (entity_def != null && entity_def.InheritList != null)
            {
                for (int i = entity_def.InheritList.Count - 1; i >= 0; i--)
                {
                    string parent_type = entity_def.InheritList.StringAt(i);

                    _EntityCallback.Callback(parent_type, entity_event, _IKernel, pid, args);
                }
            }

            if (entity_def != null)
            {
                _EntityCallback.Callback(entity_type, entity_event, _IKernel, pid, args);
            }
        }

        public void AddPropertyCallback(string entity_type, string property_name,
            PropertyEvent property_event, EventCallback event_handler)
        {
            _PropertyCallback.Register(entity_type, property_name, property_event, event_handler);
        }

        public void CallPropertyHandler(PersistID pid, string property_name, PropertyEvent property_event, VariantList args)
        {
            string entity_type = _IKernel.GetType(pid);

            EntityDef entity_def = _IKernel.GetEntityDef(entity_type);

            if (entity_def != null && entity_def.InheritList != null)
            {
                for (int i = entity_def.InheritList.Count - 1; i >= 0; i--)
                {
                    string parent_type = entity_def.InheritList.StringAt(i);

                    _PropertyCallback.Callback(property_event, parent_type, property_name, _IKernel, pid, args);
                }
            }

            if (entity_def != null)
            {
                _PropertyCallback.Callback(property_event, entity_type, property_name, _IKernel, pid, args);
            }
        }

        public void AddRecordCallback(string entity_type, string record_name, RecordEvent record_event,
            EventCallback event_handler)
        {
            _RecordCallback.Register(entity_type, record_name, record_event, event_handler);
        }

        public void CallRecordHandler(PersistID pid, string record_name, RecordEvent record_event, VariantList args)
        {
            string entity_type = _IKernel.GetType(pid);

            EntityDef entity_def = _IKernel.GetEntityDef(entity_type);

            if (entity_def != null && entity_def.InheritList != null)
            {
                for (int i = entity_def.InheritList.Count - 1; i >= 0; i--)
                {
                    string parent_type = entity_def.InheritList.StringAt(i);

                    _RecordCallback.Callback(record_event, parent_type, record_name, _IKernel, pid, args);
                }
            }
            
            if (entity_def != null)
            {
                _RecordCallback.Callback(record_event, entity_type, record_name, _IKernel, pid, args);
            }
        }
    }
}
