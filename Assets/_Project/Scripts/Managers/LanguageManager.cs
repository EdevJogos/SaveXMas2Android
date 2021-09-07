using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static event System.Action onLanguageUpdated;

    public static Languages Language = Languages.EN_US;

    public void UpdateLanguage(int p_id)
    {
        Language = (Languages)p_id;

        onLanguageUpdated?.Invoke();
    }
}
