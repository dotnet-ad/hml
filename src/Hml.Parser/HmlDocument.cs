using System.Collections.Generic;
using Hml.Parser.Lexing;

namespace Hml.Parser
{
    /// <summary>
    /// A document.
    /// </summary>
    public class HmlDocument
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hml.Parser.HmlDocument"/> class.
        /// </summary>
        /// <param name="root">The root node.</param>
        /// <param name="tokens">The parsed tokens.</param>
        public HmlDocument(HmlNode root, HmlToken[] tokens)
        {
            this.Root = root;
            this.Tokens = tokens;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root node.
        /// </summary>
        /// <value>The root.</value>
        public HmlNode Root { get; }

        /// <summary>
        /// Gets the parsed tokens.
        /// </summary>
        /// <value>The tokens.</value>
        public IEnumerable<HmlToken> Tokens { get; }

        #endregion
    }
}
