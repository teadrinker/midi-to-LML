#if UNITY_2019_4_OR_NEWER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MidiToLMLUnity : MonoBehaviour
{

    public string path;
    public int transpose = 0;
    public int rightHandTranspose = 0;
    public int quantizeShift = 2;

    [TextArea(20, 8500)]
    public string LML;

    void OnEnable()
    {
        LML = MidiToLML.Convert(path, transpose, rightHandTranspose, quantizeShift, s=>Debug.Log(s));
    }

}

#endif