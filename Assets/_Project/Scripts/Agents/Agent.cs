using UnityEngine;

public class Agent : MonoBehaviour
{
    public static event System.Action<Gift, Vector2> onGiftCaught;

    public void CatchGift(Gift p_gift, Vector2 p_position)
    {
        p_gift.GetComponent<BoxCollider2D>().enabled = false;

        onGiftCaught?.Invoke(p_gift, p_position);

        p_gift.caught = true;
    }
}