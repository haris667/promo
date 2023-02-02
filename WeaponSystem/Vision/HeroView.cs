using UnityEngine;
using System.Collections.Generic;

public class HeroView : Vision
{
    public HeroView(Transform tr, string layerName)
    {
        this.tr         = tr;
        this.layerName  = layerName;
    }
    public override (int, GameObject) CheckVision()
    {
        List<GameObject> enemyList = new List<GameObject>();
        Vector2 v2 = tr.right;
        for ( float f = -0.75f; f < 0.8f; f += 0.75f)
        {
            v2.y = tr.right.y + f;
            var i = View(v2);

            if ( i.Item1 == 2 ) enemyList.Add(i.Item2); 
        }
        return enemyList.Count != 0 ? (2, enemyList[Random.Range(0, enemyList.Count)]) : (0, null);
    }
}
