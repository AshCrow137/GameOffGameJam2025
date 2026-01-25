public interface IMadnessable
{
    void IncreaseMadness(int amount);

    void DecreaseMadness(int amount);

    MadnessEffect GetMadnessEffects();
}
