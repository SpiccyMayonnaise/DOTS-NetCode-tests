using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct MovableCube : IComponentData {
    [GhostField]
    public int ExampleValue;
}
