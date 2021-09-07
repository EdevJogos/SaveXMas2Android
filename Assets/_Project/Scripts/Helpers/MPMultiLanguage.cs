using System;
using UnityEngine;

public class MPMultiLanguage : MonoBehaviour
{
    [Serializable]
    public class LanguageData
    {
        public Languages language;
        public string pcText;
        public string mobileText;
    }

    public bool autoStart = true;
    public LanguageData[] availableLanguages = new LanguageData[2];

    private void Awake()
    {
        LanguageManager.onLanguageUpdated += UpdateText;
    }

    public void Start()
    {
        if (autoStart)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if (Application.isMobilePlatform)
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = availableLanguages[(int)LanguageManager.Language].mobileText.Replace("\\n", "\n");
        }
        else
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = availableLanguages[(int)LanguageManager.Language].pcText.Replace("\\n", "\n");
        }
    }
}
