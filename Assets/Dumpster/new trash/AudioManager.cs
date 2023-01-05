using Libraries.system.mathematics;
using Libraries.system.output.music;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public int lsamplerate = 44100;
    public List<AudioSource> audioSources;

    public int audioSourcesIndex = 0;

    internal void PlayNote(Sound sound)
    {

        if (sound.note == Note.Rest)
        {
            return;
        }

        int length = Maths.Round(lsamplerate * sound.length);

        float[] samples = new float[length];
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

    internal void PlayClip(AudioClip ac)
    {
        audioSourcesIndex++;

        audioSourcesIndex %= audioSources.Count;
        AudioSource ass = audioSources[audioSourcesIndex];

        ass.clip = ac;
        ass.Play();
    }

    internal float PackIt(float val)
    {
        return 1f * Mathf.FloorToInt(val * 255) / 255f;
    }
}