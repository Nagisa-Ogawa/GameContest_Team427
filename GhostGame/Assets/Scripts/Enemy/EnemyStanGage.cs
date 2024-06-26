using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStanGage: MonoBehaviour
{
    [SerializeField]
    private Image EnemyYellowGage;
    
    [SerializeField]
    private Image EnemyWhiteGage;

    private  EnemyBase enemy;
    private Tween whiteGaugeTween;


    public void GageReduction(float reducationValue, float time = 1.0f)
    {
        Debug.Log(" enemy.stan " + enemy.stanPoint);
        Debug.Log("enemy.maxstan " + enemy.maxStanPoint);
        var valueFrom = enemy.stanPoint / enemy.maxStanPoint;
        var valueTo = (enemy.stanPoint - reducationValue) / enemy.maxStanPoint;

        //ÔƒQ[ƒWŒ¸­
        EnemyYellowGage.fillAmount = valueTo;



        EnemyYellowGage.DOFillAmount(valueTo, time)
            .OnComplete(() =>
            {
                EnemyWhiteGage
                    .DOFillAmount(valueTo, time / 2f)
                    .SetDelay(0.5f);
            });

    }

    public void GageGain(/*float gainValue,*/ float time = 1.0f)
    {
        Debug.Log(" enemy.stan " + enemy.stanPoint);
        Debug.Log("enemy.maxstan " + enemy.maxStanPoint);
        var valueFrom = enemy.stanPoint / enemy.maxStanPoint;
        var valueTo =  1.0f;

        //‰©ƒQ[ƒWŒ¸­
        EnemyYellowGage.fillAmount = valueTo;



        EnemyYellowGage.DOFillAmount(valueTo, time)
            .OnComplete(() =>
            {
                EnemyWhiteGage
                    .DOFillAmount(valueTo, time / 2f)
                    .SetDelay(0.5f);
            });
    }

    public void SetStanEnemy(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    
}
