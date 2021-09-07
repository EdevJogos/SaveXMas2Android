using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int TotalYetis { get { return _yetiIDGiver; } }

    public Player player;
    public Yeti yetiPrefab;

    private int _yetiIDGiver;
    private List<Yeti> _hiredYetis = new List<Yeti>();

    public void Initiate()
    {
        player.Initiate();
    }

    public void InitializePlayer()
    {
        player.gameObject.SetActive(true);
    }

    public void ResetUnits()
    {
        player.gameObject.SetActive(false);

        for (int __i = 0; __i < _hiredYetis.Count; __i++)
        {
            Destroy(_hiredYetis[__i].gameObject);
        }

        _yetiIDGiver = 0;
        _hiredYetis.Clear();
    }

    public void HireYeti()
    {
        Yeti __yeti = Instantiate(yetiPrefab, new Vector2(0f, -3.74f), Quaternion.identity);

        __yeti.Initialize(_yetiIDGiver);
        _yetiIDGiver++;

        _hiredYetis.Add(__yeti);
    }
}
