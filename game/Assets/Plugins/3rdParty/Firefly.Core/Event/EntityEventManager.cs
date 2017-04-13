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

namespace Firefly.Core.Event
{
    class EntityEventManager : EventManager<Group<string, EntityEvent>>
    {
        internal void Callback(string entity_type, EntityEvent event_enum,
            IKernel kernel, PersistID self, VariantList args)
        {
            Group<string, EntityEvent> group
                = new Group<string, EntityEvent>(entity_type, event_enum);

            Callback(group, kernel, self, args);
        }

        internal void Register(string entity_type, EntityEvent event_enum, EventCallback event_handler)
        {
            Group<string, EntityEvent> group
                = new Group<string, EntityEvent>(entity_type, event_enum);

            Register(group, event_handler);
        }
    }
}
