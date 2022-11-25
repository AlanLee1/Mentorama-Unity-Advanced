using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), id: ID_SCENE_VIEWER_OVERLAY, displayName: "Controle de Cenas")]
[Icon("Assets/Editor/SceneViewer/Icons/unity_scene.png")]
public class SceneViewerEditor : Overlay
{
    private const string ID_SCENE_VIEWER_OVERLAY = "sceneViewerOverlay";
    private VisualElement root;

    public override VisualElement CreatePanelContent()
    {
        root = new VisualElement
        {
            style =
            {
                width = new StyleLength(new Length(120, LengthUnit.Pixel)),
                backgroundColor = new StyleColor(Color.black),
                opacity = new StyleFloat(0.85f),
                fontSize = 14
            }
        };

        CreateSceneButtons();

        return root;
    }

    public override void OnCreated()
    {
        EditorBuildSettings.sceneListChanged += CreateSceneButtons;
    }

    public override void OnWillBeDestroyed()
    {
        base.OnWillBeDestroyed();
        EditorBuildSettings.sceneListChanged -= CreateSceneButtons;
    }

    private void CreateSceneButtons()
    {
        root.Clear();

        if (EditorSceneManager.sceneCountInBuildSettings == 0)
        {
            var warningText = new TextElement();
            warningText.text = "Nenhuma cena no Build Settings";
            warningText.style.fontSize = 12;
            warningText.style.color = new StyleColor(Color.red);

            root.Add(warningText);
            return;
        }

        for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
        {
            int tempIndex = i;

            var sceneButton = new Button(() => ButtonCallback(tempIndex));

            string fileName = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(tempIndex));

            //Remove a extensão .unity do nome da cena
            sceneButton.text = fileName.Substring(0, fileName.Length - 6);

            root.Add(sceneButton);
        }
    }

    private void ButtonCallback(int index)
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            int dialogResult = EditorUtility.DisplayDialogComplex(
                "A cena foi modificada",
                "Deseja salvar as alterações feitas na cena atual?",
                "Salvar", "Não Salvar", "Cancelar");

            switch (dialogResult)
            {
                case 0: //Salva e abre a nova cena
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
                    break;
                case 1: //Abre a nova cena sem salvar
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
                    break;
                case 2: //Cancela a opção de troca de cena
                    break;
                default:
                    Debug.LogWarning("Algo deu errado ao trocar de cena.");
                    break;
            }
        } else
        {
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(index));
        }
    }
}