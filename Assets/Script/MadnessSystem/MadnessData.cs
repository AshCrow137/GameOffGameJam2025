using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MadnessData", menuName = "Scriptable Objects/MadnessData")]
public class MadnessData : ScriptableObject
{
    [SerializeField]
    private const int NONE = 0;
    [SerializeField]
    private const int less10 = 25;
    [SerializeField]
    private const int less25 = 50;
    [SerializeField]
    private const int less35 = 75;
    [SerializeField]
    private const int almostDefeat = 100;
    [SerializeField]
    private List<MadnessDataStruct> madnessEffectsList = new List<MadnessDataStruct>(5);
    
    public MadnessDataStruct GetMadnessEffects(int currentMadness)
    {
        foreach(MadnessDataStruct mstruct in madnessEffectsList)
        {
            if(currentMadness>=mstruct.minMadness&&currentMadness<=mstruct.maxMadness)
            {
                return mstruct;
            }
        }
        return madnessEffectsList[0];
    }

}
[Serializable]
public struct MadnessDataStruct
{
    public string name;
    public int minMadness;
    public int maxMadness;
    public int CreatureStatsModifier;
    public int DiplomacyModifier;
    //TODO add madness special effets

}
