using UnityEngine;

public class CentralView : Vision
{
    public CentralView(Transform tr)
    {
        this.tr = tr;
    }
    public override (int, GameObject) CheckVision()
    {
        Vector2 v2 = tr.right;
        for ( float f = -0.25f; f < 0.3; f += 0.25f)
        {
            v2.y = tr.right.y + f;
            var i = View(v2);

            if ( i.Item1 != 0) return i; 
        }
        return (0, null);
    }
}
