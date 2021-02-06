using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaginatedTextController : MonoBehaviour
{
    public static bool IsPaginationActive;

    [SerializeField] string _buttonName;
    [SerializeField] GameObject _background;
    [SerializeField] GameObject[] _pages;
    public bool IsActive { get; private set; }

    int _currentIndex;

    void Update()
    {
        HandlePagination();
    }

    void HandlePagination()
    {
        if (ShouldTurnOn())
        {
            IsActive = true;
            IsPaginationActive = true;
            _background.SetActive(true);
            SetActivePage(0);
        }
        else if (ShouldAdvancePage())
        {
            _currentIndex++;

            if (_currentIndex >= _pages.Length)
            {
                IsActive = false;
                IsPaginationActive = false;
                _background.SetActive(false);
            }
            else
                SetActivePage(_currentIndex);
        }
    }

    bool ShouldTurnOn() => Input.GetButtonDown(_buttonName) && !IsActive && !IsPaginationActive;

    bool ShouldAdvancePage() => (Input.GetButtonDown(_buttonName) || Input.GetButtonDown("Advance")) && IsActive;

    void SetActivePage(int index)
    {
        for (int i = 0; i < _pages.Length; i++)
            _pages[i].SetActive(i == index);
            
        _currentIndex = index;
    }
}
