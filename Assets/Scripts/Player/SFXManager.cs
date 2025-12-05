using UnityEngine;
using FMODUnity;

public class SFXManager : MonoBehaviour
{
    public static SFXManager sfxManager;
    public EventReference sfxEvent;

    private FMOD.Studio.EventInstance instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void PlayClick()
    {
        RuntimeManager.PlayOneShot(sfxEvent);
    }


}
