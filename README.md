# hml

[![NuGet](https://img.shields.io/nuget/v/Hml.svg?label=NuGet)](https://www.nuget.org/packages/Hml/) [![Donate](https://img.shields.io/badge/donate-paypal-yellow.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ZJZKXPPGBKKAY&lc=US&item_name=GitHub&item_number=0000001&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted)

Hierarchy Markup Language, or less verbose xml.

## Quickstart

```
node(property="value", other="another value"): this is the content
  child: content of the child
  child2
  	child21(prop="v1")
  child3(prop="v3"): another content
```

```csharp
var node = HmlParser.Default.Parse(hmlContent).Root;
var other = node["other"]; //another value
var child = node.First(); 
var text = child.Text; //content of the child
```

## Usage

### Writing a document

An `hml` document should be structured following those rules :

* A unique root node
* One node per line
* A node must have a name
* A node can have properties
* A node property must have a key and a string value.
* A node can have text content
* The parent of a node is the first node in the tree that has an indentation level lower than the current node (except for the first one in the document which the root node)

### Parser

The parser will load an `hml` content as an `HmlDocument`, which is  a tree of `HmlNode`.

###### Loading a document from a string

```csharp
var hml = "node(property=\"value\"): sample\n child: test";
var document = HmlParser.Default.Parse(hml);
```

###### Accessing nodes

```csharp
HmlNode node = document.Root;
HmlNode child = node[0];
```

###### Accessing properties

```csharp
string value = node["property"];
```

###### Accessing text

```csharp
string text = node.Text;
```

### Lexer

The lexer splits a document into tokens. You shouldn't use the lexer until you want to analyze the document structure more precisely (*for example for pretty printing an hml document*).

###### Loading tokens

```csharp
var hml = "node(property=\"value\")";
HmlToken[] tokens = HmlLexer.Default.Tokenize(hmlContent);
```

###### Accessing tokens

```csharp
var nodeName = tokens[0];
var startProperties = tokens[1];
var propertyName = tokens[2];
var equals = tokens[3];
var propertyValue = tokens[4];
var endProperties = tokens[5];
```

###### Accessing token data

```csharp
var type = nodeName.Type; // HmlTokenType.Identifier
var content = nodeName.Content; // "node"
var line = nodeName.Position.Line; // 0
var column = nodeName.Position.Column; // 0
```

### Parsing exceptions

In case of a parsing error an `HmlParsingException` that will contains token information (*position in content, expected token, ...*).

## Specification

A `.hml` describe a tree where each node has a name,  set of properties (with a name and a string value), a value (string), a list of descendants.

* Node : `<Indent>``<Name>``<Properties>`? (`:` `<Text>`)?
* Indent : `( )*`
* Whitespaces : `( )*`
* Name : `<Letter>|_``<Letter>|<digit>|.|_|-`*
* Properties : `(``<Property>`(`,``<Property>`)*`)`
* Property : `<Name>``=``"``<Value>``"`
* Value : (`^"`)*
* Text : (`^\r`|`^\n`|`^\r\n`)*

## About

You might guess why I felt the need for a new data description model while there is already JSON, XML, YAML and so on. Because I like XML for the way its describing a tree of elements, but I find it may be too verbose. For example, I liked the simplicity of template engines like [pug](https://pugjs.org/).

## Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

## License

MIT © [Aloïs Deniel](http://aloisdeniel.github.io)