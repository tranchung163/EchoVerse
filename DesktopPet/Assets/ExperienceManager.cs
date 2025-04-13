using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    int currentLevel, totalExperience, leveupExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;

    void Start()
    {
        UpdateLevel();
    }

    public void AddExperience(int _amount)
    {
        totalExperience += _amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if(totalExperience >= leveupExperience)
        {
            currentLevel++;
            UpdateLevel();
        }

        UpdateInterface();
    }

    void UpdateLevel()
    {
        totalExperience = 0;
        leveupExperience += 1;
        UpdateInterface();
    }

    void UpdateInterface()
    {
        levelText.text = currentLevel.ToString();
        experienceText.text = totalExperience + " exp / " + leveupExperience + " exp";
        experienceFill.fillAmount = (float)totalExperience / (float)leveupExperience;
    }
}
