using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogues;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerDialogueManager playerDialogueManager;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button quitButton;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform playerResponses;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] TextMeshProUGUI conversantName;

        void Start()
        {
            playerDialogueManager = 
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueManager>();
            playerDialogueManager.onConversationUpdated += RedrawUI;
            nextButton.onClick.AddListener(Next);
            quitButton.onClick.AddListener(Quit);

            RedrawUI();
        }

        private void Next()
        {
            playerDialogueManager.Next();
        }
        
        private void Quit()
        {
            playerDialogueManager.QuitDialogue();
        }

        private void RedrawUI()
        {
            gameObject.SetActive(playerDialogueManager.IsActive());
            if (!playerDialogueManager.IsActive()) return;

            conversantName.text = playerDialogueManager.GetCurrentConversantName();

            AIResponse.SetActive(!playerDialogueManager.IsChoosing());
            playerResponses.gameObject.SetActive(playerDialogueManager.IsChoosing());

            if(playerDialogueManager.IsChoosing())
            {
                BuildResponseList();
            }
            else
            {
                AIText.text = playerDialogueManager.GetText();
                nextButton.gameObject.SetActive(playerDialogueManager.HasNext());
                quitButton.gameObject.SetActive(!playerDialogueManager.HasNext());
            }        
        }

        private void BuildResponseList()
        {
            foreach (Transform item in playerResponses)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode node in playerDialogueManager.GetChoices())
            {
                GameObject choiceResponse = Instantiate(choicePrefab, playerResponses);
                var textMeshPro = choiceResponse.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.text = node.GetText();
                Button button = choiceResponse.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => {
                    playerDialogueManager.SelectChoice(node); 
                });
            }
        }
    }
}