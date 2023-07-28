using UnityEngine;

public static class Extension
{
    public static bool IsContain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}
