using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager instance = null;

    [SerializeField] private GameObject _keyboardGameObject;
    [SerializeField] private GameObject _numbersParent;
    [SerializeField] private GameObject[] _qweParents;
    [SerializeField] private InputField _inputField;
    
    private Button[] _tmpButtons;
    private List<Text> _qweButtonTexts = new List<Text>();
    private bool _qweUpperMode = false;
    private bool _shiftPressed = false;
    private string _savedText = "";

    public void PressClose()
    {
        Application.Quit();
    }
    public void PressBackSpace()
    {
        if (_inputField.text.Length > 0)
            _inputField.text = _inputField.text.Remove(_inputField.text.Length - 1);
    }
    public void PressShift()
    {
        _shiftPressed = !_shiftPressed; 
        SwitchQWERegisrt();
    }
    public void SwitchNumberMode()
    {
        _numbersParent.SetActive(!_numbersParent.activeInHierarchy);
        _qweParents[0].SetActive(!_qweParents[0].activeInHierarchy);

    }
    public void SwitchQWERegisrt()                       // if upper => Upper register all qwe else Lower register all qwe
    {
        if (!_qweUpperMode)
            foreach (var _qwe in _qweButtonTexts)
                _qwe.text = _qwe.text.ToUpper();
        else
            foreach (var _qwe in _qweButtonTexts)
                _qwe.text = _qwe.text.ToLower();

        _qweUpperMode = !_qweUpperMode;
    }
    private void PrintKey(Text _keyText)
    {
        string _key = _keyText.text;
        
        if (_key.Length == 1)
        {
            _inputField.text += _key;

            if (_shiftPressed)
            {
                SwitchQWERegisrt();
                _shiftPressed = false;
            }
        }
        /// if you want realese press event special keys with code;
        // else
        // {
        //     switch (_key)
        //     {
        //         case "123": SwitchNumberMode();
        //             break;
        //         case "caps": SwitchQWERegisrt();
        //             break;
        //         case "shift": PressShift();
        //             break;
        //         case "back": PressBackSpace();
        //             break;
        //         case "close": PressClose();
        //             break;
        //         default: Debug.Log("Error code keyb_0:" + _key);
        //             break;
        //     }
        // }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _savedText = PlayerPrefs.GetString("InputFieldText", "");
        _inputField.text = _savedText;

        _tmpButtons = _keyboardGameObject.GetComponentsInChildren<Button>();   /// for only select buttons
        
        foreach (var _btn in _tmpButtons)
        {
            _btn.onClick.AddListener(() => PrintKey(_btn.GetComponentInChildren<Text>()));
        }

        foreach (var _qwe in _qweParents)
        {
            _qweButtonTexts.AddRange(_qwe.GetComponentsInChildren<Text>());
        }

        _numbersParent.SetActive(false);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetString("InputFieldText", _inputField.text);
        PlayerPrefs.Save();
    }
}
