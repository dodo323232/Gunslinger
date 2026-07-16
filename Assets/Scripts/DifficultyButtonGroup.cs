using UnityEngine;

public class DifficultyButtonGroup : MonoBehaviour
{
    DifficultyButton currentSelected;
    [SerializeField]
    private GameObject easyEnemy;
    [SerializeField]
    private GameObject normalEnemy;
    [SerializeField]
    private GameObject hardEnemy;

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
            switch (GameManager.instance.d)
            {
                case GameManager.Difficulty.Easy:
                    GameManager.instance.frame1Animator[(int)GameManager.instance.d].SetTrigger("IdleTrigger"); 
                    easyEnemy.SetActive(false);
                    break;
                case GameManager.Difficulty.Normal:
                    GameManager.instance.frame1Animator[(int)GameManager.instance.d].SetTrigger("IdleTrigger");
                    normalEnemy.SetActive(false);
                    break;
                case GameManager.Difficulty.Hard:
                    GameManager.instance.frame1Animator[(int)GameManager.instance.d].SetTrigger("IdleTrigger"); 
                    hardEnemy.SetActive(false);
                    break;
            }
            currentSelected.SetSelected(false);
        }
        currentSelected = button;
        currentSelected.SetSelected(true);
    }
    public void selectAiPlayer()
    {
        switch (GameManager.instance.d)
        {
            case GameManager.Difficulty.Easy:
                easyEnemy.SetActive(true);
                break;
            case GameManager.Difficulty.Normal:
                normalEnemy.SetActive(true);
                break;
            case GameManager.Difficulty.Hard:
                hardEnemy.SetActive(true);
                break;
        }
        
    }
}