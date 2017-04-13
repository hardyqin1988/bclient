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

using ProtoBuf;
using System;
using System.Runtime.Serialization;

namespace Firefly.Core.Data
{
    [DataContract]
    [Serializable]
    [ProtoContract]
    public struct PersistID : IComparable<PersistID>
    {
        [DataMember]
        [ProtoMember(Constant.PERSISTID_PROTO_IDENT)]
        private Guid _Root;

        [DataMember]
        [ProtoMember(Constant.PERSISTID_PROTO_SERIAL)]
        private Guid _Self;

        public static readonly PersistID Empty = new PersistID();
        
        public static implicit operator PersistID(Guid guid)
        {
            return PersistID.GenRoot(guid);
        }

        public static implicit operator Guid(PersistID pid)
        {
            return pid.Self;
        }

        private static PersistID GenRoot(Guid guid)
        {
            return new PersistID(guid, guid);
        }

        private PersistID(Guid root, Guid self)
        {
            _Root = root;
            _Self = self;
        }

        private PersistID(string s)
        {
            string[] guids = s.Split(Constant.PERSISTID_MEMBERS_SPLIT_FLAG);
            if (guids == null || guids.Length == Constant.PERSISTID_MEMBERS_SIZE)
            {
                _Root = Guid.Empty;
                _Self = Guid.Empty;
            }

            _Root = new Guid(guids[Constant.PERSISTID_PROTO_IDENT-1]);
            _Self = new Guid(guids[Constant.PERSISTID_PROTO_SERIAL-1]);
        }

        public Guid Root { get { return _Root; } }
        public Guid Self { get { return _Self; } }

        public static implicit operator PersistID(string guid)
        {
            return new PersistID(guid);
        }

        public int CompareTo(PersistID other)
        {
            int result = _Root.CompareTo(other._Root);
            if (result != 0)
            {
                return result;
            }

            return _Self.CompareTo(other._Self);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Root.ToString(), Self.ToString());
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(PersistID pid1, PersistID pid2)
        {
            return (pid1.Root == pid2.Root) && (pid1.Self == pid2.Self);
        }

        public static bool operator !=(PersistID pid1, PersistID pid2)
        {
            return !(pid1 == pid2);
        }

        public static PersistID Parse(string g)
        {
            try
            {
                return new PersistID(g);
            }
            catch
            {
                return Empty;
            }
        }

        public static PersistID New()
        {
            Guid guid = Guid.NewGuid();
            PersistID pid = new PersistID(guid, guid);

            return pid;
        }

        public static PersistID New(Guid root)
        {
            PersistID pid = new PersistID(root, root);

            return pid;
        }

        public static PersistID New(Guid root, Guid self)
        {
            PersistID pid = new PersistID(root, self);

            return pid;
        }

        public bool IsRoot
        {
            get { return Root == Self; }
        }
        

        public static bool IsEmpty(PersistID pid)
        {
            return pid == Empty;
        }

        public PersistID Spawn()
        {
            Guid serial = Guid.NewGuid();

            PersistID new_pid = new PersistID(Root, serial);

            return new_pid;
        }
    }
}