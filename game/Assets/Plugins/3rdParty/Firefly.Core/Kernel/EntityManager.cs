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
using Firefly.Core.Interface;
using System.Collections.Generic;

namespace Firefly.Core.Kernel
{
    public class EntityManager
    {
        private Dictionary<PersistID, Entity> _EntityDic;

        private Entity _Entity;

        private IKernel _Kernel;

        public EntityManager(IKernel kernel)
        {
            _Kernel = kernel;
            _EntityDic = new Dictionary<PersistID, Entity>();
            _Entity = null;
        }

        #region ------ ------ ------ ------Entity------ ------ ------ ------
        public Entity GetEntity(PersistID pid)
        {
            if (_Entity != null && _Entity.Self == pid)
            {
                return _Entity;
            }

            _EntityDic.TryGetValue(pid, out _Entity);
            return _Entity;
        }

        public void AddEntity(Entity entity)
        {
            if (_Entity != null && _EntityDic.ContainsKey(entity.Self))
            {
                return;
            }

            _EntityDic.Add(entity.Self, entity);
        }

        public bool FindEntity(PersistID pid)
        {
            if (_Entity != null && _Entity.Self == pid)
            {
                return true;
            }

            return _EntityDic.ContainsKey(pid);
        }

        private Entity GenEntity(string name)
        {
            EntityDef definition = _Kernel.GetEntityDef(name);
            if (definition == null)
            {
                return null;
            }

            Entity new_entity = new Entity();
            new_entity.Type = definition.Name;

            foreach (PropertyDef property_def in definition.GetAllProperties())
            {
                byte type = property_def.Define.GetByte(Constant.FLAG_TYPE);
                new_entity.CreateProperty(property_def.Name, (VariantType)type);
            }

            foreach (RecordDef record_def in definition.GetAllRecords())
            {
                new_entity.CreateRecord(record_def.Name, record_def.ColTypes);
            }

            return new_entity;
        }

        public IEntity CreateEntity(PersistID pid, string name)
        {
            if (PersistID.IsEmpty(pid))
            {
                return null;
            }

            if (_EntityDic.ContainsKey(pid))
            {
                return null;
            }

            Entity new_entity = GenEntity(name);
            if (new_entity == null)
            {
                return null;
            }

            _EntityDic.Add(pid, new_entity);
            new_entity.Self = pid;

            return new_entity;
        }

        public IEntity CreateEntity(string name, PersistID root)
        {
            PersistID pid = root.Spawn();

            return CreateEntity(pid, name);
        }

        public bool DelEntity(PersistID pid)
        {
            Entity found = null;
            if (_EntityDic.TryGetValue(pid, out found))
            {
                found.Clear();
            }

            return _EntityDic.Remove(pid);
        }
        #endregion
    }
}
