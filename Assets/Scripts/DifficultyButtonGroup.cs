using UnityEngine;

public class DifficultyButtonGroup : MonoBehaviour
{
    DifficultyButton currentSelected;
    void Start()
    {
        DifficultyButton[] buttons = GetComponentsInChildren<DifficultyButton>();
        foreach(DifficultyButton btn in buttons)
        {
            if(btn.difficulty == GameManager.Difficulty.Easy)
            {
                SelectedButton(btn);
            }
        }
    }
    public void SelectedButton(DifficultyButton button)
    {
        if(currentSelected != null)
        {
            currentSelected.SetSelected(false);
        }
        currentSelected = button;
        currentSelected.SetSelected(true);
    }
}