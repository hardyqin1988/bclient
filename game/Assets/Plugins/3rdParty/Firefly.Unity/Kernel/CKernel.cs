using System;
using Firefly.Core.Config;
using Firefly.Core.Data;
using Firefly.Core.Event;
using Firefly.Core.Interface;
using Firefly.Core.Kernel;
using Firefly.Core.Utility;
using Firefly.Core.Variant;
using Firefly.Unity.Global;

namespace Firefly.Unity.Kernel
{
    public partial class CKernel : IKernel
    {
        private EntityManager          _EntityManager               = null;
        private DataCallbackManager    _DataCallbackManager         = null;
        private ModuleManager          _ModuleManager               = null;
        private TimerManager           _TimerManager                = null;
        private EventManager<int>      _CommandMsgCallbackManager   = null;
        private EventManager<int>      _CustomMsgCallbackManager    = null;
        private EventManager<DataType> _DataTypeCallbackManager     = null;
        private ILog                   _Logger                      = null;

        public CKernel(Definition define)
        {
            _EntityManager             = new EntityManager(this);
            _DataCallbackManager       = new DataCallbackManager(this);
            _ModuleManager             = new ModuleManager(this);
            _TimerManager              = new TimerManager(this);
            _CommandMsgCallbackManager = new EventManager<int>();
            _CustomMsgCallbackManager  = new EventManager<int>();
            _DataTypeCallbackManager   = new EventManager<DataType>();
            _Logger                    = LogAssert.GetLog("Kernel");
            _Define                    = define;
        }

        private Definition _Define = null;

        public Definition Define { get { return _Define; } }

        public PersistID CreateEntity(string define, PersistID root, VariantList args = null)
        {
            IEntity entity = _EntityManager.CreateEntity(define, root);
            if (entity == null)
            {
                return PersistID.Empty;
            }

            VariantList new_args = VariantList.New();
            if (args != null) new_args.Append(args);
            _DataCallbackManager.CallEntityHandler(entity.Self, entity.Type, EntityEvent.OnCreate, new_args);

            return entity.Self;
        }

        public PersistID CreateEntity(PersistID pid, string define, PersistID root, VariantList args = null)
        {
            IEntity entity = _EntityManager.CreateEntity(pid, define);
            if (entity == null)
            {
                return PersistID.Empty;
            }

            VariantList new_args = VariantList.New();
            if (args != null) new_args.Append(args);
            _DataCallbackManager.CallEntityHandler(entity.Self, entity.Type, EntityEvent.OnCreate, new_args);

            return entity.Self;
        }

        public void DestroyEntity(PersistID pid)
        {
            _EntityManager.DelEntity(pid);
        }

        public string GetType(PersistID pid)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_STRING;
            }

            return entity.Type;
        }

        public bool Exists(PersistID pid)
        {
            return _EntityManager.FindEntity(pid);
        }

        public void CreateModule<T>() where T : IModule
        {
            _ModuleManager.CreateModule<T>();
        }

        public void LoadEntity(PersistID pid, VariantList entity_protos)
        {
            for (int i = 0; i < entity_protos.Count; i++)
            {
                byte[] entity_proto = entity_protos.BytesAt(i);
                Entity entity = DataUtil.BytesToObject<Entity>(entity_proto);

                if (_EntityManager.FindEntity(entity.Self))
                {
                    continue;
                }

                _EntityManager.AddEntity(entity);

                _DataCallbackManager.CallEntityHandler(entity.Self, entity.Type, EntityEvent.OnLoad, VariantList.Empty);
            }
        }

        public EntityDef GetEntityDef(string type)
        {
            return _Define.GetEntity(type);
        }

        public bool Equals(PersistID pid, string type)
        {
            return type.Equals(GetType(pid));
        }

        public void OnlineEntity(PersistID root)
        {
            IEntity entity = _EntityManager.GetEntity(root);
            if (entity == null)
            {
                return;
            }

            entity.Online = true;
        }

        public void AddCountdown(PersistID pid, string countdown_name, long over_millseconds, TimerCallback countdown)
        {
            _TimerManager.AddTimer(pid, countdown_name, over_millseconds, countdown);
        }

        public void AddHeartbeat(PersistID pid, string heartbeat_name, long gap_millseconds, int count, TimerCallback heartbeat)
        {
            _TimerManager.AddTimer(pid, heartbeat_name, gap_millseconds, count, heartbeat);
        }

        public void DelHeartbeat(PersistID pid, string heartbeat_name)
        {
            _TimerManager.DelTimer(pid, heartbeat_name);
        }

        public void DelCountdown(PersistID pid, string countdown_name)
        {
            _TimerManager.DelTimer(pid, countdown_name);
        }

        public VariantList ToProto(PersistID pid)
        {
            VariantList entity_bytes = VariantList.New();

            ParseEntity(pid, ref entity_bytes);

            return entity_bytes;
        }

        private void ParseEntity(PersistID pid, ref VariantList proto)
        {
            Entity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            byte[] entity_bytes = DataUtil.ObjectToBytes(entity);
            if (entity_bytes == null || entity_bytes.Length == 0)
            {
                return;
            }

            proto.Append(entity_bytes);
        }

        public PersistID CreateEntity(PersistID pid, string define, VariantList args = null)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
        }
    }
}
