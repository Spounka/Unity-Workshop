using UnityEngine;

public class CameraBobbing
{

    public float UpdateXPos(float waveLength, float amplitude)
    {
        return Mathf.Cos(waveLength * Time.time * Mathf.Rad2Deg) * amplitude;
    }

    public float UpdateYPos(float waveLength, float amplitude)
    {
        return -Mathf.Abs(Mathf.Sin(waveLength * Time.time * Mathf.Rad2Deg) * amplitude) - amplitude ;
    }

}
