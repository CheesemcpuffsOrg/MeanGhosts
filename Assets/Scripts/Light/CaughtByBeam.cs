using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CaughtByBeam : MonoBehaviour
{
    GameObject player;

    [Header ("Torch")]
    [SerializeField] List<GameObject> spottedInterfaces = new List<GameObject>();
    FlashLight flashLight;
    bool spotted;
    public bool Spotted => spotted;

    bool started = false;

    [Header("References")]
    [SerializeField] ReferenceScriptableObject playerReference;

    private void Start()
    {
        player = ReferenceManager.ReferenceManagerInstance.GetReference(playerReference);

        flashLight = player.GetComponentInChildren<FlashLight>();

        started = true;
        OnStartOrEnable();
    }

    public void SpottedByTorch(bool isSpotted)
    {
        ISpotted iSpotted;

        spotted = isSpotted;

        if (isSpotted)
        {
            foreach (var i in spottedInterfaces)
            {
                iSpotted = i.GetComponent<ISpotted>();

                iSpotted.SpottedByTorch();
            }
        }
        else
        {
            foreach (var i in spottedInterfaces)
            {
                iSpotted = i.GetComponent<ISpotted>();

                iSpotted.NotSpottedByTorch();
            }
        }
    }

    public void SpottedByHighBeam(bool isSpotted)
    {
        ISpotted iSpotted;

        spotted = isSpotted;

        if (isSpotted)
        {
            foreach (var i in spottedInterfaces)
            {
                iSpotted = i.GetComponent<ISpotted>();

                iSpotted.SpottedByHighBeam();
            }
        }
        else
        {
            foreach (var i in spottedInterfaces)
            {
                iSpotted = i.GetComponent<ISpotted>();

                iSpotted.NotSpottedByHighBeam();
            }
        }
    }

    void FlashlightStateChange(FlashLightState state)
    {
        ISpotted iSpotted;

        foreach (var i in spottedInterfaces)
        {
            iSpotted = i.GetComponent<ISpotted>();

            iSpotted.StateOfFlashLight(state);
        }
    }

    void OnStartOrEnable()
    {
        flashLight.FlashLightStateChange += FlashlightStateChange;
    }

    private void OnEnable()
    {
        if (started) OnStartOrEnable();
    }

    private void OnDisable()
    {
        flashLight.FlashLightStateChange -= FlashlightStateChange;
    }

}
