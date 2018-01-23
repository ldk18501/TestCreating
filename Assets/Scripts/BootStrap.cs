using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DoozyUI;
using System.Collections.Generic;

namespace smallone
{
    public class BootStrap : MonoBehaviour
    {
		#if UNITY_EDITOR
        public string mLanguage;
		#endif
        public string mDefaultLanguage = "Chinese";//refer to Unity API "SystemLanguage" for more language options
			
        void Awake()
        {
			#region multiTouch
			//Input.multiTouchEnabled = false;
			#endregion

            #region set traget frame rate
            Application.targetFrameRate = 60;
            #endregion

            #region set log filter
            LogUtil.SetFilters(LogMask.NONE, null);
            #endregion

            #region set language
            string language = null;
			#if UNITY_EDITOR
            language = mLanguage;
			#else
            LogUtil.LogNoTag("System Language: {0}", Application.systemLanguage);
            language = Application.systemLanguage.ToString();
			if (language == "Chinese")
				language = "ChineseSimplified";
			#endif
            Localization.language = Localization.HasLanguage(language) ? language : mDefaultLanguage;
            Localization.LoadFonts(new string[] { "Arial" }, true);
            #endregion

            #region create ItemManager instance and load data
            BuildingManager.GetOrCreateInstance();
            #endregion

            #region init lean touch
            //create lean touch
            GameObject leanTouchObj = new GameObject("LeanTouch");
            leanTouchObj.AddComponent<Lean.Touch.LeanTouch>();
            leanTouchObj.AddComponent<DontDestroyOnLoad>();
            #endregion

            #region init EventCenter
            EventCenter.GetOrCreateInstance();
            DataCenter.GetOrCreateInstance();
            #endregion

            InitGame();
        }

        // Use this for initialization
        void Start()
        {
            LevelManager.Instance.ChangeLevel(LevelEnum.Main);
        }

        void InitGame()
        {
            // EffectCenter.GetOrCreateInstance();

            #region init 2d UI
            Canvas canvas2dUI = UIManager.GetUiContainer.GetComponent<Canvas>();
            CanvasScaler canvasScaler2dUI = UIManager.GetUiContainer.GetComponent<CanvasScaler>();
            //force UI to scale to match width only, different games may have different settings
            canvasScaler2dUI.matchWidthOrHeight = 0f;
            canvas2dUI.worldCamera.depth = Camera.main.depth + 10;
            #endregion

            GameData.Init();
        }
	}
}