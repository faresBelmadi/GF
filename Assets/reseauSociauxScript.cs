using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reseauSociauxScript : MonoBehaviour
{
    public Animator guerrier;
    public Animator ennemi;
    bool started = false;
    public void Update()
    {
        Debug.Log("t");
        if(Input.GetKey(KeyCode.Space) && !started)
        {
            started = true;
            StartCoroutine("AnimationAction");
            Debug.Log("gggg");
        }
    }

    public IEnumerator AnimationAction()
    {
        Debug.Log("dddd");
        guerrier.SetBool("2", true);
        yield return new WaitForEndOfFrame();
        guerrier.SetBool("2", false);
        yield return new WaitForSeconds(0.25f);
        ennemi.SetBool("mm", true);
        yield return new WaitForEndOfFrame();
        ennemi.SetBool("mm", false);
        //yield return new WaitForSeconds(4f);
        //ennemi.SetBool("LaunchAttaque2", true);
        //yield return new WaitForEndOfFrame();
        //ennemi.SetBool("LaunchAttaque2", false);
        //yield return new WaitForSeconds(0.5f);
        //guerrier.SetBool("IsAttacked", true);
        //yield return new WaitForEndOfFrame();
        //guerrier.SetBool("IsAttacked", false);


        started = false;
    }

}
