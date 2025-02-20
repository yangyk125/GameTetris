using GameTetris;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameTetris
{
    public class GameJoystick : MonoBehaviour
    {
        public GameManager gameManager;

        private RepeatFilter _repeatFilterUp = new RepeatFilter(0.4F, 0.04F);
        private RepeatFilter _repeatFilterLeft = new RepeatFilter(0.4F, 0.04F);
        private RepeatFilter _repeatFilterDown = new RepeatFilter(0.4F, 0.04F);
        private RepeatFilter _repeatFilterRight = new RepeatFilter(0.4F, 0.04F);

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void FixedUpdate()
        {
            if (_repeatFilterUp.Repeat())
            {
                gameManager.DoInputActionUp();
            }

            if (_repeatFilterLeft.Repeat())
            {
                gameManager.DoInputActionLeft();
            }

            if (_repeatFilterDown.Repeat())
            {
                gameManager.DoInputActionDown();
            }

            if (_repeatFilterRight.Repeat())
            {
                gameManager.DoInputActionRight();
            }
        }

        public void OnGameRestart()
        {
            gameManager.DoRestartGame();
        }

        public void OnGamePause()
        {
            gameManager.DoPlayPause();
        }

        public void OnMoveUp(InputValue value)
        {
            if (value.Get() != null)
            {
                _repeatFilterUp.Press();
                gameManager.DoInputActionUp();
            }
            else
                _repeatFilterUp.Release();
        }

        public void OnMoveLeft(InputValue value)
        {
            if (value.Get() != null)
            {
                _repeatFilterLeft.Press();
                gameManager.DoInputActionLeft();
            }
            else
                _repeatFilterLeft.Release();
        }

        public void OnMoveDown(InputValue value)
        {
            if (value.Get() != null)
            {
                _repeatFilterDown.Press();
                gameManager.DoInputActionDown();
            }
            else
                _repeatFilterDown.Release();
        }

        public void OnMoveRight(InputValue value)
        {
            if (value.Get() != null)
            {
                _repeatFilterRight.Press();
                gameManager.DoInputActionRight();
            }
            else
                _repeatFilterRight.Release();
        }

        public void OnRotateCW()
        {
            gameManager.DoInputActionRotateCW();
        }

        public void OnRotateCCW()
        {
            gameManager.DoInputActionRotateCCW();
        }

        public void OnFallToEnd()
        {
            gameManager.DoInputActionFallToBottom();
        }
    }

}
