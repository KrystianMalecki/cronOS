using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundGenerationTest : MonoBehaviour
{
    public AudioSource ass;
    public int position = 0;
    public int lsamplerate = 44100;
    //  public int samplerate = 44100;
    public float frequency = 880;
    public int sum;
    public AnimationCurve ac;
    public int type;
    public void Play()
    {

        if (type == 0)
        {
            sine();
        }
        if (type == 1)
        {
            square();
        }
        if (type == 2)
        {
            saw();
        }
        if (type == 3)
        {
            triangle();
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            frequency = CalculateNote(0 + (Input.GetKey(KeyCode.LeftShift) ? 1 : 0), octave);
            Play();
        }
        if (Input.GetKeyDown("b"))
        {
            frequency = CalculateNote(1, octave);
            Play();
        }
        if (Input.GetKeyDown("c"))
        {
            frequency = CalculateNote(3 + (Input.GetKey(KeyCode.LeftShift) ? 1 : 0), octave);
            Play();
        }
        if (Input.GetKeyDown("d"))
        {
            frequency = CalculateNote(5 + (Input.GetKey(KeyCode.LeftShift) ? 1 : 0), octave);
            Play();
        }
        if (Input.GetKeyDown("e"))
        {
            frequency = CalculateNote(7, octave);
            Play();
        }
        if (Input.GetKeyDown("f"))
        {
            frequency = CalculateNote(8 + (Input.GetKey(KeyCode.LeftShift) ? 1 : 0), octave);
            Play();
        }
        if (Input.GetKeyDown("g"))
        {
            frequency = CalculateNote(10 + (Input.GetKey(KeyCode.LeftShift) ? 1 : 0), octave);
            Play();
        }
        /*   if (Input.GetKeyDown("i"))
           {
               frequency = CalculateNote(7, octave);
               Play();
           }
           if (Input.GetKeyDown("o"))
           {
               frequency = CalculateNote(8, octave);
               Play();
           }
           if (Input.GetKeyDown("p"))
           {
               frequency = CalculateNote(9, octave);
               Play();
           }
           if (Input.GetKeyDown("["))
           {
               frequency = CalculateNote(10, octave);
               Play();
           }
           if (Input.GetKeyDown("]"))
           {
               frequency = CalculateNote(11, octave);
               Play();
           }*/
    }
    [Button]
    void square()
    {


        float[] samples = new float[lsamplerate];

        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = PackIt((Mathf.Repeat(i * frequency / lsamplerate, 1) > 0.5f) ? 1f : -1f);
        }

        AudioClip ac = AudioClip.Create("Test", samples.Length, 1, lsamplerate, false);
        ac.SetData(samples, 0);

     //   AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
        ass.clip = (ac);
        ass.Play();
    }
    [Button]
    void sine()
    {


        float[] samples = new float[lsamplerate];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = PackIt(Mathf.Sin(Mathf.PI * 2 * i * frequency / lsamplerate));
        }



        AudioClip ac = AudioClip.Create("Test", samples.Length, 1, lsamplerate, false);
        ac.SetData(samples, 0);
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);

    }
    public float PackIt(float val)
    {
        // return val;
        return 0.5f * Mathf.FloorToInt(val * 255) / 255;
    }
    [Button]
    void saw()
    {


        float[] samples = new float[lsamplerate];


        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = PackIt(Mathf.Repeat(i * frequency / lsamplerate, 1) * 2f - 1f);
        }

        AudioClip ac = AudioClip.Create("Test", samples.Length, 1, lsamplerate, false);
        ac.SetData(samples, 0);
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);

    }
    [Button]
    void triangle()
    {


        float[] samples = new float[lsamplerate];



        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = PackIt(Mathf.PingPong(i * 2f * frequency / lsamplerate, 1) * 2f - 1f);
        }
        AudioClip ac = AudioClip.Create("Test", samples.Length, 1, lsamplerate, false);
        ac.SetData(samples, 0);
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);

    }
    void OnAudioRead(float[] data)
    {
        int count = 0;
        Debug.Log(data.Length);
        sum += data.Length;

        while (count < data.Length)
        {
            data[count] = ac.Evaluate(1f * count / (lsamplerate * 2));
            Debug.Log(ac.Evaluate(1f * count / (lsamplerate * 2)));
            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }



    public static float startNoteAValue = 27.5f;

    public float CalculateNote(int note, int octave)
    {
        return startNoteAValue * Mathf.Pow(2, octave - (note + 11) / 12 + note / 12f);
        // return startNoteAValue * Mathf.Pow(2, (octave - 1) + note / 12f);
    }
    private float _CalculateNoteUnoptimized(int note, int octave)
    {
        return startNoteAValue * Mathf.Pow(2, (octave - 1) + note / 12f + (1 - 1 * ((11 + note) / 12)));

    }
    public int note = 3;
    public int octave = 4;
    public float outPut;
    [Button]
    public void calc()
    {

        outPut = CalculateNote(note, octave);
    }
    [Button]
    public void test()
    {
        StaticHelper.TestFunction(() =>
        {
            for (int octave = 0; octave < 8; octave++)
            {
                for (int note = 0; note < 12; note++)
                {
                    CalculateNote(note, octave);
                }
            }
        });
        StaticHelper.TestFunction(() =>
        {
            for (int octave = 0; octave < 8; octave++)
            {
                for (int note = 0; note < 12; note++)
                {
                    _CalculateNoteUnoptimized(note, octave);
                }
            }
        });
    }
}

