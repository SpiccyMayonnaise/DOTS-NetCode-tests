using Unity.Entities;
using Unity.NetCode;

public struct TestRPCCommand : IRpcCommand {
    public int data;
}