using System;
using NUnit.Framework;
using Hml.Parser;
using System.Linq;

namespace Hml.Tests
{
    [TestFixture()]
    public class LexerTests
    {
        private HmlLexer lexer;

        [SetUp]
        public void Initialize()
        {
            this.lexer = new HmlLexer();
        }

        [TearDown]
        public void Destroy()
        {
            this.lexer = null;
        }

        [Test]
        public void Tokenize_ValidSequence_Succeed()
        {
            var hml = @"node.test (prop=""v"", other=""otherv""): the value text of the element
  other_1";

            var tokens = this.lexer.Tokenize(hml);

            Assert.AreEqual(HmlTokenType.Identifier, tokens[0].Type);
            Assert.AreEqual("node.test", tokens[0].Content);
            Assert.AreEqual(0, tokens[0].Start);
            Assert.AreEqual(9, tokens[0].Length);

            Assert.AreEqual(HmlTokenType.Whitespaces, tokens[1].Type);

            Assert.AreEqual(HmlTokenType.PropertiesStart, tokens[2].Type);

            Assert.AreEqual(HmlTokenType.Identifier, tokens[3].Type);
            Assert.AreEqual("prop", tokens[3].Content);

            Assert.AreEqual(HmlTokenType.Equals, tokens[4].Type);

            Assert.AreEqual(HmlTokenType.PropertyValue, tokens[5].Type);
            Assert.AreEqual("v", tokens[5].Content);

            Assert.AreEqual(HmlTokenType.PropertiesSeparator, tokens[6].Type);

            Assert.AreEqual(HmlTokenType.Whitespaces, tokens[7].Type);

            Assert.AreEqual(HmlTokenType.Identifier, tokens[8].Type);
            Assert.AreEqual("other", tokens[8].Content);

            Assert.AreEqual(HmlTokenType.Equals, tokens[9].Type);

            Assert.AreEqual(HmlTokenType.PropertyValue, tokens[10].Type);
            Assert.AreEqual("otherv", tokens[10].Content);

            Assert.AreEqual(HmlTokenType.PropertiesEnd, tokens[11].Type);

            Assert.AreEqual(HmlTokenType.Text, tokens[12].Type);
            Assert.AreEqual("the value text of the element", tokens[12].Content);

            Assert.AreEqual(HmlTokenType.LineReturn, tokens[13].Type);

            Assert.AreEqual(HmlTokenType.Whitespaces, tokens[14].Type);

            Assert.AreEqual(HmlTokenType.Identifier, tokens[15].Type);
            Assert.AreEqual("other_1", tokens[15].Content);

            Assert.AreEqual(HmlTokenType.EndOfDocument, tokens[16].Type);
        }

        [Test]
        public void Tokenize_EscapedChar_Succeed()
        {
            var hml = @"""test\""test""";

            var tokens = this.lexer.Tokenize(hml);

            Assert.AreEqual(HmlTokenType.PropertyValue, tokens[0].Type);
            Assert.AreEqual("test\"test", tokens[0].Content);
        }
    }
}
