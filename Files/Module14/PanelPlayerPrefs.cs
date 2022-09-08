using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PanelPlayerPrefs : EditorWindow
{
    private struct PlayerPref
    {
        public string Key;
        public object Value;
    }

    private static Vector2 scrollPosition = Vector2.zero;
    private static List<PlayerPref> PlayerPrefsList = new List<PlayerPref>();

    private static PlayerPref newPlayerPref;
    private static string newKey;
    private static string newValue;
    private static int selectGrid = 0;
    private static string[] selectValue = { "Int", "Float", "String" };

    //ABRIR UMA JANELA VAZIA
    [MenuItem("Tools/Player Preferences")]
    public static void OnInit()
    {
        PanelPlayerPrefs win = GetWindow<PanelPlayerPrefs>();
        win.titleContent = new GUIContent("Player Preferences");
        win.Show();
    }

    private void OnGUI()
    {
        Create();
        List();
    }

    private static void Create()
    {
        TextStyle();
        GUILayout.Label("Criar novo Player Prefs");
        GUILayout.Label(" ");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Chave", EditorStyles.boldLabel, GUILayout.Width(40));
        newKey = EditorGUILayout.TextField(newKey);
        GUILayout.Label("Valor", EditorStyles.boldLabel, GUILayout.Width(40));
        newValue = EditorGUILayout.TextField(newValue);
        GUILayout.Label("Tipo", EditorStyles.boldLabel, GUILayout.Width(30));
        selectGrid = EditorGUILayout.Popup(selectGrid, selectValue);
        //selectGrid = GUILayout.Toolbar(selectGrid, selectValue);
        GUILayout.EndVertical();


        if (GUILayout.Button("Criar"))
        {
            newPlayerPref.Key = newKey;
            switch (selectGrid)
            {
                case 0:
                    PlayerPrefs.SetInt(newKey, int.Parse(newValue));
                    newPlayerPref.Value = int.Parse(newValue);
                    break;
                case 1:
                    PlayerPrefs.SetFloat(newKey, float.Parse(newValue));
                    newPlayerPref.Value = float.Parse(newValue);
                    break;
                case 2:
                    PlayerPrefs.SetString(newKey, newValue);
                    newPlayerPref.Value = newValue;
                    break;
                default:
                    break;
            }

            int index = PlayerPrefsList.FindIndex(element => element.Key == newPlayerPref.Key);

            //Salvar
            if (index == -1)
            {
                Debug.Log("Não existe, foi salvo um novo");
                PlayerPrefsList.Add(newPlayerPref);
                PlayerPrefs.Save();

            } else
            {
                int option = EditorUtility.DisplayDialogComplex("Alteração no PlayerPref", "Deseja substituir o valor da chave?", "Sim", "Cancelar", "Não");

                switch (option)
                {
                    // Sim
                    case 0:
                        PlayerPrefsList[index] = newPlayerPref;
                        PlayerPrefs.Save();
                        break;
                    // Não e Cancelar
                    case 1:
                    case 2:
                        break;

                    default:
                        Debug.LogError("Unrecognized option.");
                        break;
                }

                Debug.Log("Já existe!");
            }
            //Limpar campos
            newKey = "";
            newValue = "";
        }
        GUILayout.Label(" ");
    }

    private static void List()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.Label("Lista");

        EditorGUILayout.BeginHorizontal("Box");
        GUILayout.Label("Chave", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Label("Valor", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("Tipo", EditorStyles.boldLabel, GUILayout.Width(50));
        GUILayout.Label("Deletar", EditorStyles.boldLabel, GUILayout.Width(45));
        EditorGUILayout.EndHorizontal();

        foreach (var player in PlayerPrefsList)
        {
            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.TextField(player.Key, GUILayout.Width(150));
            GUILayout.TextField(player.Value.ToString(), GUILayout.Width(200));
            GUILayout.TextField(ConvertType(player.Value.GetType().Name.ToString()), GUILayout.Width(50));
            if (GUILayout.Button("X", GUILayout.Width(45)))
            {
                PlayerPrefs.DeleteKey(player.Key);
                PlayerPrefsList.Remove(player);
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    private static string ConvertType(string type)
    {
        if (type == "Single")
        {
            return "Float";
        } else if (type == "Int32")
        {
            return "Int";
        }

        return type;
    }

    private static GUIStyle TextStyle()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 18;
        style.alignment = TextAnchor.UpperCenter;

        return style;
    }
}
