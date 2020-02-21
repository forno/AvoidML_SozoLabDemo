using Unity.Entities;
using System;
using UnityEngine;

[Serializable]
public struct LineRendererReference : ISharedComponentData, IEquatable<LineRendererReference>
{
    public LineRenderer LineRenderer;

    public bool Equals(LineRendererReference obj)
    {
        return LineRenderer == obj.LineRenderer;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return LineRenderer.GetHashCode();
    }
}
