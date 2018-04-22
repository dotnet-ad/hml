using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using Hml.Parser.Lexing;
using Hml.Parser.Exceptions;

namespace Hml.Parser
{

    /// <summary>
    /// A parser for the hml language.
    /// </summary>
    public class HmlParser
    {
        #region Default

        private static readonly Lazy<HmlParser> instance = new Lazy<HmlParser>(() => new HmlParser());

        public static HmlParser Default => instance.Value;

        #endregion

        #region Fields

        private HmlLexer tokenizer = new HmlLexer();

        private HmlTokenType[] expectedNextToken = { HmlTokenType.Whitespaces, HmlTokenType.EndOfDocument, HmlTokenType.LineReturn };

        private bool isNodeStarted;

        private bool arePropertiesStarted;

        private string nodeName, nodeText;

        private string propertyName;

        private Dictionary<string, string> nodeProperties;

        private int indent;

        private HmlToken lastToken;

        private Position position;

        private List<HmlToken> tokens;

        private Stack<HmlNode> stack;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified content.
        /// </summary>
        /// <returns>The parse.</returns>
        /// <param name="content">Content.</param>
        public HmlDocument Parse(string content)
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

        /// <summary>
        /// Parse the specified stream.
        /// </summary>
        /// <returns>The parse.</returns>
        /// <param name="stream">Stream.</param>
        public HmlDocument Parse(Stream stream)
        {
            this.tokens = new List<HmlToken>();
            this.stack = new Stack<HmlNode>();

            this.tokenizer.Tokenize(stream, ParseNode);

            return new HmlDocument(this.stack.LastOrDefault(), this.tokens.ToArray());
        }

        #endregion

