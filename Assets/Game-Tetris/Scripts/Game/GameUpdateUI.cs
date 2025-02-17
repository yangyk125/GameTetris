using GameTetris;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUpdateUI : MonoBehaviour
{
    private static GameUpdateUI Instance;
    public static GameUpdateUI Get()
    {
        return Instance;
    }

    public Button ButtonPause;
    public Button ButtonRestart;

    public ButtonEx ButtonMoveUp;
    public ButtonEx ButtonMoveLeft;
    public ButtonEx ButtonMoveBottom;
    public ButtonEx ButtonMoveRight;

    public Button ButtonRotateCW;
    public Button ButtonRotateCCW;

    public Button ButtonFallBottom;

    private void OnEnable()
    {
        Instance = this;

        ButtonPause.onClick.AddListener(OnButtonPlayPause);
        ButtonRestart.onClick.AddListener(OnButtonRestart);

        ButtonMoveUp.onRepeatClick.AddListener(OnButtonMoveUp);
        ButtonMoveLeft.onRepeatClick.AddListener(OnButtonMoveLeft);
        ButtonMoveBottom.onRepeatClick.AddListener(OnButtonMoveBottom);
        ButtonMoveRight.onRepeatClick.AddListener(OnButtonMoveRight);

        ButtonRotateCW.onClick.AddListener(OnButtonRotateCW);
        ButtonRotateCCW.onClick.AddListener(OnButtonRotateCCW);

        ButtonFallBottom.onClick.AddListener(OnButtonFallBottom);
    }

    private void OnDisable()
    {
        ButtonPause.onClick.RemoveAllListeners();
        ButtonRestart.onClick.RemoveAllListeners();

        ButtonMoveUp.onRepeatClick.RemoveAllListeners();
        ButtonMoveLeft.onRepeatClick.RemoveAllListeners();
        ButtonMoveBottom.onRepeatClick.RemoveAllListeners();
        ButtonMoveRight.onRepeatClick.RemoveAllListeners();

        ButtonRotateCW.onClick.RemoveAllListeners();
        ButtonRotateCCW.onClick.RemoveAllListeners();

        ButtonFallBottom.onClick.RemoveAllListeners();

        Instance = null;
    }


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnButtonPlayPause()
    {
        GameManager.Get().DoPlayPause();
    }

    private void OnButtonRestart()
    {
        GameManager.Get().DoRestartGame();
    }

    private void OnButtonMoveUp()
    {
        GameManager.Get().DoInputActionUp();
    }

    private void OnButtonMoveLeft()
    {
        GameManager.Get().DoInputActionLeft();
    }

    private void OnButtonMoveBottom()
    {
        GameManager.Get().DoInputActionDown();
    }

    private void OnButtonMoveRight()
    {
        GameManager.Get().DoInputActionRight();
    }

    private void OnButtonRotateCW()
    {
        GameManager.Get().DoInputActionRotateCW();
    }

    private void OnButtonRotateCCW()
    {
        GameManager.Get().DoInputActionRotateCCW();
    }

    private void OnButtonFallBottom()
    {
        GameManager.Get().DoInputActionFallToBottom();
    }
}
