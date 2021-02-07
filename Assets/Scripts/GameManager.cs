using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentLevel { get; private set; }

    [SerializeField] bool _forceLoadLevel;
    [SerializeField] int _levelToLoad = 0;

    TurnController _turnController;

    public void LoadLevelScene()
    {
        var op = SceneManager.LoadSceneAsync("LevelScene");
        op.completed += OnLevelSceneLoaded;
        
    }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Debug.Log("GM Start");
        if (SceneManager.GetActiveScene().buildIndex != 0)
            LoadLevelScene();
    }

    void OnLevelSceneLoaded(AsyncOperation obj)
    {
        _turnController = FindObjectOfType<TurnController>();
        _turnController.LevelCompleted += OnLevelCompleted;

        FindObjectOfType<UIGameCanvasManager>().ReloadRequested += OnReloadRequested;

        if (_forceLoadLevel)
            CurrentLevel = _levelToLoad;
        else
            CurrentLevel = 1;

        LoadNextLevel();
    }

    void OnReloadRequested()
    {
        ReloadLevel();
    }

    void ReloadLevel()
    {
        var op = SceneManager.UnloadSceneAsync(GetLevelName(CurrentLevel));
        op.completed += OnUnloadComplete;
    }

    void OnLevelCompleted()
    {
        // Debug.Log("on level completed");

        var op = SceneManager.UnloadSceneAsync(GetLevelName(CurrentLevel));
        op.completed += OnUnloadComplete;
        CurrentLevel++;
    }

    void OnUnloadComplete(AsyncOperation obj)
    {
        // Debug.Log("unload level completed");

        if (CurrentLevel == 0)
            LoadMainMenu();
        else
            LoadNextLevel();
    }

    void LoadNextLevel()
    {
        // Debug.Log("load next level");

        var op = SceneManager.LoadSceneAsync(GetLevelName(CurrentLevel), LoadSceneMode.Additive);
        op.completed += OnLoadLevelCompleted;
    }

    void OnLoadLevelCompleted(AsyncOperation obj)
    {
        // Debug.Log("load level completed");
        InitLevel();
    }

    void LoadMainMenu()
    {
        // Debug.Log("load main menu");
        SceneManager.LoadSceneAsync(0);
    }
    
    void InitLevel()
    {
        _turnController.Init();
        FindObjectOfType<UIGameCanvasManager>().Init(CurrentLevel);
    }

    string GetLevelName(int level) => $"Level-{level}";
}
