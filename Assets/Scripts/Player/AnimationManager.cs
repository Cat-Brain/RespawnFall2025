using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    public PlayMode mode;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    /*public void PlayAnimation(AnimationClip animClip)
    {
        animClip.Play(mode = PlayMode.StopSameLayer);
    }*/
}
