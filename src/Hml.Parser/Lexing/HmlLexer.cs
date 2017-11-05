using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hml.Parser
{
    public class HmlLexer
    {
        public HmlToken[] Tokenize(string content)
        {
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(content);
                writer.Flush();
                stream.Position = 0;
                return this.Tokenize(stream);
            }
        }

        public HmlToken[] Tokenize(Stream stream)
        {
            var result = new List<HmlToken>();
            this.Tokenize(stream, (token) => result.Add(token));
            return result.ToArray();
        }

        private StreamReader reader;

        public void Tokenize(Stream stream, Action<HmlToken> onToken)
        {
            this.position = 0;

            using(this.reader = new StreamReader(stream))
            {
                HmlToken token = null;

                while (token?.Type != HmlTokenType.EndOfDocument)
                {
                    token = ProcessToken();
                    onToken(token);
                }
            }
        }

        private int position;

        private char? Read() 
        {
            if (reader.EndOfStream)
                return null;

            position++;
            return (char)reader.Read();
        }

        private char? Peek() => reader.EndOfStream ? null : (char?)reader.Peek();

        private void SkipWhitespaces()
        {
            char? c;
            while((c = this.Peek()) == ' ')
            {
                this.Read();
            }
        }

        private HmlToken ProcessToken()
        {
            var start = this.position;

            var currentOrEnd = this.Read();

            if(currentOrEnd == null)
            {
                return new HmlToken(HmlTokenType.EndOfDocument, start, start, null);
            }

            HmlTokenType type;
            string content;

            var current = (char)currentOrEnd;
            content = current.ToString();

            switch (current)
            {
                case var letter when char.IsLetter(current):
                case '_':
                    type = HmlTokenType.Identifier;
                    var idBuilder = new StringBuilder();
                    idBuilder.Append(current);
                    char? next;
                    while ((next = this.Peek()) != null && (
                        char.IsLetter(next ?? '0') ||
                          char.IsDigit(next ?? 'A') ||
                          next == '.' || 
                          next == '-' ||
                          next == '_'))
                    {
                        idBuilder.Append(current = (char)this.Read());
                    }
                    content = idBuilder.ToString();
                    break;
                case ' ':
                    type = HmlTokenType.Whitespaces;
                    var wsBuilder = new StringBuilder();
                    while ((next = this.Peek()) == ' ')
                    {
                        wsBuilder.Append(current = (char)this.Read());
                    }
                    content = wsBuilder.ToString();
                    break;
                case '\n':
                case '\r':
                    type =  HmlTokenType.LineReturn;
                    break;
                case '=':
                    type =  HmlTokenType.Equals;
                    break;
                case '"':
                    type =  HmlTokenType.PropertyValue;
                    var builder = new StringBuilder();
                    while((currentOrEnd = this.Read()) != null && currentOrEnd != '"')
                    {
                        // Escape "
                        if(currentOrEnd == '\\' && (next = this.Peek()) == '"')
                        {
                            currentOrEnd = this.Read();
                        }
                        
                        builder.Append(currentOrEnd);
                    }
                    content = builder.ToString();
                    break;
                case ':':
                    type = HmlTokenType.Text;
                    this.SkipWhitespaces();
                    var textBuilder = new StringBuilder();
                    while ((next = this.Peek()) != null && next != '\n' && next != '\r')
                    {
                        textBuilder.Append(currentOrEnd = this.Read());
                    }
                    content = textBuilder.ToString();
                    break;
                case '(':
                    type = HmlTokenType.PropertiesStart;
                    break;
                case ')':
                    type = HmlTokenType.PropertiesEnd;
                    break;
                case ',':
                    type = HmlTokenType.PropertiesSeparator;
                    break;
                default:
                    type = HmlTokenType.Unknown;
                    break;
            }

            var end = this.position;

            return new HmlToken(type, start, end - start, content);
        }
    }
}
