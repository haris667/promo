using UnityEngine;

public class PlayerView : Vision
{
    public PlayerView(Transform tr, string layerName)
    {
        this.tr = tr;
        this.layerName = layerName;
    } 

    public override (int, GameObject) CheckVision()
    {
        var i = View(tr.up);
        return i; 
    }
}
