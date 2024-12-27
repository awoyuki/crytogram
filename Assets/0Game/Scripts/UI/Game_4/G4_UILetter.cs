using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G4_UILetter : MonoBehaviour
{
    [SerializeField] Text txt_letter;
    [SerializeField] Image bg_image;
    [SerializeField] Color show_color;
    [SerializeField] Color def_color;
    [SerializeField] Gradient shake_color;

    [HideInInspector] public int index_row;
    [HideInInspector] public int index_col;
    public List<int> word_ids;

    [HideInInspector] public bool status;
    public bool IsEnable;
    public int fontSize => txt_letter.cachedTextGenerator.fontSizeUsedForBestFit;
    private RectTransform rectTransform;

    private int cachedFontSize;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cachedFontSize = txt_letter.fontSize;
    }

    public void DeactiveLetter()
    {
        bg_image.color = def_color;
        bg_image.enabled = false;
        txt_letter.enabled = false;
        txt_letter.text = "";
        IsEnable = false;
        index_row = 0;
        index_col = 0;
        word_ids = new List<int>();
        status = false;
    }

    public void ActiveLetter(G4_CrytoLetter data)
    {
        bg_image.color = def_color;
        bg_image.enabled = true;
        txt_letter.enabled = false;
        txt_letter.text = data.letter;
        index_row = data.row_id;
        index_col = data.col_id;
        word_ids = new List<int>(data.word_id);
        status = data.status;
        IsEnable = true;
    }

    public void ShowLetter()
    {
        txt_letter.enabled = true;
        bg_image.enabled = true;
        bg_image.color = show_color;
        status = true;
    }

    public void ShowLetterByCorrect(float speed = 0.3f)
    {
        bg_image.enabled = true;
        txt_letter.enabled = true;
        bg_image.DOColor(show_color, speed);
    }

    public void MoveToAnswerPanel(G4_UILetter target, float speed = 0.3f)
    {
        txt_letter.transform.DOMove(target.transform.position, speed).OnComplete(() =>
        {
            target.ShowLetterByCorrect(speed);
        });
    }

    public void MoveToAnswerPanel(Vector3 position, float speed = 0.3f)
    {
        txt_letter.transform.DOMove(position, speed);
    }

    public void ShakeBlink(bool vertical)
    {
        bg_image.DOGradientColor(shake_color, 0.3f);
        StartCoroutine(IE_Shake(0.3f, vertical));
    }

    private IEnumerator IE_Shake(float time, bool vertical)
    {
        float t = 0;
        while(t < time)
        {
            t += Time.deltaTime;
            var value = Mathf.Lerp(-2f, 2f, (Mathf.Sin(Mathf.PI * t * 3 / time) + 1) / 2);
            if(vertical)
                bg_image.transform.localPosition += new Vector3(0, value, 0);
            else
                bg_image.transform.localPosition += new Vector3(value, 0, 0);
            yield return null;
        }
        bg_image.transform.localPosition = Vector3.zero;
    }


    public void LetterKeyboardInit(string letter)
    {
        txt_letter.fontSize = cachedFontSize;
        txt_letter.transform.localPosition = Vector3.zero;
        txt_letter.text = letter;
        var tempColor = txt_letter.color;
        tempColor.a = 1f;
        txt_letter.color = tempColor;
        rectTransform.sizeDelta = new Vector2(txt_letter.preferredWidth + 10.0f, rectTransform.sizeDelta.y);
    }

    public void FadeHide()
    {
        txt_letter.DOKill();
        txt_letter.DOFade(0, 0.2f);
    }
    public void FadeShow()
    {
        txt_letter.DOKill();
        txt_letter.DOFade(1, 0.1f);
    }

}
