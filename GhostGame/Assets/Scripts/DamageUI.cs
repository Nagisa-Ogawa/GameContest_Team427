
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{

    private Text damageText;
    private Outline outline;

    private Color damageFadeoutColor;
    private Color outlineFadeoutColor;
    //　フェードアウトするスピード
    private float fadeOutSpeed = 2.0f;
    //　移動値
    [SerializeField]
    private float moveSpeed = 0.8f;

    void Start()
    {
        //damageText = GetComponent<Text>();
        //outline = GetComponent<Outline>();

        //damageFadeoutColor = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 0.0f);
        //outlineFadeoutColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 0.0f);

    }
    
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        damageText.color = Color.Lerp(damageText.color, damageFadeoutColor, fadeOutSpeed * Time.deltaTime);
        outline.effectColor = Color.Lerp(outline.effectColor, outlineFadeoutColor, fadeOutSpeed * Time.deltaTime);

        if (damageText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        damageText = GetComponent<Text>();
        outline = GetComponent<Outline>();

        damageFadeoutColor = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 0.0f);
        outlineFadeoutColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 0.0f);

    }

    public void TextChange(string text)
    {
        damageText.text = text;
    }
}
