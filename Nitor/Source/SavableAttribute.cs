namespace Nitor;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SavableAttribute : Attribute
{
	// Marks data as serializable/deserializable.
}
