using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerControls playerControls;
    public bool isLegacyControlsOn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        playerControls = new PlayerControls();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isLegacyControlsOn = !isLegacyControlsOn;
            Debug.Log($"Is legacy control active: {isLegacyControlsOn}");
        }
    }

    public Vector2 GetAccelDecel()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.AccelerationDeceleration.ReadValue<Vector2>();

        return playerControls.NewPlayerControls.AccelerationDeceleration.ReadValue<Vector2>();
    }

    public Vector2 GetRotation()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.Rotation.ReadValue<Vector2>();

        return playerControls.NewPlayerControls.Rotation.ReadValue<Vector2>();
    }

    public Vector2 GetLookPosition()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.Look.ReadValue<Vector2>();

        return playerControls.NewPlayerControls.Look.ReadValue<Vector2>();
    }

    public bool GetPrimaryFire()
    {
        if(isLegacyControlsOn)
            return playerControls.OldPlayerControls.PrimaryFire.IsPressed();

        return playerControls.NewPlayerControls.PrimaryFire.IsPressed();
    }

    public bool GetSecondaryFire()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.SecondaryFire.IsPressed();

        return playerControls.NewPlayerControls.SecondaryFire.IsPressed();
    }

    public bool GetFlare()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.ShootFlare.IsPressed();

        return playerControls.NewPlayerControls.ShootFlare.IsPressed();
    }

    public bool GetLaserGun()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.SelectLaserGun.IsPressed();

        return playerControls.NewPlayerControls.SelectLaserGun.IsPressed();
    }

    public bool GetVulkanGun()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.SelectVulkanGun.IsPressed();

        return playerControls.NewPlayerControls.SelectVulcanGun.IsPressed();
    }

    public bool GetConcussionMissiles()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.SelectConcussionMissile.IsPressed();

        return playerControls.NewPlayerControls.SelectConcussionMissile.IsPressed();
    }

    public bool GetHomingMissiles()
    {
        if (isLegacyControlsOn)
            return playerControls.OldPlayerControls.SelectHomingMissile.IsPressed();

        return playerControls.NewPlayerControls.SelectHomingMissile.IsPressed();
    }

    public Vector2 GetAscendDescent()
    {
        return playerControls.NewPlayerControls.AscendDescent.ReadValue<Vector2>();
    }

    public void OnEnable()
    {
        playerControls.Enable();
    }

    public void OnDisable()
    {
        playerControls.Disable();
    }
}
