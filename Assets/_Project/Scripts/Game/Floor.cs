using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.tag != "Gift")
            return;

        Gift __gift = p_other.GetComponent<Gift>();

        __gift.RequestDestroy();
    }
}
