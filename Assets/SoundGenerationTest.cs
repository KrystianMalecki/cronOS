using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGenerationTest : MonoBehaviour
{
    public AudioSource ass;
    public int position = 0;
    public int lsamplerate = 44100;
    public int samplerate = 44100;
    public float frequency = 880;
    public int sum;
    public AnimationCurve ac;
    void Start()
    {
        sum = 0;
       // AudioClip clip = AudioClip.Create("MySinusoid", lsamplerate*2, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);


       // AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);

    }

    void OnAudioRead(float[] data)
    {
        int count = 0;
        Debug.Log(data.Length);
        sum += data.Length;

        while (count < data.Length)
        {
            data[count] = ac.Evaluate(1f * count / (lsamplerate*2));
            Debug.Log(ac.Evaluate(1f * count / (lsamplerate * 2)));
            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
    /* void Start()
     {
         AudioClip clip = AudioClip.Create("MySinusoid", samplerate * 2, 1, samplerate, true);
         AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);

     }

     void OnAudioRead(float[] data)
     {
         int count = 0;
         while (count < data.Length)
         {
             data[count] = Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate);
             position++;
             count++;
         }
     }

     void OnAudioSetPosition(int newPosition)
     {
         position = newPosition;
     }
     [Button("play", EButtonEnableMode.Playmode)]
     public void Play()
     {
         int length = 10;
         AudioClip clip = AudioClip.Create("", 44100 * 2, 1, 44100, false);

         float[] samples = new float[length];

         for (int i = 0; i < samples.Length; ++i)
         {
             samples[i] = (i + 1) / samples.Length;
         }
         clip.SetData(samples, 0);
         AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
     }*/
}
