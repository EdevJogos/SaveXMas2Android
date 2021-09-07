using UnityEngine;

[System.Serializable]
public struct GiftData
{
    public int id;
    public float speed;
    public float weight;
    public Vector2 colliderSize;
    public Sprite[] sprite;

    public Sprite GetSprite()
    {
        return sprite[Random.Range(0, sprite.Length)];
    }
}

public class GiftsDatabase : MonoBehaviour
{
    public Gift giftPrefab;
    public GiftData[] giftsDatabase;

    public GiftData GetRandomGiftData()
    {
        return giftsDatabase[Random.Range(0, giftsDatabase.Length)];
    }
}
