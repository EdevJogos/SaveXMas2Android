using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public GameObject pointsPrefab;
    public Sprite[] pointSprites;

    public void Initiate()
    {
        
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void ScorePoints(Gift p_gift, int p_streak, Vector2 p_position)
    {
        ParticlesDabatase.InstantiateParticle(Particles.STAR_BURST, p_position);
        GameObject __pointsAnimation = Instantiate(pointsPrefab, p_position, Quaternion.identity);

        int __id = p_streak >= 30 ? 2 : p_streak >= 10 ? 1 : 0;
        int __points = __id == 0 ? 10 : __id == 1 ? 50 : 100;

        __pointsAnimation.GetComponentInChildren<SpriteRenderer>().sprite = pointSprites[__id];

        currentScore += __points;
    }
}
