namespace Eagle.CodeAnalysis
{
    public class BoundLabel
    {
        public string Name { get; }

        public BoundLabel(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}