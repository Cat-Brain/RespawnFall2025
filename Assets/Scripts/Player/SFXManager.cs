using UnityEngine;
using FMODUnity;

public class SFXManager : MonoBehaviour
{
    public EventReference sfxEvent;

    private FMOD.Studio.EventInstance instance;

    public void PlayClick()
    {
        RuntimeManager.PlayOneShot(sfxEvent);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
