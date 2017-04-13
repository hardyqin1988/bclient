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
using System;
using System.Collections.Generic;

namespace Firefly.Core.Variant
{
    internal class VariantPool
    {
        private static Queue<VariantList> _VariantLists = new Queue<VariantList>();

        private static Dictionary<VariantType, Queue<IVariant>> _VariantElements = new Dictionary<VariantType, Queue<IVariant>>();

        internal static VariantList NewList()
        {
            if (_VariantLists.Count == 0)
            {
                return new VariantList();
            }

            return _VariantLists.Dequeue();
        }

        internal static VARIANT NewVariant<VARIANT>(VariantType variant_type) where VARIANT : IVariant
        {
            Queue<IVariant> found = null;
            if (_VariantElements.TryGetValue(variant_type, out found) && found.Count > 0)
            {
                return (VARIANT)found.Dequeue();
            }

            IVariant variant = Activator.CreateInstance<VARIANT>();
            return (VARIANT)variant;
        }

        internal static void Recycle<VARIANT>(VARIANT variant) where VARIANT : IVariant
        {
            Queue<IVariant> found = null;
            if (!_VariantElements.TryGetValue(variant.Type, out found))
            {
                found = new Queue<IVariant>();
                _VariantElements.Add(variant.Type, found);
            }

            variant.Clear();
            found.Enqueue(variant);
        }

        internal static void Recycle(VariantList variant_list)
        {
            for (int i = 0; i < variant_list.Count; i++)
            {
                IVariant variant = variant_list[i];
                Recycle(variant);
            }

            variant_list.Clear();

            _VariantLists.Enqueue(variant_list);
        }
    }
}
