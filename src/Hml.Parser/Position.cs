namespace Hml.Parser
{
    /// <summary>
    /// A text portion position in a source file.
    /// </summary>
    public struct Position
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hml.Parser.Position"/> struct.
        /// </summary>
        /// <param name="line">Line.</param>
        /// <param name="column">Column.</param>
        /// <param name="start">Start.</param>
        /// <param name="length">Length.</param>
        public Position(int line, int column, int start, int length)
        {
            this.Line = line;
            this.Column = column;
            this.Start = start;
            this.Length = length;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the line number.
        /// </summary>
        /// <value>The line.</value>
        public int Line { get; }

        /// <summary>
        /// Gets the column number.
        /// </summary>
        /// <value>The column.</value>
        public int Column { get; }

        /// <summary>
        /// Gets the start character index.
        /// </summary>
        /// <value>The start.</value>
        public int Start { get; }

        /// <summary>
        /// Gets the number of characters.
        /// </summary>
        /// <value>The length.</value>
        public int Length { get; }

        #endregion
    }
}
