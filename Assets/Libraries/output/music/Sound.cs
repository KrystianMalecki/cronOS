using System;

namespace Libraries.system.output.music
{
//todo-think maybe make it struct?
    [Serializable]
    public class Sound
    {
        public Note note;
        public int octave;
        [NonSerialized] public float frequency = 0;
        public Instrument instrument;
        public float length = 1;

        public Sound(string note, int octave, float length)
        {
            this.note = AudioHandler.NoteToEnum(AudioHandler.notesToNumbers[note]);
            this.octave = octave;
            this.length = length;
            SetFrequency();
        }

        public Sound(Note note, int octave, float length)
        {
            this.note = note;
            this.octave = octave;
            this.length = length;
            SetFrequency();
        }

        public void SetFrequency()
        {
            frequency = AudioHandler._CalculateNoteUnoptimizedCorrect(AudioHandler.EnumToNote(note), this.octave);
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

    public enum Note
    {
        A,
        ASharp,
        B,
        C,
        CSharp,
        D,
        DSharp,
        E,
        F,
        FSharp,
        G,
        GSharp,
        Rest
    }
}