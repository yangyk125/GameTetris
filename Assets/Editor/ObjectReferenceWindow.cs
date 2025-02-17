using UnityEditor;
using UnityEngine;

using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor
{
    public enum ObjectReferenceSearchType
    {
        ObjectReferSelection = 1,
        SelectionReferObject = 2
    }

    public enum ObjectReferenceSearchStatus
    {
        SearchIdle = 1,
        SearchStart = 2,
        SearchRunning = 3,
        SearchStop = 4
    }

    public class ObjectReferenceWindow : EditorWindow, SearchObjectCallback
    {
        private static ObjectReferenceWindow ReferenceWindow = null;

        public static void ShowWindow(ObjectReferenceSearchType SearchType, UnityEngine.Object SearchObject)
        {
            if (ReferenceWindow == null)
                ReferenceWindow = GetWindow<ObjectReferenceWindow>("Object Referring");
            ReferenceWindow.Show();
            ReferenceWindow.SearchType = SearchType;
            ReferenceWindow.SearchObject = SearchObject;
        }

        public static void HideWindow()
        {
            if (ReferenceWindow == null)
                return;
            ReferenceWindow.Close();
        }

        

        private ObjectReferenceSearchType SearchType = ObjectReferenceSearchType.SelectionReferObject;
        private ObjectReferenceSearchStatus SearchStatus = ObjectReferenceSearchStatus.SearchIdle;
        private UnityEngine.Object SearchObject = null;

        private GUIContent FilterLabel = new GUIContent("Filter Classes:", "eg. Material,Texture,GameObject,SceneAsset");
        private string FilterString = "Object";

        private SearchObjectBase SearchJob = null;
        private ArrayList SearchResult = new ArrayList();
        private Vector2Int SearchProgress = new Vector2Int();

        private bool CheckCanSearch()
        {
            return SearchObject != null && SearchJob == null;
        }
        private bool CheckCanStop()
        {
            return SearchJob != null;
        }

        private Vector2 ScrollPosition;

        private void OnDestroy()
        {
            OnSearchStop();
        }

        void OnGUI()
        {
            SearchType = (ObjectReferenceSearchType)EditorGUILayout.EnumPopup("Select an option:", SearchType);
            GUILayout.Space(2);

            EditorGUILayout.ObjectField("Select an object:", SearchObject, typeof(UnityEngine.Object), true);
            GUILayout.Space(2);

            FilterString = EditorGUILayout.TextField(FilterLabel, FilterString);
            GUILayout.Space(2);

            EditorGUILayout.BeginHorizontal();
            {
                GUI.enabled = CheckCanSearch();
                if (GUILayout.Button("Search"))
                {
                    OnSearchStart();
                }

                GUI.enabled = CheckCanStop();
                if (GUILayout.Button("Stop"))
                {
                    OnSearchStop();
                }

                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.LabelField(System.String.Format("Search Progress: {0}/{1}", SearchProgress.x, SearchProgress.y));
            EditorGUILayout.LabelField("Search Result:");
            ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition, GUILayout.Width(440), GUILayout.Height(500));
            foreach (Object obj in SearchResult)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField("Find object:", obj, typeof(UnityEngine.Object), true);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        void Update()
        {
            switch (SearchStatus)
            {
                case ObjectReferenceSearchStatus.SearchStart:
                    OnSearchStart();
                    break;
                case ObjectReferenceSearchStatus.SearchRunning:
                    OnSearchRunning();
                    break;
                case ObjectReferenceSearchStatus.SearchStop:
                    OnSearchStop();
                    break;
                case ObjectReferenceSearchStatus.SearchIdle:
                default:
                    break;
            }
        }

        public void OnSearchObject(UnityEngine.Object obj)
        {
            SearchResult.Add(obj);
        }

        public void OnSearchProgress(Vector2Int step)
        {
            SearchProgress.Set(step.x, step.y);
        }

        void OnSearchStart()
        {
            if (SearchObject == null)
                return;

            if (SearchJob != null)
                return;

            SearchResult.Clear();
            SearchProgress.Set(0, 0);

            if (SearchType == ObjectReferenceSearchType.ObjectReferSelection)
                SearchJob = new SearchObjectReferSelection(SearchObject, FilterString, this);
            else if (SearchType == ObjectReferenceSearchType.SelectionReferObject)
                SearchJob = new SearchSelectionReferObject(SearchObject, FilterString, this);

            SearchJob.OnSearchStart();

            SearchStatus = ObjectReferenceSearchStatus.SearchRunning;
        }

        void OnSearchRunning()
        {
            if (SearchJob == null)
                return;

            if (!SearchJob.OnSearchRunning())
            {
                SearchStatus = ObjectReferenceSearchStatus.SearchStop;
            }

            Repaint();
        }

        void OnSearchStop()
        {
            if (SearchJob == null)
                return;

            SearchJob.OnSearchStop();
            SearchJob = null;

            SearchStatus = ObjectReferenceSearchStatus.SearchIdle;

            Repaint();
        }
    }

    abstract class SearchObjectBase
    {
        public abstract void OnSearchStart();
        public abstract bool OnSearchRunning();
        public abstract void OnSearchStop();

        protected System.Type FindEngineType(string clazz)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly.FullName.StartsWith("UnityEngine"))
                {
                    System.Type find = assembly.GetType("UnityEngine." + clazz);
                    if (find != null)
                    {
                        return find;
                    }
                }

                if (assembly.FullName.StartsWith("UnityEditor"))
                {
                    System.Type find = assembly.GetType("UnityEditor." + clazz);
                    if (find != null)
                    {
                        return find;
                    }
                }
            }

            return null;
        }
    }

    interface SearchObjectCallback
    {
        public void OnSearchObject(UnityEngine.Object obj);
        public void OnSearchProgress(Vector2Int step);
    }

    class SearchObjectReferSelection : SearchObjectBase
    {
        private UnityEngine.Object SearchObject = null;
        private ArrayList SearchClasses = new ArrayList();    
        private SearchObjectCallback SearchCallback = null;

        private string[] CachedGuids = null;
        private int CachedIndex = -1;
        private Vector2Int CachedStep = new Vector2Int();

        public SearchObjectReferSelection(UnityEngine.Object obj, string filters, SearchObjectCallback callback)
        {
            SearchObject = obj;
            SearchCallback = callback;

            if (filters != null && filters.Length > 0)
            {
                string[] classes = filters.Split(',');
                foreach (string clazz in classes)
                {
                    System.Type type = FindEngineType(clazz);
                    if (type != null)
                        SearchClasses.Add(type);
                }
            }

            if (SearchClasses.Count == 0)
            {
                SearchClasses.Add(typeof(UnityEngine.Object));
            }
        }

        public override void OnSearchStart()
        {
            CachedGuids = AssetDatabase.FindAssets("", new string[] { "Assets" });
            CachedIndex = 0;
        }

        public override bool OnSearchRunning()
        {
            if (CachedGuids != null && CachedIndex >= 0 && CachedIndex < CachedGuids.Length)
            {
                string guid = CachedGuids[CachedIndex];
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

                if (obj != null && obj != SearchObject)
                {
                    bool matched = false;
                    foreach (System.Type type in SearchClasses)
                    {
                        if (type.IsInstanceOfType(obj))
                        {
                            matched = true;
                            break;
                        }
                    }

                    if (matched && CheckObjectReferedSelection(obj, SearchObject))
                        SearchCallback.OnSearchObject(obj);
                }

                CachedIndex++;
                CachedStep.Set(CachedIndex, CachedGuids.Length);
                SearchCallback.OnSearchProgress(CachedStep);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnSearchStop()
        {
            CachedGuids = null;
            CachedIndex = -1;
        }

        private bool CheckObjectReferedSelection(Object ToCheck, Object ToSearch)
        {
            Object[] roots = new Object[] { ToCheck };
            Object[] finds = EditorUtility.CollectDependencies(roots);

            foreach (Object obj in finds)
            {
                if (obj == ToCheck)
                    continue;
                if (obj == ToSearch)
                    return true;
            }

            return false;
        }
    }

    class SearchSelectionReferObject : SearchObjectBase
    {
        private UnityEngine.Object SearchObject = null;
        private ArrayList SearchClasses = new ArrayList();
        private SearchObjectCallback SearchCallback = null;

        private Object[] CachedObjects = null;
        private int CachedIndex = -1;
        private Vector2Int CachedStep = new Vector2Int();

        public SearchSelectionReferObject(UnityEngine.Object obj, string filters, SearchObjectCallback callback)
        {
            SearchObject = obj;
            SearchCallback = callback;

            if (filters != null && filters.Length > 0)
            {
                string[] classes = filters.Split(',');
                foreach (string clazz in classes)
                {
                    System.Type type = FindEngineType(clazz);
                    if (type != null)
                        SearchClasses.Add(type);
                }
            }

            if (SearchClasses.Count == 0)
            {
                SearchClasses.Add(typeof(UnityEngine.Object));
            }
        }

        public override void OnSearchStart()
        {
            Object[] roots = new Object[] { SearchObject };

            CachedObjects = EditorUtility.CollectDependencies(roots);
            CachedIndex = 0;
        }

        public override bool OnSearchRunning()
        {
            if (CachedObjects != null && CachedIndex >= 0 && CachedIndex < CachedObjects.Length)
            {
                Object obj = CachedObjects[CachedIndex];
                if (obj != SearchObject)
                {
                    bool matched = false;
                    foreach (System.Type type in SearchClasses)
                    {
                        if (type.IsInstanceOfType(obj))
                        {
                            matched = true;
                            break;
                        }
                    }

                    if (matched)
                    {
                        SearchCallback.OnSearchObject(obj);
                    }
                }

                CachedIndex++;
                CachedStep.Set(CachedIndex, CachedObjects.Length);
                SearchCallback.OnSearchProgress(CachedStep);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnSearchStop()
        {
            CachedObjects = null;
            CachedIndex = -1;
        }
    }
}

