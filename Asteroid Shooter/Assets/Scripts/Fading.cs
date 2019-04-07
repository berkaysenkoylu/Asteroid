using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    void OnGUI()
    {
        // Change alpha value
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        // Clamp
        alpha = Mathf.Clamp01(alpha);

        // Set color of GUI. All color values remain same except for alpha value
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Set depth
        GUI.depth = drawDepth;

        // Draw texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    // Set the fade direction parameter making the scene fade in if parameter is -1, fade out if 1
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }

    void OnLevelWasLoaded(int level)
    {
        BeginFade(-1);
    }

}
