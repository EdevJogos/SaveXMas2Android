using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public class PlayTimer
    {
        public System.Action onCompleted;

        public bool loop;
        public float duration;
        public float endTime;

        public PlayTimer(bool p_loop, float p_duration, System.Action p_completed)
        {
            loop = p_loop;
            duration = p_duration;
            endTime = PlayTime + p_duration;
            onCompleted = p_completed;
        }
    }

    public static float PlayTime;

    private static List<PlayTimer> PlayTimers = new List<PlayTimer>();

    private void Initiate()
    {
        PlayTime = Time.time;
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        PlayTime += Time.deltaTime;

        for (int __i = 0; __i < PlayTimers.Count; __i++)
        {
            if(PlayTime >= PlayTimers[__i].endTime)
            {
                PlayTimers[__i].onCompleted?.Invoke();

                if(PlayTimers[__i].loop)
                {
                    PlayTimers[__i].endTime = PlayTime + PlayTimers[__i].duration;
                }
            }
        }
    }

    public static void AddTimer(PlayTimer p_time)
    {
        PlayTimers.Add(p_time);
    }

    public static void ResetTimers()
    {
        PlayTimers.Clear();
        PlayTime = Time.time;
    }
}