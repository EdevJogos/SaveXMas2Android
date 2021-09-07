using UnityEngine;

public class GiftsWorker : MonoBehaviour
{
    public GiftsDatabase giftsDatabase;

    public Gift SpawnRandomGift(Vector2 p_position)
    {
        Gift __gift = Instantiate(giftsDatabase.giftPrefab, p_position, Quaternion.identity);

        __gift.Initialize(giftsDatabase.GetRandomGiftData());

        return __gift;
    }
}
