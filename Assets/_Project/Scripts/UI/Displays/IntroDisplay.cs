using UnityEngine;

public class IntroDisplay : Display
{
    public ParticleSystem snowFlakeExplosion;

    public override void RequestAction(int p_action)
    {
        snowFlakeExplosion.Play();

        base.RequestAction(p_action);
    }
}
