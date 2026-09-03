namespace Nitor;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ExposeAttribute : Attribute
{
	// Marks data as being exposed and editable in editor mode.
}
