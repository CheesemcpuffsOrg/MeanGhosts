using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

    [SerializeField]GameObject highBeam;
    [SerializeField]GameObject normalBeam;

    [SerializeField]bool beamControl = false;

    // Start is called before the first frame update
    void Start()
    {
        normalBeam.SetActive(true);
        highBeam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeamControl()
    {
        beamControl = !beamControl;

        if(beamControl)
        {
            normalBeam.SetActive(false);
            highBeam.SetActive(true);
        }
        else
        {
            normalBeam.SetActive(true);
            highBeam.SetActive(false);
        }
    }
}
