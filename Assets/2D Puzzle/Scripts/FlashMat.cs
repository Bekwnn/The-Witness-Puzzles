using UnityEngine;
using System.Collections;

public class FlashMat : MonoBehaviour {

    public Material[] matsFlashed;
    public Color flashColor;
    public int flashCount;
    public float flashDuration;
    public float flashCooldown;

    private Color[] startColors;
    private bool bFlashing;
    private float timer;
    private float cooldownTimer;
    private int count;
    private bool bFlashingUp;
    
	void Start () {
        //save default colors
        startColors = new Color[matsFlashed.Length];
        
        for (int i = 0; i < matsFlashed.Length; i++)
        {
            startColors[i] = matsFlashed[i].color;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (bFlashing)
        {
            timer += Time.deltaTime;
            if (timer > flashDuration)
            {
                count++;

                //if count*2 = flashCount we're done. Make sure colors are right and quit.
                if (count/2 >= flashCount)
                {
                    for (int i = 0; i < matsFlashed.Length; i++)
                    {
                        matsFlashed[i].color = startColors[i];
                    }
                    bFlashing = false;
                    cooldownTimer = flashCooldown;
                    return;
                }

                //flip direction, reset timer
                bFlashingUp = (bFlashingUp) ? false : true;
                timer -= flashDuration;
            }

            //heading towards the warning color
            if (bFlashingUp)
            {
                for (int i = 0; i < matsFlashed.Length; i++)
                {
                    matsFlashed[i].color = Color.Lerp(startColors[i], flashColor, timer / flashDuration);
                }
            }
            //heading away from the warning color
            else
            {
                for (int i = 0; i < matsFlashed.Length; i++)
                {
                    matsFlashed[i].color = Color.Lerp(flashColor, startColors[i], timer / flashDuration);
                }
            }
        }
        else
        {
            if (cooldownTimer >= 0f)
                cooldownTimer -= Time.deltaTime;
        }
	}

    public void Flash()
    {
        if (!bFlashing && cooldownTimer < 0f)
        {
            bFlashing = true;
            bFlashingUp = true;
            timer = 0f;
            count = 0;
        }
    }
}
