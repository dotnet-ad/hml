using NUnit.Framework;
using Hml.Parser;
using System.Linq;
namespace Hml.Tests
{
    [TestFixture()]
    public class ParserTests
    {
        private HmlParser parser;

        [SetUp]
        public void Initialize()
        {
            this.parser = new HmlParser();
        }

        [TearDown]
        public void Destroy()
        {
            this.parser = null;
        }

        #region Single node

        [Test]
        public void Parse_SingleNode_Succeed()
        {
            var hml = "test";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNullOrEmpty(root.Root.Text);
            Assert.IsEmpty(root.Root.Properties);
        }

        [Test]
        public void Parse_SingleNodeWithText_Succeed()
        {
            var hml = "test: great sample!";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsEmpty(root.Root.Properties);
            Assert.AreEqual("great sample!", root.Root.Text);
        }

        [Test]
        public void Parse_SingleNodeWithProperty_Succeed()
        {
            var hml = "test(prop=\"propv\")";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNotEmpty(root.Root.Properties);
            Assert.AreEqual("propv", root.Root["prop"]);
        }

        [Test]
        public void Parse_SingleNodeWithProperties_Succeed()
        {
            var hml = "test(prop1=\"propv1\", prop2=\"propv2\")";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNotEmpty(root.Root.Properties);
            Assert.AreEqual("propv1", root.Root["prop1"]);
            Assert.AreEqual("propv2", root.Root["prop2"]);
        }

        [Test]
        public void Parse_SingleNodeWithPropertiesAndText_Succeed()
        {
            var hml = "test(prop1=\"propv1\", prop2=\"propv2\"): great sample!";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNotEmpty(root.Root.Properties);
            Assert.AreEqual("propv1", root.Root["prop1"]);
            Assert.AreEqual("propv2", root.Root["prop2"]);
            Assert.AreEqual("great sample!", root.Root.Text);
        }

        [Test]
        public void Parse_SingleNodeWithPropertiesAndTextAndExtraSpaces_Succeed()
        {
            var hml = "test  (    prop1 =  \"propv1\"   ,   prop2  =\"propv2\"   )  :    great sample!";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNotEmpty(root.Root.Properties);
            Assert.AreEqual("propv1", root.Root["prop1"]);
            Assert.AreEqual("propv2", root.Root["prop2"]);
            Assert.AreEqual("great sample!", root.Root.Text);
        }

        #endregion

        #region Multiple nodes

        [Test]
        public void Parse_OneLevelNodes_Succeed()
        {
            var hml = @"test(prop1=""propv1"", prop2=""propv2""): great sample!
  child(cp=""v""): child text";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNotEmpty(root.Root.Properties);
            Assert.AreEqual("propv1", root.Root["prop1"]);
            Assert.AreEqual("propv2", root.Root["prop2"]);
            Assert.AreEqual("great sample!", root.Root.Text);

            Assert.IsNotEmpty(root.Root);
            var child = root.Root.First();
            Assert.AreEqual("child", child.Name);
            Assert.IsNotEmpty(child.Properties);
            Assert.AreEqual("v", child["cp"]);
            Assert.AreEqual("child text", child.Text);
        }

        [Test]
        public void Parse_TwoLevelNodes_Succeed()
        {
            var hml = @"test(prop1=""propv1"", prop2=""propv2""): great sample!
  child1(cp=""v1""): child1 text
    child11(cp=""v11""): child11 text
  child2(cp=""v2""): child2 text";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNotEmpty(root.Root.Properties);
            Assert.AreEqual("propv1", root.Root["prop1"]);
            Assert.AreEqual("propv2", root.Root["prop2"]);
            Assert.AreEqual("great sample!", root.Root.Text);

            Assert.IsNotEmpty(root.Root);
            var child1 = root.Root.First();
            Assert.AreEqual("child1", child1.Name);
            Assert.IsNotEmpty(child1.Properties);
            Assert.AreEqual("v1", child1["cp"]);
            Assert.AreEqual("child1 text", child1.Text);

            Assert.IsNotEmpty(child1);
            var child11 = child1.First();
            Assert.AreEqual("child11", child11.Name);
            Assert.IsNotEmpty(child11.Properties);
            Assert.AreEqual("v11", child11["cp"]);
            Assert.AreEqual("child11 text", child11.Text);
            Assert.IsEmpty(child11);

            Assert.IsTrue(root.Root.Count() > 1);
            var child2 = root.Root.ElementAt(1);
            Assert.AreEqual("child2", child2.Name);
            Assert.IsNotEmpty(child2.Properties);
            Assert.AreEqual("v2", child2["cp"]);
            Assert.AreEqual("child2 text", child2.Text);
            Assert.IsEmpty(child2);
        }

        [Test]
        public void Parse_EmptyLines_Succeed()
        {
            var hml = @"
test


    child


";
            var root = this.parser.Parse(hml);

            Assert.IsNotNull(root);
            Assert.AreEqual("test", root.Root.Name);
            Assert.IsNullOrEmpty(root.Root.Text);
            Assert.IsEmpty(root.Root.Properties);

            Assert.IsNotEmpty(root.Root);
            var child = root.Root.First();
            Assert.AreEqual("child", child.Name);
            Assert.IsNullOrEmpty(child.Text);
            Assert.IsEmpty(child.Properties);
        }

        [Test]
        public void Parse_OneLevelNodes_LineAndColumns()
        {
            var hml = @"test(prop1=""propv1"", prop2=""propv2""): great sample!
  child(cp=""v""): child text";
            
            var root = this.parser.Parse(hml).Root;
            Assert.AreEqual(0, root.Position.Line);
            Assert.AreEqual(0, root.Position.Column);

            var child = root.First();
            Assert.AreEqual(1, child.Position.Line);
            Assert.AreEqual(2, child.Position.Column);
        }

        #endregion

        // TODO parse exception
    }
}
