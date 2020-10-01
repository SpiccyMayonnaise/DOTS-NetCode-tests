using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public struct CubeInput : ICommandData {
    public uint Tick { get; set; }
    public int horizontal;
    public int vertical;
}
