using System.Collections.Generic;
using UnityEngine;

public class BSPGen : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> _pos;
    public List<Container> Generate() 
    {
        return GetContainer();
    }

    List<Container> GetContainertemp()
    {
        List<Container> temp = new List<Container>()
        {
            new Container(-25, 20, 10, 10),
            new Container(-14, 30,10,10),
            new Container(-4, 30, 10, 10),
            new Container(3, 20,10,10),
            new Container(10, 10, 10, 10),
            new Container(20, 10,10,10),
            new Container(27, 20, 10, 10),
            new Container(34, 30, 10, 10),
            new Container(44, 30, 10, 10),
            //new Container(51, 20, 10, 10),
            new Container(61, 20, 10, 10)
            //new Container(20, 0, 10, 10),//start
            //new Container(40, 20, 10, 10),
            //new Container(20, 40, 10, 10),
            //new Container(0, 20, 10, 10),
            //new Container(60, 20, 10, 10)
        };
        return temp;
    }
    public List<Container> GetContainer()
    {
        List<Container> temp = new List<Container>();
        foreach (var item in _pos)
        {
            Container cont = new Container(item.position.x, item.position.y, 1,1);
            temp.Add(cont);
        }
        return temp;


    }
}
