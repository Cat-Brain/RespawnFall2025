using UnityEngine;
using FMODUnity;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    

    public EventReference click, shootFire;

    private FMOD.Studio.EventInstance instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
 
            return;
        }
    }

    public void Play(EventReference evt)
    {
        RuntimeManager.PlayOneShot(evt);
    }

    public void PlayClick()
    {
        RuntimeManager.PlayOneShot(click);
    } 

}
