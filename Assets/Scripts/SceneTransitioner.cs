using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitioner : SingletonMonoBehaviour<SceneTransitioner>
{
    private Image fadeImage;
    [SerializeField]
    [Range(0.1f, 1)] private float fadeDuration = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        fadeImage = GetComponentInChildren<Image>();
        fadeImage.raycastTarget = false;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneUnloaded += OnSceneLoaded;
    }

    private void Start()
    {
        StartCoroutine(_FadeIn());
    }

    public void TransitionStart(int sceneNum)
    {
        StartCoroutine(_FadeOut(sceneNum));
    }

    private void OnSceneLoaded(Scene prevScene)
    {
        StartCoroutine(_FadeIn());
    }

    private IEnumerator _FadeIn()
    {
        var waitForSeconds = new WaitForSeconds(0.01f);
        var loop = fadeDuration / 0.01f;
        for (int i = 0; i < (int)loop; i++)
        {
            yield return waitForSeconds;
            fadeImage.color -= new Color(0, 0, 0, 1 / loop);
        }
        fadeImage.color = new Color(0, 0, 0, 0);

    }

    private IEnumerator _FadeOut(int sceneNum)
    {
        var waitForSeconds = new WaitForSecondsRealtime(0.01f);
        var loop = fadeDuration / 0.01f;
        for(int i = 0; i < (int)loop; i++)
        {
            yield return waitForSeconds;
            fadeImage.color += new Color(0, 0, 0, 1 / loop);
        }
        fadeImage.color += new Color(0, 0, 0, 1);
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
    }

}
