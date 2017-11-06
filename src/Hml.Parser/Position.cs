namespace Hml.Parser
{
    public struct Position
    {
        public Position(int line, int column, int start, int length)
        {
            this.Line = line;
            this.Column = column;
            this.Start = start;
            this.Length = length;
        }

        public int Line { get; }

        public int Column { get; }

        public int Start { get; }

        public int Length { get; }
    }
}
