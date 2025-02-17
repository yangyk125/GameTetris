using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace GameTetris
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager Instance;
        public static GameManager Get()
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

        public AudioSource AudioOutput;
        public AudioClip[] AudioClips;

        public GameObject ObjectPoolRoot;
        public GameObject BackgroundRoot;

        public GameObject TileImagesBack;
        public GameObject TileImagesRoot;
        

        public GameObject NextImagesBack;
        public GameObject NextImagesRoot;

        public Camera MainCamera;

        public TextMeshPro LevelText;
        public TextMeshPro ScoreText;

        ////////////////////////////////////////////////////////
        private bool _gameIsPaused = false;
        
        private int _backImageIndex = 0;
        private int _tileImageIndex = 0;

        private GameLevelConfig _gameConfig;

        //game tiles
        private int[][] _gameTileMaskArray = null;
        private GameObject[][] _gameTileAnchorArray = null;

        //next tiles
        private int[][] _nextTileMaskArray = null;
        private GameObject[][] _nextTileAnchorArray = null;

        private GameProcessor _gameProcessor = null;
        private GameObjectPool _gameTilePool = null;

        private Vector2 _userInputTest;

        // Start is called before the first frame update
        private void Start()
        {
            InitializeBackground();
            InitializeGameTilesBasic();
            InitializeNextTilesBasic();
            InitializeGameConfig();
            InitializeCamera();
            InitializeProcessor();
            InitializeTilePool();
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateGameInputTest();
            UpdateGameTilesFromMasks();
            UpdateNextTilesFromMasks();
            UpdateGameRunning();
        }

        private void FixedUpdate()
        {
            if (!_gameIsPaused)
            {
                bool gameOver = _gameProcessor.FixedUpdate();
            }
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

        private void InitializeGameTilesBasic()
        {
            //set size
            Vector2 PlaneSize = GameConfigs.Get().GetGameTilesPlaneSizeIn3D();
            TileImagesBack.transform.localScale = new Vector3(PlaneSize.x / 10, 1, PlaneSize.y / 10);

            //clean children
            GameUtility.CleanChildren(TileImagesRoot.transform);

            //init game tiles
            Vector2Int planeSecs = GameConfigs.Get().GetGameTilesPlaneSectionsIn3D();
            float sizePerH = + PlaneSize.x / planeSecs.x; //从左到右
            float sizePerV = - PlaneSize.y / planeSecs.y; //从上到下
            float startH = - PlaneSize.x / 2; //从左到右
            float startV = + PlaneSize.y / 2; //从上到下

            _gameTileMaskArray = new int[planeSecs.y][];
            _gameTileAnchorArray = new GameObject[planeSecs.y][];
            for (int idxV = 0; idxV < planeSecs.y; idxV++)
            {
                _gameTileMaskArray[idxV] = new int[planeSecs.x];
                _gameTileAnchorArray[idxV] = new GameObject[planeSecs.x];

                float posV = startV + idxV * sizePerV + sizePerV / 2;
                for (int idxH = 0; idxH < planeSecs.x; idxH++)
                {
                    float posH = startH + idxH * sizePerH + sizePerH / 2;

                    _gameTileMaskArray[idxV][idxH] = 0;
                    GameObject anchor =_gameTileAnchorArray[idxV][idxH] = new GameObject();
                    GameUtility.SetTransformIdentity(anchor.transform);
                    anchor.name = string.Format("Anchor_{0,2:00}_{1,2:00}", idxV, idxH);
                    anchor.transform.parent = TileImagesRoot.transform;
                    anchor.transform.localPosition = new Vector3(posH, posV, 0);
                }
            }
        }

        private void InitializeNextTilesBasic()
        {
            //set size
            Vector2 PlaneSize = GameConfigs.Get().GetNextTilesPlaneSizeIn3D();
            NextImagesBack.transform.localScale = new Vector3(PlaneSize.x / 10, 1, PlaneSize.y / 10);

            //clean children
            GameUtility.CleanChildren(NextImagesRoot.transform);

            //init next tiles
            Vector2Int planeSecs = GameConfigs.Get().GetNextTilesPlaneSectionsIn3D();
            float sizePerH = +PlaneSize.x / planeSecs.x; //从左到右
            float sizePerV = -PlaneSize.y / planeSecs.y; //从上到下
            float startH = -PlaneSize.x / 2; //从左到右
            float startV = +PlaneSize.y / 2; //从上到下

            _nextTileMaskArray = new int[planeSecs.y][];
            _nextTileAnchorArray = new GameObject[planeSecs.y][];
            for (int idxV = 0; idxV < planeSecs.y; idxV++)
            {
                _nextTileMaskArray[idxV] = new int[planeSecs.x];
                _nextTileAnchorArray[idxV] = new GameObject[planeSecs.x];

                float posV = startV + idxV * sizePerV + sizePerV / 2;
                for (int idxH = 0; idxH < planeSecs.x; idxH++)
                {
                    float posH = startH + idxH * sizePerH + sizePerH / 2;

                    _nextTileMaskArray[idxV][idxH] = 0;
                    GameObject anchor = _nextTileAnchorArray[idxV][idxH] = new GameObject();
                    GameUtility.SetTransformIdentity(anchor.transform);
                    anchor.name = string.Format("Anchor_{0,2:00}_{1,2:00}", idxV, idxH);
                    anchor.transform.parent = NextImagesRoot.transform;
                    anchor.transform.localPosition = new Vector3(posH, posV, 0);
                }
            }

        }

        private void InitializeGameConfig()
        {
            _gameConfig = GameConfigs.Get().GetGameLevels()[0];
        }

        private void InitializeCamera()
        {
            //调整相机，宽度高度
            float aligned = GameConfigs.Get().GetScreenHeightAlignedSizeIn3D();
            MainCamera.orthographicSize = aligned / 2.0f;
        }

        private void InitializeProcessor()
        {
            Vector2Int gameSecs = GameConfigs.Get().GetGameTilesPlaneSectionsIn3D();
            Vector2Int nextSecs = GameConfigs.Get().GetNextTilesPlaneSectionsIn3D();
            _gameProcessor = new GameProcessor(_gameTileMaskArray, gameSecs, _nextTileMaskArray, nextSecs);
            _gameProcessor.RestartGame(_gameConfig.speed);
        }

        private void InitializeTilePool()
        {
            List<GameObject> tileImages = GameConfigs.Get().GetTileImages();
            GameObject tilePrefab = tileImages[_tileImageIndex];
            _gameTilePool = new GameObjectPool(ObjectPoolRoot, tilePrefab);
        }

        private void UpdateGameInputTest()
        {
            _userInputTest.x = Input.GetAxis("Horizontal");
            _userInputTest.y = Input.GetAxis("Vertical");

            if (Input.GetButtonUp("Vertical"))
            {
                if (_userInputTest.y > 0)
                    _gameProcessor.DoInputActionRotateCCW();
                else if (_userInputTest.y < 0)
                    _gameProcessor.DoInputActionDown();
            }

            if (Input.GetButtonUp("Horizontal"))
            {
                if (_userInputTest.x > 0)
                    _gameProcessor.DoInputActionRight();
                else if (_userInputTest.x < 0)
                    _gameProcessor.DoInputActionLeft();
            }

            if (Input.GetButtonUp("Jump"))
            {
                _gameProcessor.DoInputActionFallToBottom();
            }
        }

        private void UpdateGameTilesFromMasks()
        {
            Vector2Int planeSecs = GameConfigs.Get().GetGameTilesPlaneSectionsIn3D();

            List<GameObject> tileImages = GameConfigs.Get().GetTileImages();
            GameObject tilePrefab = tileImages[_tileImageIndex];

            for (int idxV = 0; idxV < planeSecs.y; idxV++)
            {
                for (int idxH = 0; idxH < planeSecs.x; idxH++)
                {
                    if (_gameTileMaskArray[idxV][idxH] == 0 && _gameTileAnchorArray[idxV][idxH].transform.childCount != 0)
                    {
                        Transform child = _gameTileAnchorArray[idxV][idxH].transform.GetChild(0);
                        _gameTilePool.Release(child.gameObject);
                    }
                    else if (_gameTileMaskArray[idxV][idxH] != 0 && _gameTileAnchorArray[idxV][idxH].transform.childCount == 0)
                    {
                        GameObject newTile = _gameTilePool.Acquire();
                        newTile.transform.parent = _gameTileAnchorArray[idxV][idxH].transform;
                        GameUtility.SetTransformIdentity(newTile.transform);
                    }
                }
            }
        }

        private void UpdateNextTilesFromMasks()
        {
            Vector2Int planeSecs = GameConfigs.Get().GetNextTilesPlaneSectionsIn3D();

            List<GameObject> tileImages = GameConfigs.Get().GetTileImages();
            GameObject tilePrefab = tileImages[_tileImageIndex];

            for (int idxV = 0; idxV < planeSecs.y; idxV++)
            {
                for (int idxH = 0; idxH < planeSecs.x; idxH++)
                {
                    if (_nextTileMaskArray[idxV][idxH] == 0 && _nextTileAnchorArray[idxV][idxH].transform.childCount != 0)
                    {
                        Transform child = _nextTileAnchorArray[idxV][idxH].transform.GetChild(0);
                        _gameTilePool.Release(child.gameObject);
                    }
                    else if (_nextTileMaskArray[idxV][idxH] != 0 && _nextTileAnchorArray[idxV][idxH].transform.childCount == 0)
                    {
                        GameObject newTile = _gameTilePool.Acquire();
                        newTile.transform.parent = _nextTileAnchorArray[idxV][idxH].transform;
                        GameUtility.SetTransformIdentity(newTile.transform);
                    }
                }
            }
        }

        private void UpdateGameRunning()
        {
            int gameScore = _gameProcessor.GetGameScore();

            ScoreText.text = gameScore.ToString();
            LevelText.text = _gameConfig.name;

            List<GameLevelConfig> levels = GameConfigs.Get().GetGameLevels();
            for (int idx=0; idx< levels.Count; idx++)
            {
                GameLevelConfig config = levels[idx];
                if (gameScore >= config.score)
                {
                    _gameConfig = config;
                }
            }
        }

        public void DoPlayPause()
        {
            _gameIsPaused = !_gameIsPaused;
        }

        public void DoRestartGame()
        {
            _gameConfig = GameConfigs.Get().GetGameLevels()[0];
            _gameProcessor.RestartGame(_gameConfig.speed);
        }

        public void DoInputActionUp()
        {
            AudioOutput.clip = AudioClips[0];
            AudioOutput.Play();

            _gameProcessor.DoInputActionUp();
        }

        public void DoInputActionDown()
        {
            AudioOutput.clip = AudioClips[0];
            AudioOutput.Play();

            _gameProcessor.DoInputActionDown();
        }

        public void DoInputActionLeft()
        {
            AudioOutput.clip = AudioClips[0];
            AudioOutput.Play();

            _gameProcessor.DoInputActionLeft();
        }

        public void DoInputActionRight()
        {
            AudioOutput.clip = AudioClips[0];
            AudioOutput.Play();

            _gameProcessor.DoInputActionRight();
        }

        public void DoInputActionRotateCCW()
        {
            AudioOutput.clip = AudioClips[1];
            AudioOutput.Play();

            _gameProcessor.DoInputActionRotateCCW();
        }

        public void DoInputActionRotateCW()
        {
            AudioOutput.clip = AudioClips[1];
            AudioOutput.Play();

            _gameProcessor.DoInputActionRotateCW();
        }

        public void DoInputActionFallToBottom()
        {
            AudioOutput.clip = AudioClips[2];
            AudioOutput.Play();

            _gameProcessor.DoInputActionFallToBottom();
        }
    }
}
