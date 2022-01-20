using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Math = Libraries.system.mathematics.Math;

public class SGT2 : MonoBehaviour
{
    public List<Sound> notes = new();
    public int octave = 4;
    public const float startNoteAValue = 27.5f;
    public int lsamplerate = 44100;
    public AudioClip audioClip;
    public TextAsset textAsset;

    internal static float _CalculateNoteUnoptimizedCorrect(int note, int octave)
    {
        return startNoteAValue * Mathf.Pow(2, (octave - 1) + (note + 3) / 12f);
    }

    public static readonly Dictionary<string, int> notesToNumbers = new Dictionary<string, int>
    {
        { "A", 9 },
        { "A#", 10 },
        { "B", 11 },
        { "C", 0 },
        { "C#", 1 },
        { "D", 2 },
        { "D#", 3 },
        { "E", 4 },
        { "F", 5 },
        { "F#", 6 },
        { "G", 7 },
        { "G#", 8 },
        { "-", 12 },
    };

    [Button]
    public void a()
    {
        StartCoroutine(nameof(PlayNotes));
    }

    [Button()]
    public void GetData()
    {
        int length = audioClip.samples;
        float[] data = new float[length];
        audioClip.GetData(data, 0);
        Debug.Log(length);
        StreamWriter writer = new StreamWriter(AssetDatabase.GetAssetPath(textAsset), false);

        for (int i = 0; i < length; i++)
        {
            writer.WriteLine(data[i]);
        }

        writer.Close();
    }


    public IEnumerator PlayNotes()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            Debug.Log("playing note" + notes[i]);
            if (notes[i].frequency == 0)
            {
                notes[i].SetFrequency();
            }

            PlayNote(notes[i]);


            yield return new WaitForSeconds(notes[i].length);
        }
    }


    public void PlayNote(Sound sound)
    {
        if (sound.note == "-")
        {
            return;
        }

        float[] samples = new float[lsamplerate];
        int length = Math.Round(lsamplerate * sound.length);
        Action<int> function = i => { };
        switch (sound.instrument)
        {
            case Instrument.Square:
                function = i =>
                {
                    samples[i] = PackIt((Mathf.Repeat(i * sound.frequency / lsamplerate, 1) > 0.5f) ? 1f : -1f);
                };
                break;
            case Instrument.Sine:
                function = i => { samples[i] = PackIt(Mathf.Sin(Mathf.PI * 2 * i * sound.frequency / lsamplerate)); };
                break;
            case Instrument.Sawtooth:
                function = i => { samples[i] = PackIt(Mathf.Repeat(i * sound.frequency / lsamplerate, 1) * 2f - 1f); };
                break;
            case Instrument.Triangle:
                function = i =>
                {
                    samples[i] = PackIt(Mathf.PingPong(i * 2f * sound.frequency / lsamplerate, 1) * 2f - 1f);
                };
                break;
        }

        Parallel.For(0, length,
            function);
        /* for (int i = 0; i < samples.Length; i++)
         {
             samples[i] = PackIt((Mathf.Repeat(i * frequency / lsamplerate, 1) > 0.5f) ? 1f : -1f);
             // samples[i] = PackIt(Mathf.Sin(Mathf.PI * 2 * i * frequency / lsamplerate));
         }*/

        AudioClip ac = AudioClip.Create("Test", samples.Length, 1, lsamplerate, false);
        ac.SetData(samples, 0);

        PlayClip(ac);
    }

    public float PackIt(float val)
    {
        return 0.5f * Mathf.FloorToInt(val * 255) / 255f;
    }

    public void PlayClip(AudioClip ac)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = Camera.main.transform.position;
        AudioSource ass = go.AddComponent<AudioSource>();
        ass.loop = false;
        ass.dopplerLevel = 0;
        ass.reverbZoneMix = 0;
        ass.spatialBlend = 0;
        ass.clip = ac;
        ass.Play();
        Destroy(go, ac.length);
    }
}

[Serializable]
public class Sound
{
    public string note;
    public int octave;
    [NonSerialized] public float frequency = 0;
    public Instrument instrument;
    public float length = 1;

    public Sound(string note, int octave, float length)
    {
        this.note = note;
        this.octave = octave;
        this.length = length;
        SetFrequency();
    }

    public void SetFrequency()
    {
        frequency = SGT2._CalculateNoteUnoptimizedCorrect(SGT2.notesToNumbers[note], this.octave);
    }

    public override string ToString()
    {
        return $"{note} {octave} {instrument.ToString()} {frequency}";
    }
}

public enum Instrument
{
    Square,
    Sine,
    Triangle,
    Sawtooth
}