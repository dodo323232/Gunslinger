using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public DifficultyButtonGroup group;
    [SerializeField]
    Color normalColor;
    [SerializeField]
    Color hoverColor;
    [SerializeField]
    Color selectedColor;
    Image[] images;
    private bool isHovered = false;
    private bool isSelected = false;
    
    [SerializeField]
    public GameManager.Difficulty difficulty; // 변수


    void Awake()
    {
        group = GetComponentInParent<DifficultyButtonGroup>();
        images = GetComponentsInChildren<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData) // 마우스 버튼 위로 들어왔을 때 자동 실행
    {
        isHovered = true;
        ApplyColor();
    }

    public void OnPointerExit(PointerEventData eventData) // 마우스가 버튼 밖으로 나갔을 때 자동 싫행
    {
        isHovered = false;
        ApplyColor();
    }

    public void OnPointerClick(PointerEventData eventData) // 버튼을 클릭했을 때 자동 실행
    {
        group.SelectedButton(this);
        GameManager.instance.d = difficulty;
    }
    private void ApplyColor()
    {
        Color target = isSelected ? selectedColor : isHovered ? hoverColor : normalColor;
        foreach(Image img in images)
        {
            img.color = target;
        }
    }
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        ApplyColor();
    }

}