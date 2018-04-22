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