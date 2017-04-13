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
using Firefly.Core.Variant;
using ProtoBuf;

namespace Firefly.Core.Interface
{
    [ProtoContract]
    [ProtoInclude(Constant.DATA_PROTO_ENTITY, typeof(Entity))]
    public interface IEntity
    {
        PersistID Self { get; }

        string Type { get; }

        bool Online { get; set;  }

        void Clear();

        bool FindProperty(string property_name);
        bool FindRecord(string record_name);

        IProperty GetProperty(string property_name);
        IRecord GetRecord(string record_name);

        IProperty[] GetProperties();
        IRecord[] GetRecords();

        IProperty CreateProperty(string property_name, VariantType property_type);
        IRecord CreateRecord(string record_name, VariantList col_types);
    }
}
