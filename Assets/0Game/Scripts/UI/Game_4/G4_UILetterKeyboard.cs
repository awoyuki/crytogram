using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class G4_UILetterKeyboard : MonoBehaviour
{
    [SerializeField] ButtonEffectLogic btn;
    [SerializeField] Text txt_letter;
    [SerializeField] Image button_bg;
    [SerializeField] Image line;
    [SerializeField] Material front_mat;

    private G4_UIKeyboard key_board;
    [HideInInspector] public char letter;

    private Camera main_camera;

    private void Awake()
    {
        btn.onDown.AddListener(OnButtonDown);
        button_bg.enabled = false;
        line.enabled = false;
        txt_letter.color = Color.black;
        main_camera = Camera.main;
    }

    public void Init(char letter, G4_UIKeyboard new_key_board)
    {
        key_board = new_key_board;
        txt_letter.text = letter.ToString();
        this.letter = letter;
    }

    private void OnButtonDown()
    {
        if (GameManager.Instance.GameState != GameState.InProgress)
            return;

        if (key_board.IsCreating)
            return;
        key_board.StartCreateAnswer(this);
        OnSelectLetter(true);
        btn.onUp.AddListener(OnButtonUp);
    }
    private void OnButtonUp()
    {
        key_board.ReturnAnswer();
        OnSelectLetter(false);
        btn.onUp.RemoveListener(OnButtonUp);
    }

    public void OnSelectLetter(bool select)
    {
        if (select)
        {
            button_bg.enabled = true;
            line.enabled = true;
            txt_letter.color = Color.white;
            btn.hasEffect = false;
            txt_letter.material = front_mat;
            button_bg.material = front_mat;
        }
        else
        {
            line.rectTransform.sizeDelta = new Vector3(0, 0);
            button_bg.enabled = false;
            line.enabled = false;
            txt_letter.color = Color.black;
            btn.hasEffect = true;
            txt_letter.material = null;
            button_bg.material = null;
        }
    }

    public void BindingCreatingAnswer()
    {
        btn.onEnter.AddListener(OnButtonEnter);
    }
    public void RemoveBindingCreatingAnswer()
    {
        btn.onEnter.RemoveListener(OnButtonEnter);
        OnSelectLetter(false);
    }

    private void OnButtonEnter()
    {
        if (key_board.cur_letter_keyboard_list.Contains(this))
        {
            var list = key_board.cur_letter_keyboard_list;
            var check_key = key_board.cur_letter_index - 1;
            if (check_key >= 0 && list[check_key] == this)
            {
                var target = list[key_board.cur_letter_index];
                key_board.RemoveLetterFromAnswer(target);
                target.OnSelectLetter(false);
            }
        }
        else
        {
            key_board.AddLetterToAnswer(this);
            OnSelectLetter(true);
        }
    }


    public void UpdateDrawingLine()
    {
        var target_pos = Input.mousePosition;

        if (Input.touchCount > 0)
            target_pos = Input.touches[0].position;

        Vector3 world_target_pos = main_camera.ScreenToWorldPoint(target_pos);
        LockDrawingLineToWorldPos(world_target_pos);
    }

    public void LockDrawingLineToWorldPos(Vector3 target, bool locked = false)
    {
        var relative_target = transform.InverseTransformPoint(target);
        relative_target.z = 0f;

        var x = line.rectTransform.sizeDelta.x; 
        var y = line.rectTransform.parent.rect().sizeDelta.y / 5.0f; 

        Vector3 dif = relative_target - line.rectTransform.localPosition;

        var target_length = new Vector2(dif.magnitude, y);
        var target_rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan2(dif.y, dif.x) / Mathf.PI));

        if (!locked)
        {
            line.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(x, y), target_length, 20.0f * Time.deltaTime);
            line.rectTransform.rotation = Quaternion.Slerp(line.rectTransform.rotation, target_rotation, 20.0f * Time.deltaTime);

        }
        else
        {
            line.rectTransform.sizeDelta = target_length;
            line.rectTransform.rotation = target_rotation;
        }

    }

}
