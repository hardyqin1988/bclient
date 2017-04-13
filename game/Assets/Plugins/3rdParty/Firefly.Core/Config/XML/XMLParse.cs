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
using System.Collections.Generic;

namespace Firefly.Core.Config.XML
{
    /// <summary>
    /// Base class containing usefull features for all XML classes
    /// </summary>
    public class XMLBase
    {
        protected static bool IsSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        protected static void SkipSpaces(string str, ref int i)
        {
            while (i < str.Length)
            {
                if (!IsSpace(str[i]))
                {
                    if (str[i] == '<' && i + 4 < str.Length && str[i + 1] == '!' && str[i + 2] == '-' && str[i + 3] == '-')
                    {
                        i += 4; // skip <!--

                        while (i + 2 < str.Length && !(str[i] == '-' && str[i + 1] == '-'))
                            i++;

                        i += 2; // skip --
                    }
                    else
                        break;
                }

                i++;
            }
        }

        protected static string GetValue(string str, ref int i, char endChar, char endChar2, bool stopOnSpace)
        {
            int start = i;
            while ((!stopOnSpace || !IsSpace(str[i])) && str[i] != endChar && str[i] != endChar2) i++;

            return str.Substring(start, i - start);
        }

        protected static bool IsQuote(char c)
        {
            return c == '"' || c == '\'';
        }

        // returns name
        protected static string ParseAttributes(string str, ref int i, List<XMLAttribute> attributes, char endChar, char endChar2)
        {
            SkipSpaces(str, ref i);
            string name = GetValue(str, ref i, endChar, endChar2, true);

            SkipSpaces(str, ref i);

            while (str[i] != endChar && str[i] != endChar2)
            {
                string attrName = GetValue(str, ref i, '=', '\0', true);

                SkipSpaces(str, ref i);
                i++; // skip '='
                SkipSpaces(str, ref i);

                char quote = str[i];
                if (!IsQuote(quote))
                    throw new XMLParsingException("Unexpected token after " + attrName);

                i++; // skip quote
                string attrValue = GetValue(str, ref i, quote, '\0', false);
                i++; // skip quote

                attributes.Add(new XMLAttribute(attrName, attrValue));

                SkipSpaces(str, ref i);
            }

            return name;
        }
    }

    /// <summary>
    /// Class representing whole DOM XML document
    /// </summary>
    public class XMLDocument : XMLBase
    {
        private XMLNode rootNode;
        private List<XMLAttribute> declarations = new List<XMLAttribute>();
        /// <summary>
        /// Public constructor. Loads xml document from raw string
        /// </summary>
        /// <param name="xmlString">String with xml</param>
        public XMLDocument(string xmlString)
        {
            int i = 0;

            while (true)
            {
                SkipSpaces(xmlString, ref i);

                if (xmlString[i] != '<')
                    throw new XMLParsingException("Unexpected token");

                i++; // skip <

                if (xmlString[i] == '?') // declaration
                {
                    i++; // skip ?
                    ParseAttributes(xmlString, ref i, declarations, '?', '>');
                    i++; // skip ending ?
                    i++; // skip ending >

                    continue;
                }

                if (xmlString[i] == '!') // doctype
                {
                    while (xmlString[i] != '>') // skip doctype
                        i++;

                    i++; // skip >

                    continue;
                }

                rootNode = new XMLNode(xmlString, ref i);
                break;
            }
        }
        /// <summary>
        /// Root document element
        /// </summary>
        public XMLNode RootNode
        {
            get { return rootNode; }
        }
        /// <summary>
        /// List of XML Declarations as <see cref="XMLAttribute"/>
        /// </summary>
        public IEnumerable<XMLAttribute> Declarations
        {
            get { return declarations; }
        }
    }

    /// <summary>
    /// Element node of document
    /// </summary>
    public class XMLNode : XMLBase
    {
        private string value;
        private string name;

        private List<XMLNode> subNodes = new List<XMLNode>();
        private List<XMLAttribute> attributes = new List<XMLAttribute>();

        internal XMLNode(string str, ref int i)
        {
            name = ParseAttributes(str, ref i, attributes, '>', '/');

            if (str[i] == '/') // if this node has nothing inside
            {
                i++; // skip /
                i++; // skip >
                return;
            }

            i++; // skip >

            // temporary. to include all whitespaces into value, if any
            int tempI = i;

            SkipSpaces(str, ref tempI);

            if (str[tempI] == '<')
            {
                i = tempI;

                while (str[i + 1] != '/') // parse subnodes
                {
                    i++; // skip <
                    subNodes.Add(new XMLNode(str, ref i));

                    SkipSpaces(str, ref i);

                    if (i >= str.Length)
                        return; // EOF

                    if (str[i] != '<')
                        throw new XMLParsingException("Unexpected token");
                }

                i++; // skip <
            }
            else // parse value
            {
                value = GetValue(str, ref i, '<', '\0', false);
                i++; // skip <

                if (str[i] != '/')
                    throw new XMLParsingException("Invalid ending on tag " + name);
            }

            i++; // skip /
            SkipSpaces(str, ref i);

            string endName = GetValue(str, ref i, '>', '\0', true);
            if (endName != name)
                throw new XMLParsingException("Start/end tag name mismatch: " + name + " and " + endName);
            SkipSpaces(str, ref i);

            if (str[i] != '>')
                throw new XMLParsingException("Invalid ending on tag " + name);

            i++; // skip >
        }
        /// <summary>
        /// Element value
        /// </summary>
        public string Value
        {
            get { return value; }
        }
        /// <summary>
        /// Element name
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// List of subelements
        /// </summary>
        public IEnumerable<XMLNode> SubNodes
        {
            get { return subNodes; }
        }
        /// <summary>
        /// List of attributes
        /// </summary>
        public IEnumerable<XMLAttribute> Attributes
        {
            get { return attributes; }
        }

        public string GetValue(string name)
        {
            foreach (var xml_attr in Attributes)
            {
                if (xml_attr.Name.Equals(name))
                {
                    return xml_attr.Value;
                }
            }

            return string.Empty;
        }
        /// <summary>
        /// Returns subelement by given name
        /// </summary>
        /// <param name="nodeName">Name of subelement to get</param>
        /// <returns>First subelement with given name or NULL if no such element</returns>
        public XMLNode this[string nodeName]
        {
            get
            {
                foreach (XMLNode nanoXmlNode in subNodes)
                    if (nanoXmlNode.name == nodeName)
                        return nanoXmlNode;

                return null;
            }
        }
        /// <summary>
        /// Returns attribute by given name
        /// </summary>
        /// <param name="attributeName">Attribute name to get</param>
        /// <returns><see cref="XMLAttribute"/> with given name or null if no such attribute</returns>
        public XMLAttribute GetAttribute(string attributeName)
        {
            foreach (XMLAttribute nanoXmlAttribute in attributes)
                if (nanoXmlAttribute.Name == attributeName)
                    return nanoXmlAttribute;

            return null;
        }
    }

    /// <summary>
    /// XML element attribute
    /// </summary>
    public class XMLAttribute
    {
        private string name;
        private string value;
        /// <summary>
        /// Attribute name
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Attribtue value
        /// </summary>
        public string Value
        {
            get { return value; }
        }

        internal XMLAttribute(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }

    [Serializable]
    class XMLParsingException : Exception
    {
        public XMLParsingException(string message) : base(message)
        {
        }
    }
}