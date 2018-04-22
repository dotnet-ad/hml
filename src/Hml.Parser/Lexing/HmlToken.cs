namespace Hml.Parser
{
    public class HmlToken
    {
        public HmlToken(HmlTokenType type, int start, int length, string content)
        {
            this.Type = type;
            this.Start = start;
            this.Length = length;
            this.Content = content;
        }

        public HmlTokenType Type { get; }

        public int Start { get; }

        public int Length { get; }

        public string Content { get; }
    }
}
