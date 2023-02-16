using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Era", menuName = "AddEra")]
public class ErasData : ScriptableObject
{
    [SerializeField] private int eraNo;
    public int EralNo { get { return eraNo; } }

    [SerializeField] private string eraName;
    public string EraName { get { return eraName; } }
}
