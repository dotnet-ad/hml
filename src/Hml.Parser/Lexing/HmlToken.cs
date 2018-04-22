namespace Hml.Parser.Lexing
{
    /// <summary>
    /// A parsed token.
    /// </summary>
    public class HmlToken
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hml.Parser.Lexing.HmlToken"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="line">Line.</param>
        /// <param name="column">Column.</param>
        /// <param name="start">Start.</param>
        /// <param name="length">Length.</param>
        /// <param name="content">Content.</param>
        public HmlToken(HmlTokenType type, int line, int column, int start, int length, string content)
        {
            this.Position = new Position(line, column, start, length);
            this.Type = type;
            this.Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public HmlTokenType Type { get; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public Position Position { get; }

        /// <summary>
        /// Gets the source content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; }

        #endregion
    }
}
