using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PesanController : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Referensi ke UI Text
    // public TextMeshProUGUI messageText; // Uncomment jika menggunakan TextMeshPro

    public GameObject messagePanel; // Referensi ke panel untuk menampilkan pesan
    private PesanScene currentScene;
    private int currentSentenceIndex = 0;
    private bool isDialogueEnded = false;

    public float typingSpeed = 0.05f; // Kecepatan pengetikan teks

    public void SetupPesan(PesanScene scene)
    {
        currentScene = scene;
        messagePanel.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (currentSentenceIndex < currentScene.sentences.Count)
        {
            StopAllCoroutines();
            bool isLastSentence = currentSentenceIndex == currentScene.sentences.Count - 1;
            StartCoroutine(TypeSentence(currentScene.sentences[currentSentenceIndex].text, isLastSentence));
            currentSentenceIndex++;
        }
        else
        {
            isDialogueEnded = true;
        }
    }

    private IEnumerator TypeSentence(string sentence, bool isLastSentence)
    {
        messageText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (isLastSentence)
        {
            isDialogueEnded = true;
        }
    }

    private void Update()
    {
        if (isDialogueEnded && Input.GetMouseButtonDown(0))
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        GameController gameController = FindObjectOfType<GameController>();
        gameController.PlayScene(currentScene.nextScene);
        messagePanel.SetActive(false);
    }
}
