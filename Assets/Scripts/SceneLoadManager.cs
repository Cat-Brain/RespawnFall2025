using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SceneLoadManager : MonoBehaviour
{
    public Image fadeBox;
    public string sceneToLoad;

    public float fadeTime;

    public bool loadingScene = false;

    void Update()
    {
        if (!loadingScene)
        {
            fadeBox.color = Color.clear;
            fadeBox.enabled = false;
        }
    }

    public void SetSceneToLoad(string sceneToLoad)
    {
        this.sceneToLoad = sceneToLoad;
    }

    public void StartLoad(UnityEvent onBlackScreen = null)
    {
        if (loadingScene)
            return;

        loadingScene = true;
        StartCoroutine(LoadSceneInternal(onBlackScreen));
    }

    public IEnumerator LoadSceneInternal(UnityEvent onBlackScreen = null)
    {
        fadeBox.enabled = true;
        fadeBox.DOFade(1, fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(fadeTime);

        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);

        onBlackScreen?.Invoke();
        
        fadeBox.DOFade(0, fadeTime).OnComplete(() => { fadeBox.enabled = false; loadingScene = false; });
    }
}
