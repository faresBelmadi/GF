using UnityEngine;

[System.Serializable]
public class Container
{

    public Vector2 Center;

    public float x,y,h,w;

    public Container(float _x,float _y, float _h, float _w)
    {
        x = _x;
        y = _y;
        w = _w;
        h = _h;
        Center = new Vector2(x+w/2,y+h/2);
    }
}
