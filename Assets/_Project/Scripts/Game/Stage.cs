using UnityEngine;

public class Stage
{
    public float speedRatio;
    public Vector2 quantityMinMax;
    public Vector2 spawnMinMax;
    public Vector2 windMinMax;
    public Vector2 delayMinMax;

    public int Quantity { get { return Random.Range((int)quantityMinMax.x, (int)quantityMinMax.y); } }
    public float SpawnDelay { get { return Random.Range(spawnMinMax.x, spawnMinMax.y); } }
    public float WindDelay { get { return Random.Range(windMinMax.x, windMinMax.y); } }
    public float Delay { get { return Random.Range(delayMinMax.x, delayMinMax.y); } }


    public Stage(float p_speedRatio, Vector2 p_delayMinMax, Vector2 p_quanityMinMax, Vector2 p_spawnMinMax, Vector2 p_windMinMax)
    {
        speedRatio = p_speedRatio;
        quantityMinMax = p_quanityMinMax;
        spawnMinMax = p_spawnMinMax;
        windMinMax = p_windMinMax;
        delayMinMax = p_delayMinMax;
    }
}
