using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libraries.system.output.music
{
    public class AudioHandler : BaseLibrary
    {
        public void PlaySound(Sound sound)
        {
            Hardware.currentThreadInstance.hardwareInternal.stackExecutor.AddDelegateToStack(
                (hardware) => { hardware.hardwareInternal.audioManager.PlayNote(sound); }, sync);
        }

        private const float startNoteAValue = 27.5f;

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

        internal static float _CalculateNoteUnoptimizedCorrect(int note, int octave)
        {
            return startNoteAValue * Mathf.Pow(2, (octave - 1) + (note + 3) / 12f);
        }

        public static int StringToNote(string note)
        {
            if (!String.IsNullOrEmpty(note) && notesToNumbers.TryGetValue(note, out int noteNumber))
            {
                return noteNumber;
            }

            return 13;
        }

        public static int EnumToNote(Note note)
        {
            return note switch
            {
                Note.A => 9,
                Note.ASharp => 10,
                Note.B => 11,
                Note.C => 0,
                Note.CSharp => 1,
                Note.D => 2,
                Note.DSharp => 3,
                Note.E => 4,
                Note.F => 5,
                Note.FSharp => 6,
                Note.G => 7,
                Note.GSharp => 8,
                Note.Rest => 12,
                _ => 13
            };
        }

        public static Note NoteToEnum(int note)
        {
            return note switch
            {
                9 => Note.A,
                10 => Note.ASharp,
                11 => Note.B,
                0 => Note.C,
                1 => Note.CSharp,
                2 => Note.D,
                3 => Note.DSharp,
                4 => Note.E,
                5 => Note.F,
                6 => Note.FSharp,
                7 => Note.G,
                8 => Note.GSharp,
                12 => Note.Rest,
                _ => Note.Rest
            };
        }
    }
}