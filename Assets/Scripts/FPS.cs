using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public int FramesPerSec { get; protected set; }
    float frequency = 0.5f;
    public TMPro.TextMeshProUGUI counter;
	
    void Start()
    {
        //counter = GetComponent<TMPro.TextMeshProUGUI>();
        counter.text = "";
        StartCoroutine(SAYAC());
    }

    IEnumerator SAYAC()
    {
        for (; ; )
        {
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
 
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;
 
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
            counter.text = "FPS: " + FramesPerSec.ToString();
        }
    }
}
