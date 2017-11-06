namespace Hml.Parser.Lexing
{
    public class HmlToken
    {
        public HmlToken(HmlTokenType type, int line, int column, int start, int length, string content)
        {
            this.Position = new Position(line,column, start, length);
            this.Type = type;
            this.Content = content;
        }

        public HmlTokenType Type { get; }

        public Position Position { get; }

        public string Content { get; }
    }
}
