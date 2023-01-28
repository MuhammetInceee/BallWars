using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class Deneme : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            Denemee(i);
        }
    }

    private void Denemee(int a)
    {
        HapticTypes[] types = new[]
        {
            HapticTypes.Failure, HapticTypes.Selection, HapticTypes.None, HapticTypes.Success,
            HapticTypes.Warning, HapticTypes.HeavyImpact, HapticTypes.LightImpact, HapticTypes.MediumImpact,
            HapticTypes.RigidImpact, HapticTypes.SoftImpact
        };
        
        MMVibrationManager.Haptic(types[a]);
        
        print(types[a]);
    }

}
