using UnityEngine;

public abstract class Vision 
{
    protected Transform   tr;
    protected string      layerName = "Hero";
    public abstract (int, GameObject) CheckVision();
    
    protected (int, GameObject) View(Vector2 v2)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(tr.position, v2, 3f);
        if (hit.Length > 1)
        {
            string hitlayerName = LayerMask.LayerToName(hit[1].transform.gameObject.layer);

            if (hitlayerName != layerName)
            {
                Debug.DrawRay(tr.position, v2 * 2.5f, Color.green);
                return (1, null);
            } 
            else 
            {
                Debug.DrawRay(tr.position, v2 * 2.5f, Color.red);
                return (2, hit[1].transform.gameObject);
            } 
        }
        
        Debug.DrawRay(tr.position, v2 * 2.5f, Color.yellow);
        return (0, null);
    }
}
