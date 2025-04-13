using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroCutsceneManager : MonoBehaviour
{
    public TypeWriter typeWriter;
    public string typeWriterDisplay;
    void Start()
    {
        StartCoroutine(TransferScene());        
    }

    IEnumerator TransferScene()
    {
        typeWriter.SentText(typeWriterDisplay);
        yield return new WaitForSeconds(25);

        SceneManager.LoadScene("NicePet");
    }
}
