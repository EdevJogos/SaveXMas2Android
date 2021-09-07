using UnityEngine;
using System;

public class GUIManager : MonoBehaviour
{
    public event Action onStartGameRequested;
    public event Action onShopRequested;
    public event Action onHireRequested;
    public event Action onExitGameRequested;
    public event Action onResumePlayRequested;
    public event Action onCreditsRequested;
    public event Action onHomeRequested;
    public event Action onTutorialCompleted;

    public Display[] displays;

    private Display _activeDisplay;

    public void Initiate()
    {
        Display.onActionRequested += OnActionRequested;

        for (int __i = 0; __i < displays.Length; __i++)
        {
            displays[__i].Initiate();
        }
    }

    private void OnDestroy()
    {
        Display.onActionRequested -= OnActionRequested;
    }

    public void Initialize()
    {
        for (int __i = 0; __i < displays.Length; __i++)
        {
            displays[__i].Initialize();
        }
    }

    public void ShowDisplay(Displays p_display, Action p_onHideCompleted = null, float p_hideRatio = 1f, Action p_onShowCompleted = null, float p_showRatio = 1f)
    {
        if (_activeDisplay == null || (_activeDisplay != null && _activeDisplay.ID != p_display))
        {
            if (_activeDisplay != null)
            {
                _activeDisplay.Show(false, p_onHideCompleted, p_hideRatio);
            }

            _activeDisplay = displays[(byte)p_display];
            _activeDisplay.Show(true, p_onShowCompleted, p_showRatio);
        }
    }

    #region Update Display Calls

    public void UpdateDisplay(Displays p_id, int p_operation, bool p_value)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_value);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, int p_value, int p_data = -99999)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_value, p_data);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, int[] p_data)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_data);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, object p_data)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_data);
    }

    #endregion

    public object GetData(Displays p_id, int p_data)
    {
        return displays[(int)p_id].GetData(p_data);
    }

    private void OnActionRequested(Displays p_id, int p_action)
    {
        switch (p_id)
        {
            case Displays.INTRO:
                switch (p_action)
                {
                    case 0:
                        onStartGameRequested?.Invoke();
                        break;
                    case 1:
                        onCreditsRequested?.Invoke();
                        break;
                }
                break;
            case Displays.HUD:
                switch (p_action)
                {
                    case 0:
                        onShopRequested?.Invoke();
                        break;
                }
                break;
            case Displays.CREDITS:
                switch (p_action)
                {
                    case 0:
                        onHomeRequested?.Invoke();
                        break;
                }
                break;
            case Displays.SHOP:
                switch (p_action)
                {
                    case 0:
                        onHireRequested?.Invoke();
                        break;
                    case 1:
                        onExitGameRequested?.Invoke();
                        break;
                    case 2:
                        onResumePlayRequested?.Invoke();
                        break;
                }
                break;
            case Displays.TUTORIAL:
                switch (p_action)
                {
                    case 0:
                        onTutorialCompleted?.Invoke();
                        break;
                }
                break;
        }
    }
}