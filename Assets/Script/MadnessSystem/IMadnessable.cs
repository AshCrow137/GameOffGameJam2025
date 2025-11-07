using UnityEngine;

public interface IMadnessable
{
    void IncreaseMadness(float amount);

    void DecreaseMadness(float amount);

    MadnessEffect GetMadnessEffects();
}
