using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentLevel { get; private set; }

    [SerializeField] bool _infiniteMana;
    [SerializeField] bool _forceLoadLevel;
    [SerializeField] int _levelToLoad = 0;

    TurnController _turnController;
    UIGameCanvasManager _uiGameCanvasManager;

    public void LoadLevelScene()
    {
        var op = SceneManager.LoadSceneAsync("LevelScene");
        op.completed += OnLevelSceneLoaded;
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
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
        if (ShouldLoadLevel())
            LoadLevelScene();
    }

    private bool ShouldLoadLevel() => SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1;

    void OnLevelSceneLoaded(AsyncOperation obj)
    {
        _turnController = FindObjectOfType<TurnController>();
        _uiGameCanvasManager = FindObjectOfType<UIGameCanvasManager>();

        _uiGameCanvasManager.ReloadRequested += OnReloadRequested;
        _uiGameCanvasManager.LevelCompleted += OnLevelCompleted;

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
        var op = SceneManager.UnloadSceneAsync(GetLevelName(CurrentLevel));
        op.completed += OnUnloadComplete;
        CurrentLevel++;
    }

    void OnUnloadComplete(AsyncOperation obj)
    {
        if (CurrentLevel == 0)
            LoadMainMenu();
        else
            LoadNextLevel();
    }

    void LoadNextLevel()
    {
        var nextLevelName = GetLevelName(CurrentLevel);

        try
        {
            var op = SceneManager.LoadSceneAsync(nextLevelName, LoadSceneMode.Additive);
            op.completed += OnLoadLevelCompleted;
        }
        catch (NullReferenceException ex)
        {
            LoadEndScene();
        }
    }

    void OnLoadLevelCompleted(AsyncOperation obj)
    {
        InitLevel();
        FindObjectOfType<PlayerController>().IsInfiniteManaModeActive = _infiniteMana;
    }

    void InitLevel()
    {
        _turnController.Init();
        _uiGameCanvasManager.Init(CurrentLevel);
    }

    void LoadEndScene()
    {
        SceneManager.LoadSceneAsync("EndScene");
    }

    string GetLevelName(int level) => $"Level-{level}";
}
