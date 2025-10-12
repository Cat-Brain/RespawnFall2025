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

    public void StartLoad(string sceneName, UnityEvent onBlackScreen = null)
    {
        if (loadingScene)
            return;

        loadingScene = true;
        StartCoroutine(LoadSceneInternal(sceneName, onBlackScreen));
    }

    public IEnumerator LoadSceneInternal(string sceneName, UnityEvent onBlackScreen = null)
    {
        fadeBox.enabled = true;
        fadeBox.DOFade(1, fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(fadeTime);

        onBlackScreen?.Invoke();

        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);

        

        fadeBox.DOFade(0, fadeTime).OnComplete(() => { fadeBox.enabled = false; loadingScene = false; });
    }
}
