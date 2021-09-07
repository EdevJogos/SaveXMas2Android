using System;
using UnityEngine;

public class MultiLanguage : MonoBehaviour
{
    [Serializable]
    public class LanguageData
    {
        public Languages language;
        public string text;
    }

    public bool autoStart = true;
    public LanguageData[] availableLanguages = new LanguageData[2];

    private void Awake()
    {
        LanguageManager.onLanguageUpdated += UpdateText;
    }

    public void Start()
    {
        if(autoStart)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = availableLanguages[(int)LanguageManager.Language].text.Replace("\\n", "\n");
    }
}
