using BoilerPlateGeneration.ObjectImplementation;

namespace BoilerPlateGeneration.InterfaceGeneration;

public record GenerationPropertyInfoWithExternalInfo(
    bool HasExternalPropertyAttribute,
    string Type,
    string Name,
    bool HasGetter,
    bool HasSetter)
    : GenerationPropertyInfo(Type, Name, HasGetter, HasSetter);