namespace Hml.Parser
{
    public enum HmlTokenType : byte
    {
        Unknown,
        Whitespaces,
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
