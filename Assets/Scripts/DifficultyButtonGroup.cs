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
    [SerializeField]
    private Vector3 easyPos;
    [SerializeField]
    private Vector3 normalPos;
    [SerializeField]
    private Vector3 hardPos;

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
    public void selectAiPlayer()
    {
        switch (GameManager.instance.d)
        {
            case GameManager.Difficulty.Easy:
                Instantiate(easyEnemy,easyPos,Quaternion.identity);
                break;
            case GameManager.Difficulty.Normal:
                Instantiate(normalEnemy,normalPos,Quaternion.identity);
                break;
            case GameManager.Difficulty.Hard:
                Instantiate(hardEnemy,hardPos,Quaternion.identity);
                break;
        }
        
    }
}