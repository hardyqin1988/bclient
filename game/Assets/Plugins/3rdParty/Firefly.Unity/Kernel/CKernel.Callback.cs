using Firefly.Core.Data;
using Firefly.Core.Event;
using Firefly.Core.Variant;

namespace Firefly.Unity.Kernel
{
    partial class CKernel
    {
        public void RegisterEntityCallback(string entity_type, EntityEvent event_enum, EventCallback handler)
        {
            _DataCallbackManager.AddEntityCallback(entity_type, event_enum, handler);
        }

        public void RegisterPropertyCallback(string entity_type, string property_name, PropertyEvent event_enum, EventCallback handler)
        {
            _DataCallbackManager.AddPropertyCallback(entity_type, property_name, event_enum, handler);
        }

        public void RegisterRecordCallback(string entity_type, string record_name, RecordEvent event_enum, EventCallback handler)
        {
            _DataCallbackManager.AddRecordCallback(entity_type, record_name, event_enum, handler);
        }

        public void RegisterCommandCallback(int command_msg, EventCallback handler)
        {
            _CommandMsgCallbackManager.Register(command_msg, handler);
        }

        public void CallCommandMsgHandler(PersistID self, int command_msg, VariantList msg)
        {
            _CommandMsgCallbackManager.Callback(command_msg, this, self, msg);
        }

        public void RegisterDataTypeCallback(DataType data_type, EventCallback handler)
        {
            _DataTypeCallbackManager.Register(data_type, handler);
        }

        public void CallDataTypeCallback(DataType data_type, PersistID self, VariantList args)
        {
            _DataTypeCallbackManager.Callback(data_type, this, self, args);
        }

        public void RegisterCustomCallback(int custom_msg, EventCallback handler)
        {
            _CustomMsgCallbackManager.Register(custom_msg, handler);
        }

        public void CallCustomMsgHandler(PersistID self, int custom_msg, VariantList msg)
        {
            _CustomMsgCallbackManager.Callback(custom_msg, this, self, msg);
        }
    }
}
