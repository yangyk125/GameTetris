using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{
    [Serializable]
    public struct GameLevelConfig
    {
        public string name;
        public float speed;
        public int score;
    }

    public class GameConfigs : MonoBehaviour
    {
        private static GameConfigs Instance;
        public static GameConfigs Get()
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

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }


        [SerializeField]
        [Tooltip("��Ϸ�����ڶ��ٳ��ȶ�����Ļ�ĸ߶�")]
        private float screenHeightAlignedSizeIn3D = 21f;
        public float GetScreenHeightAlignedSizeIn3D()
        {
            return screenHeightAlignedSizeIn3D;
        }

        [SerializeField]
        [Tooltip("��Ϸש�����������Ļ�ĳߴ�")]
        private Vector2 gameTilesPlaneSizeIn3D = new Vector2(10, 20);
        public Vector2 GetGameTilesPlaneSizeIn3D()
        {
            return gameTilesPlaneSizeIn3D;
        }

        [SerializeField]
        [Tooltip("��Ϸש�������п�ĸ���")]
        private Vector2Int gameTilesPlaneSectionsIn3D = new Vector2Int(10, 20);
        public Vector2Int GetGameTilesPlaneSectionsIn3D()
        {
            return gameTilesPlaneSectionsIn3D;
        }

        [SerializeField]
        [Tooltip("��һ��ͼ��ש�����������Ļ�ĳߴ�")]
        private Vector2 nextTilesPlaneSizeIn3D = new Vector2(4, 4);
        public Vector2 GetNextTilesPlaneSizeIn3D()
        {
            return nextTilesPlaneSizeIn3D;
        }

        [SerializeField]
        [Tooltip("��һ��ͼ��Ԥ�����п����")]
        private Vector2Int nextTilesPlaneSectionsIn3D = new Vector2Int(4, 4);
        public Vector2Int GetNextTilesPlaneSectionsIn3D()
        {
            return nextTilesPlaneSectionsIn3D;
        }

        [SerializeField]
        [Tooltip("Ԥ��Ŀ��л��ı���ͼ")]
        private List<GameObject> backgroundImages = new List<GameObject>();
        public List<GameObject> GetBackgroundImages()
        {
            return backgroundImages;
        }

        [SerializeField]
        [Tooltip("Ԥ��ķ���ͼ")]
        private List<GameObject> tileImages = new List<GameObject>();
        public List<GameObject> GetTileImages()
        {
            return tileImages;
        }

        [SerializeField]
        [Tooltip("������Ϸ�Ѷ���Ϣ")]
        private List<GameLevelConfig> gameLevels = new List<GameLevelConfig>();
        public List<GameLevelConfig> GetGameLevels()
        {
            return gameLevels;
        }
    }

}
