using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    // Start is called before the first frame update

    public Text display_Text;

    int frameCounter = 0;
    float timeCounter = 0.0f;
   
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ++frameCounter;
        timeCounter +=  Time.deltaTime;

        if (timeCounter >= 1.0f)
        {
            int count = (int)(frameCounter / timeCounter);
            display_Text.text = count.ToString() + " fps";
            frameCounter = 0;
            timeCounter = 0.0f;
        }



    }
}
