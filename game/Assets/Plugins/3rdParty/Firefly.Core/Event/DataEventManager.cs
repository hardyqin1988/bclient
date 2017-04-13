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
using System;

namespace Firefly.Core.Event
{
    public class DataEventManager<DATA_EVENT> : EventManager<Group<string, string, int>>
    {
        internal void Callback(DATA_EVENT event_enum, string entity_type, string name,
            IKernel kernel, PersistID self, VariantList args)
        {
            int event_id = (int)Convert.ChangeType(event_enum, TypeCode.Int32);

            Group<string, string, int> group
                = new Group<string, string, int>(entity_type, name, event_id);

            Callback(group, kernel, self, args);
        }

        internal void Register(string entity_type, string name, DATA_EVENT event_enum, EventCallback event_handler)
        {
            int event_id = (int)Convert.ChangeType(event_enum, TypeCode.Int32);

            Group<string, string, int> group
                = new Group<string, string, int>(entity_type, name, event_id);

            Register(group, event_handler);
        }
    }
}
