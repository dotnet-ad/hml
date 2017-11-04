using System;
namespace Hml.Parser
{
    public class ParsingException : Exception
    {
        public ParsingException(int line, int column, string message) : base(message)
        {
        }

        public int Line { get; }

        public int Column { get; }
    }
}
