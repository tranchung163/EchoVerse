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
        if (animator != null) animator.SetBool("Talking", true);
    }

    private IEnumerator TypeText(string _text)
    {
        if (audioSource != null) audioSource.Play();
        if (audioSource != null) audioSource.volume = UnityEngine.Random.Range(0.9f, 1.2f);
        if (audioSource != null) audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.2f);
        foreach (char letter in _text)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        StillTalking = false;

        if (animator != null) animator.SetBool("Talking", false);
        if (audioSource != null) audioSource.Stop();
    }
}
