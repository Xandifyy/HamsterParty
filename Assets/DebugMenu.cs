using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    public Slider AccelSlider;
    public Slider JumpSlider;
    public Slider HoldSlider;
    public Slider WallBounceSlider;

    public PlayerMovement playerMovement;
    public JumpMovement JumpMovement;

    public Text AccelSliderText;
    public Text JumpSliderText;
    public Text HoldSliderText;
    public Text WallBounceSliderText;

    // Update is called once per frame
    void Update()
    {
        AccelSliderText.text = "Accel Speed: " + AccelSlider.value.ToString();
        JumpSliderText.text = "Jump Speed: " + JumpSlider.value.ToString();
        HoldSliderText.text = "Hold Jump Speed: " + HoldSlider.value.ToString();
        WallBounceSliderText.text = "Wall Bounce Momentum: " + WallBounceSlider.value.ToString();

        playerMovement.accelerationRate = AccelSlider.value;
        JumpMovement.defaultJump = JumpSlider.value;
        JumpMovement.maxJump = HoldSlider.value;
        playerMovement.wallMomentum = WallBounceSlider.value;

    }
}
