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
        [Tooltip("游戏场景内多少长度对齐屏幕的高度")]
        private float screenHeightAlignedSizeIn3D = 21f;
        public float GetScreenHeightAlignedSizeIn3D()
        {
            return screenHeightAlignedSizeIn3D;
        }

        [SerializeField]
        [Tooltip("游戏砖块区域对齐屏幕的尺寸")]
        private Vector2 gameTilesPlaneSizeIn3D = new Vector2(10, 20);
        public Vector2 GetGameTilesPlaneSizeIn3D()
        {
            return gameTilesPlaneSizeIn3D;
        }

        [SerializeField]
        [Tooltip("游戏砖块区域切块的个数")]
        private Vector2Int gameTilesPlaneSectionsIn3D = new Vector2Int(10, 20);
        public Vector2Int GetGameTilesPlaneSectionsIn3D()
        {
            return gameTilesPlaneSectionsIn3D;
        }

        [SerializeField]
        [Tooltip("下一个图形砖块区域对齐屏幕的尺寸")]
        private Vector2 nextTilesPlaneSizeIn3D = new Vector2(4, 4);
        public Vector2 GetNextTilesPlaneSizeIn3D()
        {
            return nextTilesPlaneSizeIn3D;
        }

        [SerializeField]
        [Tooltip("下一个图形预览区切块个数")]
        private Vector2Int nextTilesPlaneSectionsIn3D = new Vector2Int(4, 4);
        public Vector2Int GetNextTilesPlaneSectionsIn3D()
        {
            return nextTilesPlaneSectionsIn3D;
        }

        [SerializeField]
        [Tooltip("预设的可切换的背景图")]
        private List<GameObject> backgroundImages = new List<GameObject>();
        public List<GameObject> GetBackgroundImages()
        {
            return backgroundImages;
        }

        [SerializeField]
        [Tooltip("预设的方块图")]
        private List<GameObject> tileImages = new List<GameObject>();
        public List<GameObject> GetTileImages()
        {
            return tileImages;
        }

        [SerializeField]
        [Tooltip("配置游戏难度信息")]
        private List<GameLevelConfig> gameLevels = new List<GameLevelConfig>();
        public List<GameLevelConfig> GetGameLevels()
        {
            return gameLevels;
        }
    }

}
