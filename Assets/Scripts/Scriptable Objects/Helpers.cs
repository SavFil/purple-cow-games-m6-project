using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu (fileName="Helper", menuName="SHMUP/Helper")]
public class Helpers : ScriptableObject
{
    public int nextFreePatternID = 0;
}
