using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{
    public class StartupManager : MonoBehaviour
    {
        private static StartupManager Instance;
        public static StartupManager Get()
        {
            return Instance;
        }

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }


        public GameObject BackgroundRoot;

        public Camera MainCamera;

        ////////////////////////////////////////////////////////

        private int _backImageIndex = 0;

        // Start is called before the first frame update
        private void Start()
        {
            InitializeBackground();
            InitializeCamera();
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void InitializeBackground()
        {
            GameUtility.CleanChildren(BackgroundRoot.transform);

            List<GameObject> backImages = GameConfigs.Get().GetBackgroundImages();
            GameObject backPrefab = backImages[_backImageIndex];

            if (backPrefab != null)
            {
                GameObject newObj = Object.Instantiate<GameObject>(backPrefab);
                newObj.transform.parent = BackgroundRoot.transform;
                GameUtility.SetTransformIdentity(newObj.transform);
            }

        }

        private void InitializeCamera()
        {
            //调整相机，宽度高度
            float aligned = GameConfigs.Get().GetScreenHeightAlignedSizeIn3D();
            MainCamera.orthographicSize = aligned / 2.0f;
        }
    }
}
