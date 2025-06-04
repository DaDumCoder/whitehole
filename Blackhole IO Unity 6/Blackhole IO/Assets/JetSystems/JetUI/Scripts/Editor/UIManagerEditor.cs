#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace JetSystems
{
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor
    {
        UIManager uiManager;
        CanvasScaler scaler;

 

        int orientationPreference;

        private void OnEnable()
        {
            if (uiManager == null)
            {
                // Store the ui Manager
                uiManager = (UIManager)target;

                // Store the canvas Scaler
                scaler = uiManager.GetComponent<CanvasScaler>();

                // Hide the unnecessary components
                HideUselessComponents();

                orientationPreference = EditorPrefs.GetInt("Orientation");
                if(orientationPreference == 0)
                {
                    // Set the orientation for the firs time
                    UpdateOrientation();
                    EditorPrefs.SetInt("Orientation", 1);
                    EditorUtility.SetDirty(uiManager);
                }
            }

        }

        private void HideUselessComponents()
        {
            // Return if no ui manager found ( impossible )
            if (uiManager == null) return;

            uiManager.GetComponent<RectTransform>().hideFlags = HideFlags.HideInInspector;
            uiManager.GetComponent<Canvas>().hideFlags = HideFlags.HideInInspector;
            uiManager.GetComponent<GraphicRaycaster>().hideFlags = HideFlags.HideInInspector;
            uiManager.GetComponent<CanvasScaler>().hideFlags = HideFlags.HideInInspector;

        }


        public override void OnInspectorGUI()
        {
            // Show the orientation settings
            ShowOrientationSettings();

            // Show the canvases
            ShowCanvases();

            // Show the menu ui
            ShowMenuUI();

            // Show Game UI
            ShowGameUI();

            // Show Shop UI
            ShowShopUI();

            // Show Level Complete UI
            ShowLevelCompleteUI();

            if(GUI.changed)
            {
                UpdateOrientation();
                EditorUtility.SetDirty(uiManager);
                serializedObject.ApplyModifiedProperties();
            }
        }

        #region Orientation Settings

        private void ShowOrientationSettings()
        {
            Utilsjet.CategoryHeader("Orientation");

            uiManager.orientation = (UIManager.Orientation)EditorGUILayout.EnumPopup("Orientation", uiManager.orientation);
        }

        private void UpdateOrientation()
        {
            switch(uiManager.orientation)
            {
                case UIManager.Orientation.Portrait:
                    SetPortrait();
                    break;

                case UIManager.Orientation.Landscape:
                    SetLandscape();
                    break;
            }
        }

        private void SetPortrait()
        {
            // If no scaler was found, return
            if (scaler == null) return;

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 1;

            // Save that in the preferences
            EditorPrefs.SetInt("Orientation", 1);
        }

        private void SetLandscape()
        {
            // If no scaler was found, return
            if (scaler == null) return;

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0;

            // Save that in the preferences
            EditorPrefs.SetInt("Orientation", 2);
        }

        #endregion

        private void ShowCanvases()
        {
            Utilsjet.CategoryHeader("Canvases");

            Utilsjet.ShowSerializedField(serializedObject, "MENU", "Menu");
            Utilsjet.ShowSerializedField(serializedObject, "GAME", "Game");
            Utilsjet.ShowSerializedField(serializedObject, "LEVELCOMPLETE", "Level Complete");
            Utilsjet.ShowSerializedField(serializedObject, "GAMEOVER", "Gameover");
            Utilsjet.ShowSerializedField(serializedObject, "SETTINGS", "Settings");
            Utilsjet.ShowSerializedField(serializedObject, "shopManager", "Shop Manager");

        }

        private void ShowMenuUI()
        {
            Utilsjet.CategoryHeader("Menu UI");

            // Show the needed data
            Utilsjet.ShowSerializedField(serializedObject, "menuCoinsText");
        }

        private void ShowGameUI()
        {
            Utilsjet.CategoryHeader("Game UI");

            Utilsjet.ShowSerializedField(serializedObject, "progressBar");
            Utilsjet.ShowSerializedField(serializedObject, "gameCoinsText");
            Utilsjet.ShowSerializedField(serializedObject, "levelText");
        }

        private void ShowShopUI()
        {
            Utilsjet.CategoryHeader("Shop");

            Utilsjet.ShowSerializedField(serializedObject, "shopCoinsText");
        }

        private void ShowLevelCompleteUI()
        {
            
            Utilsjet.CategoryHeader("Level Complete");
            Utilsjet.ShowSerializedField(serializedObject, "levelCompleteCoinsText");
        }
    }
}
#endif