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
using System;
using System.Collections.Generic;
using System.Text;

namespace Firefly.Core.Kernel
{
    public class ModuleManager
    {
        public ModuleManager(IKernel kernel)
        {
            _Kernel = kernel;
            _ModuleDic = new Dictionary<string, IModule>();
        }

        private IKernel _Kernel;
        private Dictionary<string, IModule> _ModuleDic;

        public IModule GetModule(string name)
        {
            IModule found = null;

            _ModuleDic.TryGetValue(name, out found);

            return found;
        }

        public void CreateModule<T>() where T : IModule
        {
            try
            {
                Type type = typeof(T);
                string name = type.ToString();
                IModule module = Activator.CreateInstance(type) as IModule;

                if (module == null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Create {0} Failed.", name);
                    throw new ModuleException(sb.ToString());
                }
                else if (_ModuleDic.ContainsKey(type.ToString()))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0} is exist.", name);
                    throw new ModuleException(sb.ToString());
                }
                else
                {
                    _ModuleDic.Add(name, module);
                    module.Create(_Kernel);
                    _Kernel.Info("CreateModule {0} Success", name);
                }
            }
            catch (ModuleException ex)
            {
                throw new ModuleException("Error Module Because ", ex);
            }
        }
    }
}
