using UnityEngine;


public class AnimationEvent : MonoBehaviour
{
    public GameObject Canvas;

    public GameObject Button1, Button2, Button3;



    public void StartPaning()
    {
        Canvas.GetComponent<Animator>().SetTrigger("Click");

    }

    public void HideMenu()
    {
        Button1.GetComponent<Animator>().SetTrigger("Hide");
        Button2.GetComponent<Animator>().SetTrigger("Hide");
        Button3.GetComponent<Animator>().SetTrigger("Hide");
    }
}
