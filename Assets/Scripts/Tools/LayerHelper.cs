using UnityEngine;

public static class LayerHelper
{
    public static void MoveToLayer(Transform root, int layer) 
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
        {
            MoveToLayer(child, layer);
        }
    }
}