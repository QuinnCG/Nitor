namespace Nitor;

[AttributeUsage(AttributeTargets.Class)]
public class RequireSiblingAttribute<T> : Attribute where T : Component 
{ }
