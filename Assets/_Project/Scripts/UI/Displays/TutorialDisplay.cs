using System;
using UnityEngine;

public class TutorialDisplay : Display
{
    public GameObject[] tutorial;

    public override void Show(bool p_show, Action p_callback, float p_ratio)
    {
        if(p_show)
        {
            tutorial[0].SetActive(true);
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    public void Next()
    {
        tutorial[0].SetActive(false);
        tutorial[1].SetActive(true);
    }
}
