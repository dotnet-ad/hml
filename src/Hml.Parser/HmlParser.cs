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

        private HmlTokenizer tokenizer = new HmlTokenizer();

        #endregion

        #region Public methods

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

        private List<HmlToken> tokens;

        public HmlDocument Parse(Stream stream)
        {
            this.tokens = new List<HmlToken>();

            this.tokenizer.Tokenize(stream, ParseNode);

            return new HmlDocument(null, this.tokens.ToArray());
        }

        #endregion

        private HmlTokenType[] expectedNextToken = { HmlTokenType.Whitespaces, HmlTokenType.EndOfDocument, HmlTokenType.LineReturn };

        private void ParseNode(HmlToken token)
        {
            this.tokens.Add(token);


        }
    }
}
