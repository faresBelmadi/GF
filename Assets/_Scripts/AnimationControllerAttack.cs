using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerAttack : MonoBehaviour
{

    public GameObject Main;
    public Animator MainAnimator;
    public GameObject OmbreIdle;
    public List<GameObject> OmbreTransition;

    public KeyCode keyStart;

    public List<Transform> Waypoints;

    public Action tempAction;

    [SerializeField]private float TimeIdleDisappear;
    [SerializeField]private float TimeTransition;
    [SerializeField]private float TimeAttaque;


    // Update is called once per frame
    void Update()
    {
        Debug.Log("waiting for input");
        if(Input.GetKeyDown(keyStart))
        {
        }
    }

    public void StartAttack(Action ToDo)
    {
        tempAction = ToDo;
        Debug.Log("spell detected");
        StartCoroutine("StartAnimation");
    }

    IEnumerator StartAnimation()
    {
        Debug.Log("Start animation");
        Debug.Log("Change pose to Transition");

        yield return LerpPosition(Main.transform,Waypoints[0].transform,TimeTransition);
        OmbreIdle.GetComponent<SpriteRenderer>().enabled = true;
        MainAnimator.SetBool("Transition",true);
        yield return LerpPosition(Main.transform,Waypoints[1].transform,TimeTransition);
        OmbreTransition[0].GetComponent<SpriteRenderer>().enabled = true;
        yield return LerpPosition(Main.transform,Waypoints[2].transform,TimeTransition);
        OmbreTransition[1].GetComponent<SpriteRenderer>().enabled = true;
        yield return LerpPosition(Main.transform,Waypoints[3].transform,TimeTransition);
        //OmbreTransition[2].GetComponent<SpriteRenderer>().enabled = true;
        MainAnimator.SetBool("Transition",false);
        MainAnimator.SetBool("Attaque",true);
        yield return LerpPosition(Main.transform,Waypoints[4].transform,TimeAttaque);
        MainAnimator.SetBool("Attaque",false);
        yield return LerpPosition(Main.transform,Waypoints[3].transform,TimeTransition);
        //OmbreTransition[2].GetComponent<SpriteRenderer>().enabled = false;
        yield return LerpPosition(Main.transform,Waypoints[2].transform,TimeTransition);
        OmbreTransition[1].GetComponent<SpriteRenderer>().enabled = false;
        yield return LerpPosition(Main.transform,Waypoints[1].transform,TimeTransition);
        OmbreTransition[0].GetComponent<SpriteRenderer>().enabled = false;
        yield return LerpPosition(Main.transform,Waypoints[0].transform,TimeTransition);
        OmbreIdle.GetComponent<SpriteRenderer>().enabled = false;
        if(tempAction != null)
            tempAction();
        
        // Debug.Log("Starting animation");
        // yield return new WaitForSeconds(TimeIdleDisappear);
        // Idle.SetActive(false);
        // Debug.Log("transition animation start");
        // AI1.SetActive(true);
        // yield return new WaitForSeconds(TimeTransition);
        // AI2.SetActive(true);
        // yield return new WaitForSeconds(TimeTransition);
        // AI3.SetActive(true);
        // yield return new WaitForSeconds(TimeTransition);
        // Transition.SetActive(true);
        // yield return new WaitForSeconds(TimeTransition);
        // Transition.SetActive(false);
        // AI1.SetActive(false);
        // AI2.SetActive(false);
        // AI3.SetActive(false);
        // Debug.Log("transition animation end");
        // Debug.Log("attaque animation start");
        // Attaque.SetActive(true);
        // yield return new WaitForSeconds(TimeAttaque);
        // Attaque.SetActive(false);
        // Debug.Log("attaque animation end");
        // Debug.Log("transition animation start");
        // TransitionRetour.SetActive(true);
        // yield return new WaitForSeconds(TimeTransition);
        // TransitionRetour.SetActive(false);
        // Debug.Log("transition animation end");
        // Idle.SetActive(true);
        // Debug.Log("ending animation");
    }


    IEnumerator LerpPosition(Transform ToMove, Transform Target, float duration)
    {
        float time = 0;
        Vector3 startPosition = ToMove.position;

        while (time < duration)
        {
            ToMove.position = Vector3.Lerp(startPosition, Target.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        ToMove.position = Target.position;
    }
    
}
