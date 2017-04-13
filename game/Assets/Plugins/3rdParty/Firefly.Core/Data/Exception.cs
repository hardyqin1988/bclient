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

using System;
using System.Diagnostics;
using System.Text;

namespace Firefly.Core.Data
{
    [Serializable]
    internal class EntityException : Exception
    {
        internal EntityException()
        {
        }
        internal EntityException(string message) 
            : base(message)
        {
        }

        internal EntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    [Serializable]
    internal class EventException : Exception
    {
        public EventException()
        {
        }

        public EventException(string message)
            : base(message)
        {
        }

        public EventException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    [Serializable]
    internal class JsonException : Exception
    {
        internal JsonException()
        {
        }
        internal JsonException(string message)
            : base(message)
        {
        }

        internal JsonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    [Serializable]
    internal class DefinitionException : Exception
    {
        internal DefinitionException()
        {
        }
        internal DefinitionException(string message)
            : base(message)
        {
        }

        internal DefinitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    [Serializable]
    internal class ModuleException : Exception
    {
        public ModuleException()
        {
        }
        public ModuleException(string message)
            : base(message)
        {
        }

        public ModuleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    [Serializable]
    public sealed class NetException : Exception
    {
        /// <summary>
        /// NetException constructor
        /// </summary>
        public NetException()
            : base()
        {
        }

        /// <summary>
        /// NetException constructor
        /// </summary>
        public NetException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// NetException constructor
        /// </summary>
        public NetException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Throws an exception, in DEBUG only, if first parameter is false
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert(bool isOk, string message)
        {
            if (!isOk)
                throw new NetException(message);
        }

        /// <summary>
        /// Throws an exception, in DEBUG only, if first parameter is false
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert(bool isOk)
        {
            if (!isOk)
                throw new NetException();
        }
    }

    [Serializable]
    public class INIException : Exception
    {
        public INIException()
        {
        }
        public INIException(string message)
            : base(message)
        {
        }

        public INIException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
