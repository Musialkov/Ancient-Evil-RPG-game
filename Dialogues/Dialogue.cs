using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Dialogues
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);

        Dictionary<string, DialogueNode> nodesDictionary = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void OnValidate() 
        {
            nodesDictionary.Clear();
            foreach (var node in GetAllNodes())
            {
                nodesDictionary[node.name] = node;
            }
        }
#endif
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetChildren(DialogueNode parentNode)
        {
            List<DialogueNode> listOfChilden = new List<DialogueNode>();
            foreach (var childID in parentNode.GetChildrenNodes())
            {
                if (nodesDictionary.ContainsKey(childID))
                {
                    listOfChilden.Add(nodesDictionary[childID]);
                }
            }
            return listOfChilden;
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode parentNode)
        {
            foreach (var childNode in GetChildren(parentNode))
            {
                if(childNode.IsPlayerSpeaking())
                {
                    yield return childNode;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode parentNode)
        {
            foreach (var childNode in GetChildren(parentNode))
            {
                if (!childNode.IsPlayerSpeaking())
                {
                    yield return childNode;
                }
            }
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }


#if UNITY_EDITOR
        public void AddNode(DialogueNode parentNode)
        {
            DialogueNode childNode = MakeNode(parentNode);
            Undo.RegisterCreatedObjectUndo(childNode, "Created new Dialogue Node");
            Undo.RecordObject(this, "Added dialogue node");
            AddNodeToList(childNode);
        }

        private DialogueNode MakeNode(DialogueNode parentNode)
        {
            DialogueNode childNode = CreateInstance<DialogueNode>();
            childNode.name = System.Guid.NewGuid().ToString();
            if (parentNode != null)
            {
                parentNode.AddChild(childNode.name);
                childNode.SetIsPlayerSpeaking(!parentNode.IsPlayerSpeaking());
                childNode.SetPosition(parentNode.GetRect().position + newNodeOffset);
            }

            return childNode;
        }

        private void AddNodeToList(DialogueNode childNode)
        {
            nodes.Add(childNode);
            OnValidate();
        }

        public void RemoveNode(DialogueNode nodeToRemove)
        {
            Undo.RecordObject(this, "Remove node");
            nodes.Remove(nodeToRemove);
            OnValidate();
            foreach (var node in GetAllNodes())
            {
                node.RemoveChild(nodeToRemove.name);
            }
            Undo.DestroyObjectImmediate(nodeToRemove);
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode childNode = MakeNode(null);
                AddNodeToList(childNode);
            }

            if(AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (var node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}
