using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Hml.Parser
{
    /// <summary>
    /// A node in a tree.
    /// </summary>
    public class HmlNode : IEnumerable<HmlNode>
    {
        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hml.Parser.HmlNode"/> class.
        /// </summary>
        /// <param name="indent">The indent level.</param>
        /// <param name="name">The name.</param>
        /// <param name="text">The text content.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="position">The position in source file.</param>
        public HmlNode(int indent, string name, string text, IDictionary<string, string> properties, Position position)
        {
            this.Indent = indent;
            this.Text = text;
            this.Name = name;
            this.Position = position;
            this.Properties = properties ?? new Dictionary<string, string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>The parent.</value>
        public HmlNode Parent { get; private set; }

        /// <summary>
        /// Gets the indent level.
        /// </summary>
        /// <value>The indent level.</value>
        public int Indent { get; }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the text content.
        /// </summary>
        /// <value>The text content.</value>
        public string Text { get; }

        /// <summary>
        /// Gets the position in the source file.
        /// </summary>
        /// <value>The position in the source file.</value>
        public Position Position { get; }

        /// <summary>
        /// Gets the node properties.
        /// </summary>
        /// <value>The properties.</value>
        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the property with the specified name.
        /// </summary>
        /// <param name="name">Name.</param>
        public string this[string name]
        {
            get { return this.Properties[name]; }
            set { this.Properties[name] = value; }
        }

        /// <summary>
        /// Tries the get property.
        /// </summary>
        /// <returns><c>true</c>, if get property was tryed, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public bool TryGetProperty(string name, out string value)
        {
            return this.Properties.TryGetValue(name, out value);
        }

        #endregion

        #region Fields

        private List<HmlNode> children = new List<HmlNode>();

        #endregion

        #region Methods

        /// <summary>
        /// Add the node.
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="node">Node.</param>
        public void Add(HmlNode node)
        {
            if (node.Parent != null)
                throw new InvalidOperationException("Added child already has a parent");

            node.Parent = this;
            this.children.Add(node);
        }

        /// <summary>
        /// Gets the first occurence of a child with the specified name.
        /// </summary>
        /// <returns>The found child, else null.</returns>
        /// <param name="name">The node name.</param>
        public HmlNode Child(string name) => this.FirstOrDefault(x => x.Name == name);

        /// <summary>
        /// Gets all the direct children with the specified name.
        /// </summary>
        /// <returns>The children.</returns>
        /// <param name="name">Name.</param>
        public IEnumerable<HmlNode> Children(string name) => this.Where(x => x.Name == name);

        /// <summary>
        /// Gets the property value, or null if not found.
        /// </summary>
        /// <returns>The found property, else null.</returns>
        /// <param name="name">The property name.</param>
        public string Property(string name) 
        {
            if(this.Properties.TryGetValue(name, out string value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<HmlNode> GetEnumerator() => this.children.GetEnumerator();

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.children.GetEnumerator();

        #endregion
    }
}
