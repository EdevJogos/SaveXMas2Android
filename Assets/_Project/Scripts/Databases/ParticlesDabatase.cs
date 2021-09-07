using UnityEngine;

public class ParticlesDabatase : MonoBehaviour
{
    [System.Serializable]
    public struct ParticleData
    {
        public Particles ID;
        public GameObject particle;
    }

    public static ParticlesDabatase Instance;

    public ParticleData[] particles;

    public void Initiate()
    {
        Instance = this;
    }

    public static void InstantiateParticle(Particles p_id, Vector2 p_position)
    {
        GameObject __particlePrefab = GetParticle(p_id);

        Instantiate(__particlePrefab, p_position, __particlePrefab.transform.rotation);
    }

    public static void InstantiateParticle(Particles p_id, Vector2 p_position, Quaternion p_rotation)
    {
        GameObject __particlePrefab = GetParticle(p_id);

        Instantiate(__particlePrefab, p_position, p_rotation);
    }

    public static GameObject InstantiateParticle(Particles p_id, Transform p_parent)
    {
        return  Instantiate(GetParticle(p_id), p_parent);
    }

    private static GameObject GetParticle(Particles p_id)
    {
        for (int __i = 0; __i < Instance.particles.Length; __i++)
        {
            if(Instance.particles[__i].ID == p_id)
            {
                return Instance.particles[__i].particle;
            }
        }

        return null;
    }
}
