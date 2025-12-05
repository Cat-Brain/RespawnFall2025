using UnityEngine;
using FMODUnity;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager Instance;

    public EventReference ambienceEvent;

    private FMOD.Studio.EventInstance instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        instance = RuntimeManager.CreateInstance(ambienceEvent);
        instance.start();
    }

    public void SetTheme(int index)
    {
        instance.setParameterByName("ThemeIndex", index);
    }
}
