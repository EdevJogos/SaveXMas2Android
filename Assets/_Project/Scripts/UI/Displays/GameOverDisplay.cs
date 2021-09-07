using UnityEngine;

public class GameOverDisplay : Display
{
    public TMPro.TextMeshProUGUI resultsText;
    public ParticleSystem snowFlakeExplosion;
    public MultiLanguage.LanguageData[] resultText = new MultiLanguage.LanguageData[2];

    public override void Show(bool p_show, System.Action p_callback, float p_ratio)
    {
        if (p_show)
        {
            snowFlakeExplosion.Play();
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    public override void UpdateDisplay(int p_operation, int[] p_data)
    {
        resultsText.text = string.Format(resultText[(int)LanguageManager.Language].text.Replace("\\n", "\n"), p_data[0], p_data[1], p_data[2]);

        base.UpdateDisplay(p_operation, p_data);
    }
}
