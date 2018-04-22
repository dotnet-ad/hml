using Hml.Parser.Lexing;
using System.Linq;

namespace Hml.Parser.Exceptions
{
    public class HmlInvalidTokenParsingException : HmlParsingException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hml.Parser.Exceptions.HmlInvalidTokenParsingException"/> class.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="previousToken">Previous token.</param>
        /// <param name="expected">Expected.</param>
        public HmlInvalidTokenParsingException(HmlToken token, HmlToken previousToken, params HmlTokenType[] expected) : base(token, $"got token {GetTokenInfo(token.Type)} at position [{token.Position.Line}, {token.Position.Column}] (following { (previousToken != null ? GetTokenInfo(previousToken.Type) : "start")}), expected : { string.Join(", ", expected.Select(x => GetTokenInfo(x))) }")
        {
            this.PreviousToken = previousToken;
            this.ExpectedTokens = expected;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the previous token.
        /// </summary>
        /// <value>The previous token.</value>
        public HmlToken PreviousToken { get; }

        /// <summary>
        /// Gets the expected tokens.
        /// </summary>
        /// <value>The expected tokens.</value>
        public HmlTokenType[] ExpectedTokens { get; }

        #endregion

        #region Methods

        private static string GetTokenInfo(HmlTokenType type)
        {
            switch (type)
            {
                case HmlTokenType.EndOfDocument:
                    return "<end of document>";
                case HmlTokenType.Equals:
                    return "'='";
                case HmlTokenType.Identifier:
                    return "<identifier>";
                case HmlTokenType.Indent:
                case HmlTokenType.Whitespaces:
                    return "' '";
                case HmlTokenType.LineReturn:
                    return "<line return>";
                case HmlTokenType.PropertiesEnd:
                    return "')'";
                case HmlTokenType.PropertiesSeparator:
                    return "','";
                case HmlTokenType.PropertiesStart:
                    return "'('";
                case HmlTokenType.PropertyValue:
                    return "'\"'<value>'\"'";
                case HmlTokenType.Text:
                    return ":'<value>";
                default:
                    return "?";
            }
        }

        #endregion
    }
}
