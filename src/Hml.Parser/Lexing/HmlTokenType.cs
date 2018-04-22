namespace Hml.Parser.Lexing
{
    /// <summary>
    /// A token type.
    /// </summary>
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
