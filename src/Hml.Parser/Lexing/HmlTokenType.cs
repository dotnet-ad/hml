namespace Hml.Parser.Lexing
{
    public enum HmlTokenType : byte
    {
        Unknown,
        Whitespaces,
        Indent,
        Identifier,
        PropertiesStart,
        PropertiesEnd,
        PropertiesSeparator,
        Equals,
        PropertyValue,
        Text,
        LineReturn,
        EndOfDocument,
    }
}
