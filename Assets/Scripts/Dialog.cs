using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text dialogueText;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    private bool playerCanInteract;
    private bool isWalkingNPC;

    public GameObject dialogBox;
    public GameObject continueIcon;

    void Start()
    {
        playerCanInteract = false;
        isWalkingNPC = ((gameObject.GetComponent<NPC>().patrolPoints.Length == 0) ? true : false);
    }

    void Update()
    {
        // if dialogue box is not in use by some other char
        if (playerCanInteract && !dialogBox.activeSelf && index == 0 && Input.GetKeyDown(KeyCode.E))
        {
            dialogBox.SetActive(true);
            gameObject.GetComponent<NPC>().interactingWithPlayer = true;
            StartCoroutine(Type());
        }
        else if (playerCanInteract && !dialogBox.activeSelf && (index == sentences.Length - 1))
        {
            index = 0;
        }
        else
        {
            if (dialogueText.text == sentences[index])
            {
                continueIcon.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    NextSentence();
                }
            }
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueIcon.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Type());
        }
        else
        {
            dialogueText.text = "";
            continueIcon.SetActive(false);
            dialogBox.SetActive(false);
            gameObject.GetComponent<NPC>().interactingWithPlayer = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCanInteract = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCanInteract = false;
        }
    }
}
