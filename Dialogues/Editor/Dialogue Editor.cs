using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogues.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue currentDialogue;
        Vector2 scrollPosition;
        [NonSerialized] GUIStyle nodeStyleBasic;
        [NonSerialized] GUIStyle nodeStylePlayer;
        [NonSerialized] DialogueNode nodeToDrag;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode;
        [NonSerialized] DialogueNode deletingNode;
        [NonSerialized] DialogueNode linkingNode;
        [NonSerialized] bool isDraggingScrollView = false;
        [NonSerialized] Vector2 draggingScrollViewOffset;

        const int editorSize = 3000;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue == null) { return false; }
            else
            {
                ShowEditorWindow();
                return true;
            }
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            nodeStyleBasic = new GUIStyle();
            nodeStyleBasic.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyleBasic.padding = new RectOffset(12, 12, 12, 12);
            nodeStyleBasic.border = new RectOffset(12, 12, 12, 12);

            nodeStylePlayer = new GUIStyle();
            nodeStylePlayer.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
            nodeStylePlayer.padding = new RectOffset(12, 12, 12, 12);
            nodeStylePlayer.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            var dialogue = Selection.activeObject as Dialogue;
            if (dialogue != null)
            {
                currentDialogue = dialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (currentDialogue != null)
            {
                HandleMouseEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                Rect editorRect = GUILayoutUtility.GetRect(editorSize, editorSize);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, 60, 60);

                GUI.DrawTextureWithTexCoords(editorRect, backgroundTexture, texCoords);

                foreach (var node in currentDialogue.GetAllNodes())
                {
                    DrawBezierConnections(node);
                }
                foreach (var node in currentDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                EditorGUILayout.EndScrollView();

                if(creatingNode != null)
                {
                    currentDialogue.AddNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    currentDialogue.RemoveNode(deletingNode);
                    deletingNode = null;
                }
            }
            else
            {
                EditorGUILayout.LabelField("Nothing Selected");
            }

        }

        private void HandleMouseEvents()
        {
            if (Event.current.type == EventType.MouseDown && nodeToDrag == null)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                nodeToDrag = GetNodeAtMousePoint(mousePosition + scrollPosition);
                if (nodeToDrag != null)
                {
                    draggingOffset = nodeToDrag.GetRect().position - mousePosition;
                    Selection.activeObject = nodeToDrag;
                }
                else
                {
                    isDraggingScrollView = true;
                    draggingScrollViewOffset = mousePosition + scrollPosition;
                    Selection.activeObject = currentDialogue;
                }
            }

            else if (Event.current.type == EventType.MouseDrag && nodeToDrag != null)
            {
                Undo.RecordObject(currentDialogue, "Move Dialogue Node");
                nodeToDrag.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }

            else if (Event.current.type == EventType.MouseDrag && isDraggingScrollView)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                scrollPosition = draggingScrollViewOffset - mousePosition;

                GUI.changed = true;
            }

            else if (Event.current.type == EventType.MouseUp && nodeToDrag != null)
            {
                nodeToDrag = null;
            }

            else if (Event.current.type == EventType.MouseUp && isDraggingScrollView)
            {
                nodeToDrag = null;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle nodeStyle = nodeStyleBasic;
            if (node.IsPlayerSpeaking()) 
            {
                nodeStyle = nodeStylePlayer;
            }
            GUILayout.BeginArea(node.GetRect(), nodeStyle);

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            DrawButtons(node);

            GUILayout.EndArea();
        }

        private void DrawButtons(DialogueNode node)
        {
            if (GUILayout.Button("+ New Node"))
            {
                creatingNode = node;
            }

            if (linkingNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingNode = node;
                }
            }
            else if (linkingNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    Undo.RecordObject(currentDialogue, "Cancel Dialogue Link");
                    linkingNode = null;
                }
            }
            else if (linkingNode.GetChildrenNodes().Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    linkingNode.RemoveChild(node.name);
                    linkingNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    linkingNode.AddChild(node.name);
                    linkingNode = null;
                }
            }

            if (GUILayout.Button("- Remove Node"))
            {
                deletingNode = node;
            }
        }

        private void DrawBezierConnections(DialogueNode node)
        {
            Vector3 startPoint =  node.GetRect().center + new Vector2(node.GetRect().width/2, 0);
            foreach (var childNode in currentDialogue.GetChildren(node))
            {
                Vector3 endPoint = childNode.GetRect().center - new Vector2(childNode.GetRect().width / 2, 0);
                Vector3 bezierOffsetPoint = (endPoint - startPoint) * 0.7f;
                bezierOffsetPoint.y = 0;
                Handles.DrawBezier(startPoint, endPoint, 
                startPoint + bezierOffsetPoint, 
                endPoint - bezierOffsetPoint, 
                Color.white, null, 5f);
            }
        }

        private DialogueNode GetNodeAtMousePoint(Vector3 point)
        {
            DialogueNode nodeToReturn = null;       
            foreach (var node in currentDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    nodeToReturn = node;
                }
            }
            return nodeToReturn;
        }
    }
}


