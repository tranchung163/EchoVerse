using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    public TMP_Text textBox;
    public Animator animator;
    public float typingSpeed = 0.05f;  // Speed at which the text types
    public AudioSource audioSource;
    public bool StillTalking = false;

    public void SentText(string _text)
    {
        StillTalking = true;
        textBox.text = "";  // Clear previous text

        // Start typing the text
        StartCoroutine(TypeText(_text));
        animator.SetBool("Talking", true);
    }

    private IEnumerator TypeText(string _text)
    {
        audioSource.Play();
        foreach (char letter in _text)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        StillTalking = false;

        animator.SetBool("Talking", false);
        audioSource.Stop();
    }

}
