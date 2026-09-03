using System.Reflection;

namespace Nitor;

public class Scene : Asset
{
	// TODO: Uses serialization to serialize/deserialize entity-comp data.
	// In debug mode, it avoids hashing paths, but in release mode it hashes the paths.

	// When deserializing an entity, get all properties and fields with the SavableAttribute attached.
	// Hash each one and create a dictionary from hash to actual member.
	// For that entity's raw data, map the keys in the data to the corrosponding hash in the dictionary, then write the data to the actual member.

	// Special converter classes exist and are registered. They manage converting non-primative data.

	// Maybe for release builds just full on store in binary? Would need to store info about data size for collections, but wouldn't need info about data type.
}
