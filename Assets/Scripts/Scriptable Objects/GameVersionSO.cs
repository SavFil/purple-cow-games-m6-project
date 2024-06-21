using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameVersion", menuName = "GameSettings/GameVersion")]
public class GameVersionSO : ScriptableObject
{
    [Tooltip("The major version number. Increment for major updates or breaking changes.")]
    [SerializeField] private int major = 1;

    [Tooltip("The minor version number. Increment for new features in a backwards-compatible manner.")]
    [SerializeField] private int minor = 0;

    [Tooltip("The patch version number. Increment for backwards-compatible bug fixes.")]
    [SerializeField] private int patch = 0;

    public string Version => $"{major}.{minor}.{patch}";
}
