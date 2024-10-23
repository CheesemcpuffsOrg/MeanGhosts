using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISpotted
{
    void SpottedByTorch();
    void NotSpottedByTorch();
    void SpottedByHighBeam();
    void NotSpottedByHighBeam();
    void StateOfFlashLight(FlashLightState state);
}
