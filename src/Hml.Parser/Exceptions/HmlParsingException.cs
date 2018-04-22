using Hml.Parser.Lexing;

namespace Hml.Parser.Exceptions
{
    public class HmlParsingException : System.Exception
    {
        public HmlParsingException(HmlToken token, string message) : base(message)
        {
            this.Token = token;
        }

        public HmlToken Token { get; }
    }
}
