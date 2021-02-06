using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] bool _shouldPlayIntro = true;
    [SerializeField] GameObject _instructionsParent;
    [SerializeField] GameObject _introParent;
    [SerializeField] GameObject _fadeOutImage;
    [SerializeField] GameObject _fadeInImage;

    bool _hasRequestedStart;
    
    GameObject[] _introLines;
    bool _isIntroActive;
    int _currentIntroIndex;
    
    GameObject[] _instructions;
    bool _areInstructionsActive;
    int _currentInstructionsIndex;

    void Awake()
    {
        _instructions = _instructionsParent.GetComponentsInChildren<RectTransform>().Skip(1).Select(r => r.gameObject).ToArray();
        _introLines = _introParent.GetComponentsInChildren<RectTransform>().Skip(1).Select(r => r.gameObject).ToArray();
        _fadeOutImage.SetActive(true);
    }

    void Update()
    {
        if (ShouldStart())
        {
            _hasRequestedStart = true;
            StartCoroutine(LoadLevel());
        }

        HandleIntro();
        HandleInstructions();
    }
    
    bool ShouldStart() => !_areInstructionsActive && !_isIntroActive && !_hasRequestedStart && Input.GetButtonDown("Advance");

    IEnumerator LoadLevel()
    {
        _fadeInImage.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !_fadeInImage.GetComponent<Animation>().isPlaying);
        yield return new WaitForSeconds(0.25f);
        
        GameManager.Instance.LoadLevelScene();
    }

    void HandleIntro()
    {
        if (ShouldTurnIntroOn())
        {
            _isIntroActive = true;
            _introParent.SetActive(_isIntroActive);
            SetActiveIntroLine(0);
        }
        else if (ShouldAdvanceIntro())
        {
            _currentIntroIndex++;

            if (_currentIntroIndex >= _introLines.Length)
            {
                _isIntroActive = false;
                _introParent.SetActive(_isIntroActive);
            }
            else
                SetActiveIntroLine(_currentIntroIndex);
        }
    }

    bool ShouldTurnIntroOn() => Input.GetButtonDown("Intro") && !_isIntroActive;

    bool ShouldAdvanceIntro() => (Input.GetButtonDown("Intro") || Input.GetButtonDown("Advance")) && _isIntroActive;

    void SetActiveIntroLine(int index)
    {
        for (int i = 0; i < _introLines.Length; i++)
            _introLines[i].SetActive(i == index);
            
        _currentIntroIndex = index;
    }

    void HandleInstructions()
    {
        if (ShouldTurnInstructionsOn())
        {
            _areInstructionsActive = true;
            _instructionsParent.SetActive(_areInstructionsActive);
            SetActiveInstruction(0);
        }
        else if (ShouldAdvanceInstructions())
        {
            _currentInstructionsIndex++;

            if (_currentInstructionsIndex >= _instructions.Length)
            {
                _areInstructionsActive = false;
                _instructionsParent.SetActive(_areInstructionsActive);
            }
            else
                SetActiveInstruction(_currentInstructionsIndex);
        }
    }

    bool ShouldTurnInstructionsOn() => Input.GetButtonDown("Help") && !_areInstructionsActive;

    bool ShouldAdvanceInstructions() => (Input.GetButtonDown("Help") || Input.GetButtonDown("Advance")) && _areInstructionsActive;

    void SetActiveInstruction(int index)
    {
        for (int i = 0; i < _instructions.Length; i++)
            _instructions[i].SetActive(i == index);
            
        _currentInstructionsIndex = index;
    }
}
