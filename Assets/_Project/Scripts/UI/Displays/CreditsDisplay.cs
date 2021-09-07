using UnityEngine;

public class CreditsDisplay : Display
{
    public ParticleSystem hireExplosion;
    public ParticleSystem snowFlakeExplosion;

    public override void Show(bool p_show, System.Action p_callback, float p_ratio)
    {
        if (p_show)
        {
            snowFlakeExplosion.Play();
        }
        else
        {
            hireExplosion.Play();
        }

        base.Show(p_show, p_callback, p_ratio);
    }
}
