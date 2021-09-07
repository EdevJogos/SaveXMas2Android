using UnityEngine;

public class HUDDisplay : Display
{
    public TMPro.TextMeshProUGUI pointsText;
    public TMPro.TextMeshProUGUI lostText;

    public override void UpdateDisplay(int p_operation, int p_value, int p_data)
    {
        switch(p_operation)
        {
            case 0:
                OnScoreUpdated(p_value, p_data);
                break;
            case 1:
                OnGiftLost(p_value);
                break;
        }

        base.UpdateDisplay(p_operation, p_value, p_data);
    }

    private void OnGiftLost(int p_lost)
    {
        lostText.text = p_lost + "/10";
    }

    private void OnScoreUpdated(int p_points, int p_streak)
    {
        pointsText.text = p_points + " s" + p_streak;
    }
}
