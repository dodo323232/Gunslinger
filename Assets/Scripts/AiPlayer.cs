using System.Collections;
using UnityEngine;

public class AiPlayer : MonoBehaviour
{
    private Animator animator;
    

    // "두 개 이상의 데이터를 비교하거나 주고받을 때는, 무조건 데이터가 발생하는 '타이밍(시점)'을 하나로 통일시켜야 안전하다."

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void DecideReactionTime()
    {
        TimeManager.instance.randomReaction = 
        (float)System.Math.Round(Random.Range(GameManager.instance.minReaction[(int)GameManager.instance.d],
        GameManager.instance.maxReaction[(int)GameManager.instance.d]),1); // gamemager에 enum 이용해서 난이도 조절 만들거임
    }
    
}