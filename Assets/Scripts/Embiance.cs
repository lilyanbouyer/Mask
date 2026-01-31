using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    public void SetMaskEmbient()
    {
        RenderSettings.fog = true;
        RenderSettings.ambientLight = new Color(52f/255f, 0f, 0f);
        RenderSettings.fogDensity = 0.246f;
    }

    public void SetNormalEmbient()
    {
        RenderSettings.fog = true;
        RenderSettings.ambientLight = new Color(0f, 0f, 0f);
        RenderSettings.fogDensity = 0.068f;
    }
}
