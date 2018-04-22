using System.Collections.Generic;

namespace Hml.Parser
{
    public class HmlDocument
    {
        public HmlDocument(HmlNode root, HmlToken[] tokens)
        {
            this.Root = root;
            this.Tokens = tokens;
        }

        public HmlNode Root { get; }

        public IEnumerable<HmlToken> Tokens { get; }
    }
}
