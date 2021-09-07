using System.Collections;
using UnityEngine;

public class Display : MonoBehaviour
{
    public static System.Action<Displays, int> onActionRequested;
    public static System.Action<Displays, int, object> onDataActionRequested;

    public Displays ID;
    public GameState activeState;
    public UnityEngine.UI.Selectable startSelected;

    public virtual void Initiate()
    {
        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    }

    private void GameCEO_onGameStateChanged()
    {
        enabled = GameCEO.State == activeState;
    }

    private void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    public virtual void Initialize()
    {

    }

    public virtual void Show(bool p_show, System.Action p_callback, float p_ratio)
    {
        if (p_show)
        {
            if (startSelected != null)
            {
                startSelected.Select();
            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }

            GetComponent<Animator>().SetTrigger("In");
            AudioManager.PlaySFX(SFXOccurrence.DISPLAY_IN, 0);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Out");
        }

        if (p_callback != null) StartCoroutine(RoutineShow(p_show, p_callback, p_ratio));
    }

    private IEnumerator RoutineShow(bool p_show, System.Action p_callback, float p_ratio)
    {
        string __clipName = ID + (p_show ? "In" : "Out");

        float __delay = GetClipLength(__clipName) * GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).speed * p_ratio;

        yield return new WaitForSeconds(__delay);

        p_callback?.Invoke();
    }

    private float GetClipLength(string name)
    {
        AnimationClip[] clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (var item in clips)
        {
            if (item.name == name)
            {
                return item.length;
            }
        }

        return 0;
    }

    public virtual void UpdateDisplay(int p_operation, bool p_value)
    {

    }

    public virtual void UpdateDisplay(int p_operation, int p_value, int p_data)
    {

    }

    public virtual void UpdateDisplay(int p_operation, int[] p_data)
    {

    }

    public virtual void UpdateDisplay(int p_operation, object p_data)
    {

    }

    public virtual object GetData(int p_data)
    {
        return null;
    }

    public virtual void RequestAction(int p_action)
    {
        onActionRequested?.Invoke(ID, p_action);
    }

    public virtual void RequestAction(int p_action, object p_data)
    {
        onDataActionRequested?.Invoke(ID, p_action, p_data);
    }
}
