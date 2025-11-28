using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return ((1 << layer) & mask.value) != 0;
    }
}
