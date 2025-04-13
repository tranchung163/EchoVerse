using OpenAI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        public OpenAIController openAIController;
        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private TMP_InputField message;
        private readonly string fileName = "output.wav";
        private readonly int duration = 5;
        
        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi();

        private void Start()
        {
            recordButton.onClick.AddListener(StartRecording);
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        private void StartRecording()
        {
            isRecording = true;
            recordButton.enabled = false;
            
            #if !UNITY_WEBGL
            string defaultMic = Microphone.devices[0];
            clip = Microphone.Start(defaultMic, false, duration, 44100);
            #endif
        }

        private async void EndRecording()
        {
            message.text = "Transcripting...";
            
            #if !UNITY_WEBGL
            Microphone.End(null);
            #endif
            
            byte[] data = SaveWav.Save(fileName, clip);
            
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() {Data = data, Name = "audio.wav"},
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            progressBar.fillAmount = 0;
            message.text = res.Text;
            openAIController.GetResponse();
            recordButton.enabled = true;
        }

        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progressBar.fillAmount = time / duration;
                
                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    EndRecording();
                }
            }
        }
    }
}
