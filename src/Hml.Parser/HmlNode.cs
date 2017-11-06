using System.Collections;
using System.Collections.Generic;
using System;

namespace Hml.Parser
{
    public class HmlNode : IEnumerable<HmlNode>
    {
        public HmlNode(int indent, string name, string text, IDictionary<string,string> properties, Position position)
        {
            this.Indent = indent;
            this.Text = text;
            this.Name = name;
            this.Position = position;
            this.Properties = properties ?? new Dictionary<string, string>();
        }

        private List<HmlNode> children = new List<HmlNode>();

        public HmlNode Parent { get; private set; }

        public int Indent { get; }

        public string Name { get; }

        public string Text { get; }

        public Position Position { get; }

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public string this[string name]
        {
            get { return this.Properties[name]; }
            set { this.Properties[name] = value; }
        }

        public bool TryGetProperty(string name, out string value)
        {
            return this.Properties.TryGetValue(name, out value);
        }

        public void Add(HmlNode node)
        {
            if (node.Parent != null)
                throw new InvalidOperationException("Added child already has a parent");

            node.Parent = this;
            this.children.Add(node);
        }

        public IEnumerator<HmlNode> GetEnumerator() => this.children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.children.GetEnumerator();
    }
}
