using System;
using UnityEngine;

public class ShopDisplay : Display
{
    public UnityEngine.UI.Button hireButton;
    public TMPro.TextMeshProUGUI timerText;
    public ParticleSystem hireExplosion;
    public ParticleSystem snowFlakeExplosion;

    public override void Show(bool p_show, Action p_callback, float p_ratio)
    {
        if(p_show)
        {
            snowFlakeExplosion.Play();
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    public override void UpdateDisplay(int p_operation, int p_value, int p_data)
    {
        if(p_operation == 0)
        {
            timerText.text = "Full";
            hireButton.interactable = false;
        }
        else if(p_operation == 1)
        {
            OnHireTimerUpdated();
        }
    }

    public override void RequestAction(int p_action)
    {
        hireExplosion.Play();

        base.RequestAction(p_action);
    }

    private void OnHireTimerUpdated()
    {
        float __minutes = Mathf.FloorToInt(StageManager.HireTimer / 60);
        float __seconds = Mathf.FloorToInt(StageManager.HireTimer % 60);

        timerText.text = "Grumpy Yeti\n" + string.Format("{0:0}:{1:00}", __minutes, __seconds);

        hireButton.interactable = StageManager.HireTimer == 0;
    }
}
