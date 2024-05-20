using System;
using System.Collections;
using System.Collections.Generic;
using _SpesficCode.Timer;
using DG.Tweening;
using Sirenix.OdinInspector;
//using LionStudios.Suite.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10)]
public class LevelManager : Singleton<LevelManager>
{
    [Header("F1 For Next Level")][SerializeField ,ReadOnly] protected int childCount;
    [SerializeField ,ReadOnly]protected internal int LevelCount;
    protected int startingIndex = 0;
    
    public virtual void OnEnable()
    {
        childCount = transform.childCount;
        
        startingIndex = LevelCount = PlayerPrefs.GetInt("LevelCount",0);
        print(startingIndex + "is Starting Level index");

        #region CloseAllScenes

        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        #endregion
        
        OpenLevel();

    }

    private IEnumerator OnEnableCoroutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        OpenLevel();

    }

    protected virtual void OpenLevel()
    {
        if (childCount == 0)
        {
            Debug.LogError("There is no level to open");
            return;
        }
        int indexToOpen = LevelCount % childCount;
        Debug.Log("Açılacak index is ".Red()+indexToOpen);

        transform.GetChild(indexToOpen).gameObject.SetActive(true);
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            LoadNextLevel();
        }
    }
#endif
    public void LoadNextLevel(float delay = 0)
    {
        this.MakeAction(LoadNextLevel, delay);
    }

    public virtual void LoadNextLevel()
    {
        transform.GetChild(LevelCount % childCount).gameObject.SetActive(false);

        PlayerPrefs.SetInt("LevelCount", PlayerPrefs.GetInt("LevelCount") + 1);
        LevelCount = PlayerPrefs.GetInt("LevelCount");

        
        transform.GetChild(LevelCount % childCount).gameObject.SetActive(true);
        
        print("Starting index" + startingIndex + "LevelCount: " + LevelCount);
        PuzzleGameEvents.levelStarted?.Invoke();

        if (startingIndex + childCount == LevelCount)
        {
            print("Load Scene Again");
            DOTween.KillAll(true);
            SceneManager.LoadScene(0);
        }
    }

    [Button]
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        PuzzleGameEvents.levelStarted?.Invoke();
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    public virtual void OpenHomeLevel()
    {
        print("Open Home Level TODO");
    }
}