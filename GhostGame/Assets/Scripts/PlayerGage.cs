using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGage : MonoBehaviour
{
    [SerializeField]
    private Image GreenGage;
    [SerializeField]
    private Image RedGage;

    private PlayerController player;
    private Tween redGaugeTween;


    public void GageReduction(float reducationValue,float time = 1.0f)
    {
        Debug.Log(" player.hp " + player.hp);
        Debug.Log("player.maxHp " + player.maxHp);
        var valueFrom = player.hp / player.maxHp;
        var valueTo = (player.hp - reducationValue) / player.maxHp;

        //—ÎƒQ[ƒWŒ¸­
        GreenGage.fillAmount = valueTo;

        //if (redGaugeTween != null)
        //{
        //    redGaugeTween.Kill();
        //}

        ////ÔƒQ[ƒWŒ¸­
        //redGaugeTween = DOTween.To(
        //    () => valueFrom,
        //    x =>{
        //        RedGage.fillAmount = x;
        //    },
        //    valueTo,
        //    time
        //);

        GreenGage.DOFillAmount(valueTo, time)
            .OnComplete(() =>
            {
                RedGage
                    .DOFillAmount(valueTo, time / 2f)
                    .SetDelay(0.5f);
            });

    }

   public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }
}
