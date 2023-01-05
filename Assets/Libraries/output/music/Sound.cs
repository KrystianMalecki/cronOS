using System;

namespace Libraries.system.output.music
{

    [Serializable]
    public struct Sound
    {
        public Note note;
        public int octave;
        [NonSerialized] public float frequency;
        public Instrument instrument;
        public float length;

        public Sound(string note, int octave = 4, float length = 1, Instrument instrument = Instrument.Sine) : this(
            AudioHandler.NoteToEnum(AudioHandler.notesToNumbers[note]), octave, length, instrument)
        {
        }


        public Sound(Note note, int octave = 4, float length = 1, Instrument instrument = Instrument.Sine)
        {
            this.note = note;
            this.octave = octave;
            this.length = length;
            frequency = 0;
            this.instrument = instrument;
            SetFrequency();
        }

        public Sound(int note, int octave = 4, float length = 1, Instrument instrument = Instrument.Sine) : this(AudioHandler.NoteToEnum(note), octave, length, instrument)
        {
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