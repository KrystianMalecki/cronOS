using Libraries.system.mathematics;
using Libraries.system.output.music;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class SGT2 : MonoBehaviour
{
    [SerializeField] private List<Sound> notes = new();

    public int octave = 4;
    public const float startNoteAValue = 27.5f;
    public int lsamplerate = 44100;

    public AudioClip audioClip;
    public TextAsset textAsset;
    public List<AudioSource> audioSources;
    public int audioSourcesIndex = 0;

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
        { "a", 9 },
        { "a#", 10 },
        { "b", 11 },
        { "c", 0 },
        { "c#", 1 },
        { "d", 2 },
        { "d#", 3 },
        { "e", 4 },
        { "f", 5 },
        { "f#", 6 },
        { "g", 7 },
        { "g#", 8 },
        { ".", 12 },
    };

    [Button]
    private void a()
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


    internal void PlayNote(Sound sound)
    {
        if (sound.note == Note.Rest)
        {
            return;
        }

        float[] samples = new float[lsamplerate];
        int length = Maths.Round(lsamplerate * sound.length);
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

    internal float PackIt(float val)
    {
        return 0.5f * Mathf.FloorToInt(val * 255) / 255f;
    }

    internal void PlayClip(AudioClip ac)
    {
        audioSourcesIndex++;
        if (audioSourcesIndex > audioSources.Count)
        {
            audioSourcesIndex = 0;
        }

        AudioSource ass = audioSources[audioSourcesIndex];
        ass.clip = ac;
        ass.Play();
    }
}