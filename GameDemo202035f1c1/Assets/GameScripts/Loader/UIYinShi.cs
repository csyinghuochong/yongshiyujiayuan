using System;
using UnityEngine;
using UnityEngine.UI;

public class UIYinShi : MonoBehaviour
{
    private Button _text_Button_1;
    private Button _text_Button_2;
    private Button _btn_yes;
    private Button _btn_no;

    private void Awake()
    {
        _text_Button_1 = transform.Find("Text_Button_1").GetComponent<Button>();
        _text_Button_2 = transform.Find("Text_Button_2").GetComponent<Button>();
        _btn_yes = transform.Find("Btn_yes").GetComponent<Button>();
        _btn_no = transform.Find("Btn_no").GetComponent<Button>();

        _text_Button_1.onClick.AddListener(() =>
        {
            string url = "http://verification.weijinggame.com/weijing/yinsi3.txt";
            Application.OpenURL(url);
        });
        _text_Button_2.onClick.AddListener(() =>
        {
            string url = "http://verification.weijinggame.com/weijing/yinsi4.txt";
            Application.OpenURL(url);
        });
        _btn_yes.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("GameYinSi", 1);
            Init.Instance.StartPatch();
            gameObject.SetActive(false);
        });
        _btn_no.onClick.AddListener(() => { Application.Quit(); });
    }
}