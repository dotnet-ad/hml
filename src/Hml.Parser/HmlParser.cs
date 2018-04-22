using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System;

namespace Hml.Parser
{
    public class HmlParser
    {
        #region Default

        private static readonly Lazy<HmlParser> instance;

        public static HmlParser Default => instance.Value;

        #endregion

        #region Fields

        private int line, column;

        private StreamReader reader;

        private static readonly char[] NameAuthorizedSpecialChars = { '_', '.', '-' };

        #endregion

        #region Public methods

        public HmlNode Parse(string content)
        {
            using(MemoryStream stream = new MemoryStream())
            using(StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(content);
                writer.Flush();
                stream.Position = 0;
                return this.Parse(stream);
            }
        }

        public HmlNode Parse(Stream stream)
        {
            this.line = 0;
            this.column = 0;

            var stack = new Stack<HmlNode>();

            using (this.reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var node = this.ReadNode();

                    if(node != null)
                    {
                        HmlNode parent = null;

                        while (stack.Count > 0 && (parent = stack.First()).Indent >= node.Indent)
                        {
                            parent = stack.Pop();
                        }

                        parent?.Add(node);
                        stack.Push(node);
                    }
                }
            }

            this.reader = null;

            return stack.LastOrDefault();
        }

        #endregion

        #region Private methods

        private HmlNode ReadNode()
        {
            var indent = this.ReadChars(' ');

            if(this.TryReadNewLine())
            {
                return null;
            }

            var nodeLine = this.line;
            var nodeColumn = this.column;
            var name = this.ReadName();
            var node = new HmlNode(indent,name)
            {
                Line = nodeLine,
                Column = nodeColumn,
            };

            this.SkipWhitespaces();
            
            this.ReadProperties(node);

            this.ReadText(node);

            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ReadText(HmlNode node)
        {
            this.SkipWhitespaces();

            var hasText = this.TryReadChar(':');

            this.Assert(hasText || this.TryReadNewLine() || reader.EndOfStream, "expected character ':', new line or end of stream");

            if(hasText)
            {
                this.SkipWhitespaces();

                var builder = new StringBuilder();

                while (!reader.EndOfStream && !TryReadNewLine())
                {
                    builder.Append(this.Read());
                }

                node.Text = builder.ToString();
            }

            return hasText;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ReadProperties(HmlNode node)
        {
            if (reader.EndOfStream)
                return false;
            
            if (this.TryReadChar('('))
            {
                while (EnsureNotEnd())
                {
                    this.SkipWhitespaces();

                    var property = ReadProperty();

                    this.SkipWhitespaces();

                    node[property.Key] = property.Value;

                    if (this.TryReadChar(')'))
                        break;

                    this.Assert(this.TryReadChar(','), "expected character ')' or ','", true);
                }
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SkipWhitespaces() => ReadChars(' ');

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SkipReturns() => ReadChars('\n', '\r');

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ReadChars(params char[] c)
        {
            var count = 0;
            while (!reader.EndOfStream && c.Contains(this.Peek()))
            {
                this.Read();
                count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Assert(bool condition, string message, bool isPeeked = false) 
        {
            if(!condition)
                throw new ParsingException(this.line, this.column + (isPeeked ? 1 : 0), message);
        }

        private bool TryReadChar(char c)
        {
            var pc = this.Peek();

            if (pc == c)
            {
                this.Read();
                return true;
            }

            return false;
        }

        private bool TryReadNewLine()
        {
            var result = false;

            if(this.TryReadChar('\n'))
            {
                result = true;
            }
            else if (this.TryReadChar('\r'))
            {
                this.TryReadChar('\n');
                result = true;
            }

            if(result)
            {
                column = 0;
                line++;
            }

            return result;
        }

        private char Peek() => (char)reader.Peek();

        private char Read() 
        {
            this.column++;
            return (char)reader.Read();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadName()
        {
            this.EnsureNotEnd();

            var builder = new StringBuilder();

            var c = this.Peek();

            this.Assert(char.IsLetter(c) || c == '_', "names must begin with a letter or '_'", true);

            builder.Append(this.Read());

            while (!reader.EndOfStream && (char.IsLetterOrDigit(c = this.Peek()) || NameAuthorizedSpecialChars.Contains(c)))
            {
                builder.Append((char)reader.Read());
            }

            return builder.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private KeyValuePair<string, string> ReadProperty()
        {
            var name = this.ReadName();

            this.SkipWhitespaces();

            this.EnsureNotEnd();

            this.Assert(this.TryReadChar('='), "expected character '=' for setting property value", true);

            this.SkipWhitespaces();

            var value = this.ReadPropertyValue();

            return new KeyValuePair<string, string>(name, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadPropertyValue()
        {
            var builder = new StringBuilder();

            this.EnsureNotEnd();

            this.Assert(this.TryReadChar('"'), "expected character '\"' for starting a property value", true);

            char c;
            while (EnsureNotEnd() && (c = this.Read()) != '"')
            {
                builder.Append(c);
            }

            return builder.ToString();
        }

        private bool EnsureNotEnd()
        {
            this.Assert(!reader.EndOfStream, "reached the end of the stream");
            return true;
        }

        #endregion
    }
}
