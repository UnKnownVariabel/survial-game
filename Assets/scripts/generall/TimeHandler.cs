using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeHandler : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Gradient colorGradient;
    private ColorGrading colorGrading;
    public Gradient vignetteGradient;
    private Vignette vignette;

    public float multiplier;
    public float time;
    private float last_time;
    public int day = 1;
    public Transform minuteArm;
    public Transform hourArm;

    private void Awake()
    {
        Globals.timeHandler = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        postProcessVolume.profile.TryGetSettings(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * multiplier / 3600;
        minuteArm.localEulerAngles = new Vector3(0, 0, -360 * (time - Mathf.Floor(time)));
        hourArm.localEulerAngles = new Vector3(0, 0, -360 * time / 12);
        if (time > 24)
        {
            day++;
            time = 0;
        }
        if(time < 12)
        {
            colorGrading.colorFilter.value = colorGradient.Evaluate(time / 12);
            vignette.intensity.value = 1 - vignetteGradient.Evaluate(time / 12).grayscale;
        }
        else
        {
            colorGrading.colorFilter.value = colorGradient.Evaluate((24 - time) / 12);
            vignette.intensity.value = 1 - vignetteGradient.Evaluate((24 - time) / 12).grayscale;
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
