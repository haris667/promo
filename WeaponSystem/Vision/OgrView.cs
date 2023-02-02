using UnityEngine;

public class OgrView : Vision
{
    public OgrView(Transform tr)
    {
        this.tr = tr;
    }
    public override (int, GameObject) CheckVision()
    {
        Vector2 v2 = tr.right; 

        var i = View(v2);

        return i; 

    }
}
