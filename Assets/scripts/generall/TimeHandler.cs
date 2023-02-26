using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeHandler : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Color color;
    public Gradient colorGradient;
    private ColorGrading colorGrading;

    public float multiplier;
    public float time;
    private float last_time;
    public int day = 1;

    private void Awake()
    {
        Globals.timeHandler = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        colorGrading.colorFilter.value = color;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * multiplier / 3600;
        if (time > 24)
        {
            day++;
            time = 0;
        }
        if(time < 12)
        {
            colorGrading.colorFilter.value = colorGradient.Evaluate(time / 12);
        }
        else
        {
            colorGrading.colorFilter.value = colorGradient.Evaluate((24 - time) / 12);
        }
        if(last_time < 6 && time >= 6)
        {
            foreach(Mob mob in Globals.mobs)
            {
                mob.Morning();
            }
        }
        last_time = time;
    }
    public bool isNight()
    {
        return time < 6 || time > 18;
    }
}
