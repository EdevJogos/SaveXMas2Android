public  static class HelpExtensions
{
    public static System.Collections.Generic.List<string> IntStringLookup = new System.Collections.Generic.List<string>(290);

    public static void Initiate()
    {
        for (int __i = -60; __i < 230; __i++)
        {
            IntStringLookup.Add(__i.ToString());
        }
    }

    public static int ClampCircle(int p_value, int p_min, int p_max)
    {
        if (p_value > p_max)
        {
            return p_min;
        }
        else if (p_value < p_min)
        {
            return p_max;
        }
        else
        {
            return p_value;
        }
    }

    public static float ClampMin0(float p_value)
    {
        return p_value <= 0 ? 0 : p_value;
    }

    public static void SetAlpha(this UnityEngine.SpriteRenderer p_renderer, float p_alpha)
    {
        UnityEngine.Color __color = p_renderer.color;

        p_renderer.color = new UnityEngine.Color(__color.r, __color.g, __color.b, p_alpha);
    }
}
