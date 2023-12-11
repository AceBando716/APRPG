using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
     

    public Image experienceBar;
    public Text levelText;
    public PlayerController playerController;

    [SerializeField] private float maxWidth;

    void Update()
    {
        UpdateExperienceBar();
        UpdateLevelText();
    }

    void UpdateExperienceBar()
    {
        float experienceFraction = (float)playerController.GetCurrentExp() / playerController.GetExpToNextLevel();
        experienceBar.rectTransform.sizeDelta = new Vector2(experienceFraction * maxWidth, experienceBar.rectTransform.sizeDelta.y);
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + playerController.GetPlayerLevel().ToString();
    }
}
