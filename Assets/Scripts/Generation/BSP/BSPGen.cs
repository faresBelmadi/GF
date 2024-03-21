using System.Collections.Generic;
using UnityEngine;

public class BSPGen : MonoBehaviour
{
    public List<Container> Generate() 
    {
        return GetContainer();
    }

    List<Container> GetContainer()
    {
        List<Container> temp = new List<Container>()
        {
            new Container(-22, 20, 10, 10),
            new Container(-10,20,10,10),
            new Container(2, 20, 10, 10),
            new Container(14,20,10,10),
            new Container(26, 20, 10, 10),
            new Container(38,20,10,10),
            new Container(50, 20, 10, 10),
            new Container(62, 20, 10, 10)
            //new Container(20, 0, 10, 10),//start
            //new Container(40, 20, 10, 10),
            //new Container(20, 40, 10, 10),
            //new Container(0, 20, 10, 10),
            //new Container(60, 20, 10, 10)
        };
        return temp;
    }
}
