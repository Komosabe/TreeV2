namespace TreeV2.Entities
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; } // name of node
        public string? ParentNode { get; set; } // branch
        public List<Node> Children { get; set; } = new List<Node>(); // leaves
    }
}
