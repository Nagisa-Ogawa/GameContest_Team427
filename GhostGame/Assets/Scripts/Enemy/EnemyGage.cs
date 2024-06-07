using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGage : MonoBehaviour
{
    [SerializeField]
    private Image EnemyRedGage;
    
    [SerializeField]
    private Image EnemyWhiteGage;

    private  EnemyBase enemy;
    private Tween whiteGaugeTween;


    public void GageReduction(float reducationValue, float time = 1.0f)
    {
        Debug.Log(" enemy.hp " + enemy.hp);
        Debug.Log("enemy.maxHp " + enemy.maxHp);
        var valueFrom = enemy.hp / enemy.maxHp;
        var valueTo = (enemy.hp - reducationValue) / enemy.maxHp;

        //ÔƒQ[ƒWŒ¸­
        EnemyRedGage.fillAmount = valueTo;



        EnemyRedGage.DOFillAmount(valueTo, time)
            .OnComplete(() =>
            {
                EnemyWhiteGage
                    .DOFillAmount(valueTo, time / 2f)
                    .SetDelay(0.5f);
            });

    }

    public void SetEnemy(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    
}
