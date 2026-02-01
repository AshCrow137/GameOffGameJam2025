using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is obviously for testing. 
/// Usage: Add all your toggles here. If one is on, others will automatically be off.
/// </summary>
public class ToggleManager : MonoBehaviour
{
    [SerializeField] private Toggle[] toggles;
    [SerializeField] private TextMeshProUGUI toggleState;

    public static ToggleManager Instance { get; private set; }

    // Since this is a test script. Not adding it to bootmanager

    public void Initialize()
    {
        Instatiate();
        InitializeToggles();
    }

    private void Instatiate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitializeToggles()
    {
        // Turn off all toggles on awake
        foreach (var toggle in toggles)
        {
            if (toggle != null)
            {
                toggle.isOn = false;
            }
        }

        toggleState.text = "None";

        // Add listeners for each toggle
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null)
            {
                int index = i; // Capture index for closure
                toggles[i].onValueChanged.AddListener((isOn) => OnToggleChanged(index, isOn));
            }
        }
    }

    private void OnDestroy()
    {
        // Clean up listeners
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null)
            {
                int index = i; // Capture index for closure
                toggles[i].onValueChanged.RemoveListener((isOn) => OnToggleChanged(index, isOn));
            }
        }
    }

    private void OnToggleChanged(int toggleIndex, bool isOn)
    {
        if (isOn)
        {
            // Turn off all other toggles
            for (int i = 0; i < toggles.Length; i++)
            {
                if (i != toggleIndex && toggles[i] != null)
                {
                    toggles[i].isOn = false;
                }
            }
            ToggleUseCase useCase = (ToggleUseCase)toggleIndex;
            toggleState.text = useCase.ToString();
        }
        else
        {
            toggleState.text = "None";
        }
    }

    public bool GetToggleState(ToggleUseCase useCase)
    {
        int toggleIndex = (int)useCase;
        return toggles[toggleIndex].isOn;
    }
}

