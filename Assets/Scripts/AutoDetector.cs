// 同时判断所有支持的输入方式，有任意输入即可

using Assets.Scripts;
using UnityEngine;
using System.Collections.Generic;

public class AutoDetector : MonoBehaviour, IInputDetector
{
    private List<IInputDetector> detectors = new List<IInputDetector>{new ArrowKeysDetector(), new SwipeDetector()};

    public InputDirection? DetectInputDirection()
    {
        foreach( IInputDetector inputDetector in detectors)
        {
            InputDirection? direction = inputDetector.DetectInputDirection(); 
            if (direction != null)
                return direction;
        }
        return null;
    }
}