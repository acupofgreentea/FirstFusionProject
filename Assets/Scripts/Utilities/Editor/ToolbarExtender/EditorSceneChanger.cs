using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class EditorSceneChanger
{
    private static readonly string mainMenuScenePath = "Assets/Scenes/MainMenu.unity";
    private static readonly string sampleScenePath = "Assets/Scenes/GameScene.unity";
    
    static EditorSceneChanger()
	{
		UnityToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
	}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(new GUIContent("MMS", "Opens MainMenuScene"), ToolbarStyles.commandButtonStyle))
			{
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(mainMenuScenePath);
			}

			if(GUILayout.Button(new GUIContent("GS", "Opens GameScene"), ToolbarStyles.commandButtonStyle))
			{
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(sampleScenePath);
			}
		}
}

static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 12,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}
