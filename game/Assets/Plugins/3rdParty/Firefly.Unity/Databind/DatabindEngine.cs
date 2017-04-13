using Firefly.Core.Data;
using Firefly.Core.Variant;
using System.Collections;
using UnityEngine;

namespace Firefly.Unity.Databind
{
    public class DatabindEngine : SingletonEngine<DatabindEngine>, IEngine
    {
        private DataBinder _DataBinder;

        private MsgBinder _CustomMsgBinder;

        private MsgBinder _SystemMsgBinder;

        public override void Awake()
        {
            base.Awake();

            _DataBinder = new DataBinder();
            _CustomMsgBinder = new MsgBinder();
            _SystemMsgBinder = new MsgBinder();
        }

        protected override IEnumerator Startup()
        {
            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Shutdown()
        {
            yield return new WaitForFixedUpdate();
        }

        public void RemoveSystemMsg(int system_id, MsgCallback callback)
        {
            _SystemMsgBinder.Cancel(system_id, callback);
        }

        public void BindSystemMsg(int system_id, MsgCallback callback)
        {
            _SystemMsgBinder.Register(system_id, callback);
        }

        public void CallSystemMsg(int system_id, VariantList args)
        {
            _SystemMsgBinder.Callback(system_id, args);
        }

        public void BindCustomMsg(int custom_id, MsgCallback callback)
        {
            _CustomMsgBinder.Register(custom_id, callback);
        }

        public void CallCustomMsg(int custom_id, VariantList args)
        {
            _CustomMsgBinder.Callback(custom_id, args);
        }

        public void BindProperty(PersistID pid, string property_name, DataCallback calllback)
        {
            _DataBinder.Register(DataType.Property, pid, property_name, calllback);
        }

        public void BindRecord(PersistID pid, string record_name, DataCallback calllback)
        {
            _DataBinder.Register(DataType.Record, pid, record_name, calllback);
        }

        public void CallProperty(PersistID pid, string property_name, VariantList args)
        {
            _DataBinder.Callback(DataType.Property, pid, property_name, args);
        }

        public void CallRecord(PersistID pid, string record_name, VariantList args)
        {
            _DataBinder.Callback(DataType.Record, pid, record_name, args);
        }
    }
}