        private void ParseNode(HmlToken token)
        {
            if(token.Type == HmlTokenType.EndOfDocument)
            {
                if(isNodeStarted)
                {
                    this.CreateNode();
                }
                return;
            }

            // Ignoring Whitespaces if not at the beginning of document or line
            if(token.Type == HmlTokenType.Whitespaces)
            {
                if (token.Position.Start == 0 || lastToken?.Type == HmlTokenType.LineReturn)
                {
                    this.tokens.Add(lastToken = token);
                    indent = token.Content.Length;
                }
            }
            else
            {
                if (lastToken == null)
                {
                    if (token.Type == HmlTokenType.Identifier)
                    {
                        this.StartNode(token);
                    }
                    else if(token.Type != HmlTokenType.LineReturn)
                    {
                        // Error : expected <node identifier> or indent
                        this.Throw(token, HmlTokenType.Identifier, HmlTokenType.Whitespaces);
                    }
                }
                else if (lastToken.Type == HmlTokenType.Whitespaces)
                {
                    if (token.Type == HmlTokenType.Identifier)
                    {
                        this.StartNode(token);
                    }
                    else
                    {
                        // Error : expected <node identifier> 
                        this.Throw(token, HmlTokenType.Identifier);
                    }
                }
                else if (lastToken.Type == HmlTokenType.Identifier)
                {
                    if (!arePropertiesStarted)
                    {
                        if (token.Type == HmlTokenType.PropertiesStart)
                        {
                            this.StartProperties();
                        }
                        else if (token.Type == HmlTokenType.Text)
                        {
                            nodeText = token.Content;
                        }
                        else if (token.Type == HmlTokenType.LineReturn)
                        {
                            this.CreateNode();
                        }
                        else
                        {
                            // Error : expected '(' or ':' or <line return>
                            this.Throw(token, HmlTokenType.PropertiesStart, HmlTokenType.Text, HmlTokenType.LineReturn);
                        }
                    }
                    else
                    {
                        if (token.Type == HmlTokenType.Equals)
                        {
                            propertyName = lastToken.Content;
                        }
                        else
                        {
                            // Error : expected '='
                            this.Throw(token, HmlTokenType.Equals);
                        }
                    }
                }
                else if (lastToken.Type == HmlTokenType.Equals)
                {
                    if (token.Type == HmlTokenType.PropertyValue)
                    {
                        this.nodeProperties[propertyName] = token.Content;
                        this.propertyName = null;
                    }
                    else
                    {
                        // Error : expected '"<property value>"'
                        this.Throw(token, HmlTokenType.PropertyValue);
                    }
                }
                else if (lastToken.Type == HmlTokenType.PropertyValue)
                {
                    if (token.Type == HmlTokenType.PropertiesEnd)
                    {
                        this.EndProperties();
                    }
                    else if (token.Type != HmlTokenType.PropertiesSeparator)
                    {
                        // Error : expected ',' or `)`
                        this.Throw(token, HmlTokenType.PropertiesSeparator, HmlTokenType.PropertiesEnd);
                    }
                }
                else if (lastToken.Type == HmlTokenType.PropertiesSeparator)
                {
                    if (token.Type == HmlTokenType.Identifier)
                    {
                        propertyName = token.Content;
                    }
                    else
                    {
                        // Error : expected <identifier>
                        this.Throw(token, HmlTokenType.Identifier);
                    }
                }
                else if (lastToken.Type == HmlTokenType.PropertiesStart)
                {
                    if (token.Type == HmlTokenType.Identifier)
                    {
                        propertyName = token.Content;
                    }
                    else if (token.Type == HmlTokenType.PropertiesEnd)
                    {
                        this.EndProperties();
                    }
                    else
                    {
                        // Error : expected <property identifier> or ')'
                        this.Throw(token, HmlTokenType.Identifier, HmlTokenType.PropertiesEnd);
                    }
                }
                else if (lastToken.Type == HmlTokenType.PropertiesEnd)
                {
                    if (token.Type == HmlTokenType.Text)
                    {
                        this.nodeText = token.Content;
                    }
                    else if (token.Type == HmlTokenType.LineReturn)
                    {
                        this.CreateNode();
                    }
                    else
                    {
                        // Error : expected ':<text>' or '\n'
                        this.Throw(token, HmlTokenType.Text, HmlTokenType.LineReturn);
                    }
                }
                else if (lastToken.Type == HmlTokenType.Text)
                {
                    if (token.Type == HmlTokenType.LineReturn)
                    {
                        this.CreateNode();
                    }
                    else
                    {
                        // Error : expected '\n'
                        this.Throw(token, HmlTokenType.LineReturn);
                    }
                }
                else if (lastToken.Type == HmlTokenType.LineReturn)
                {
                    if (token.Type == HmlTokenType.Identifier)
                    {
                        this.StartNode(token);
                    }
                    else if(token.Type != HmlTokenType.Whitespaces && token.Type != HmlTokenType.LineReturn)
                    {
                        // Error : expected '<node identifier>' or ' ' or '\n'
                        this.Throw(token, HmlTokenType.Identifier, HmlTokenType.Whitespaces, HmlTokenType.LineReturn);
                    }
                }

                this.tokens.Add(lastToken = token);
            }
        }

        private void CreateNode()
        {
            var node = new HmlNode(this.indent, this.nodeName, this.nodeText, this.nodeProperties, this.position);

            if(stack.Count == 0)
            {
                stack.Push(node);
            }
            else
            {
                HmlNode parent = null;

                while (stack.Count > 0 && (parent = stack.First()).Indent >= node.Indent)
                {
                    parent = stack.Pop();
                }

                if(stack.Count == 0)
                {
                    throw new HmlParsingException(lastToken, "node has a greater level than the root node");
                }

                stack.Push(node);
                parent.Add(node);
            }

            this.EndNode();
        }

        private void StartNode(HmlToken name)
        {
            this.nodeName = name.Content;
            this.position = name.Position;
            this.nodeText = null;
            this.nodeProperties = null;
            this.isNodeStarted = true;
        }

        private void EndNode() 
        {
            this.isNodeStarted = false;
            this.indent = 0;
            this.nodeName = null;
            this.position = default(Position);
        }

        private void StartProperties() 
        {
            this.arePropertiesStarted = true; 
            this.nodeProperties = new Dictionary<string, string>();
        }

        private void EndProperties()
        {
            this.arePropertiesStarted = false;
        }

        private void Throw(HmlToken token, params HmlTokenType[] expected)
        {
            throw new HmlInvalidTokenParsingException(token, this.lastToken, expected);
        }
    }
}
