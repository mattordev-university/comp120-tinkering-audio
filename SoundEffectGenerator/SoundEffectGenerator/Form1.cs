using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NAudio;
using NAudio.Wave;

namespace SoundEffectGenerator
{

    public partial class Form1 : Form
    {

        private readonly int SAMPLE_RATE = 44100;
        private readonly int CHANNEL_COUNT = 1;
        private readonly int MAX_VALUE = (int)Math.Pow(2, 15);
        private readonly int DEFAULT_MELODY_NOTE_COUNT = 12;

        private readonly double volume = 0.08;

        private delegate double WaveFunction(double frequency, int position);
        private delegate double MathFunction(double x);

        private WaveOut waveOut = null;

        private List<double> notes;
        private double[] noteDurations;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private List<int> GenerateSilence(double durationInSeconds)
        {
            List<int> silence = new List<int>();

            for (int i = 0; i < (int)(durationInSeconds * SAMPLE_RATE); i++)
            {
                silence.Add(0);
            }

            return silence;
        }

        private List<int> GenerateTone(double durationInSeconds, WaveFunction waveFunction, double[] frequencies)
        {
            List<int> tone = new List<int>();

            short value;
            for (int i = 0; i < (int)(durationInSeconds * SAMPLE_RATE); i++)
            {
                value = 0;
                for (int j = 0; j < frequencies.Length; j++)
                {
                    value += (short)(waveFunction.Invoke(frequencies[j], i));
                }
                tone.Add(value);
            }
            return tone;
        }

        private List<int> GenerateRandomMelody(int countOfNotesToPlay, WaveFunction waveFunction)
        {
            Random rng = new Random();

            List<int> melody = new List<int>();
            melody.AddRange(GenerateSilence(0.1));

            double[] frequencies = new double[2];
            for (int i = 0; i < countOfNotesToPlay; i++)
            {
                frequencies[0] = GetRandomElement(notes, rng);
                frequencies[1] = GetRandomElement(notes, rng);

                melody.AddRange(
                    GenerateTone(
                        GetRandomElement<double>(noteDurations, rng),
                        waveFunction,
                        frequencies
                    ));
            }

            return melody;
        }

        private static T GetRandomElement<T>(IEnumerable<T> enumerable, Random rng)
        {
            int index = rng.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }

        /// <summary>
        /// Populates a given list of doubles with frequencies representing notes
        /// according to Helmholtz’s scale . Will clear the list prior to populating.
        /// </summary>
        /// <param name="notes">
        /// The list of notes which will be populated</param>
        /// <param name="baseNote">Base note for tuning an equal−tempered scale, defaults to A4 (440Hz)
        /// </param>
        /// <param name="startNote">
        /// Distance from baseNote to start, typically negative or zero
        /// </param>
        /// <param name="endNote">
        /// Distance from baseNote to end, typically positive, must be larger than startNote
        /// </param>
        /// <param name="increment">
        /// Interval between notes,typically 1
        /// </param>
        private List<double> PopulateNotes(double baseNote, int startNote, int endNote, int increment)
        {
            double ESTIMATOR = Math.Pow(2.0, (1.0 / 12.0));

            List<double> notes = new List<double>();

            for (int i = startNote; i < endNote; i += increment)
            {
                notes.Add(baseNote * Math.Pow(ESTIMATOR, i));
            }

            return notes;
        }

        private double SquareWave(double frequency, int position)
        {
            double value = PositionInWavePeriod(frequency, position, Math.Sin, 2.0);
            if (value > 0)
            {
                value = 1.0;
            }
            else
            {
                value = -1.0;
            }
            return MAX_VALUE * volume * value;
        }

        private double SinWave(double frequency, int position)
        {
            return MAX_VALUE * volume * PositionInWavePeriod(frequency, position, Math.Sin, 2.0);
        }

        private double PositionInWavePeriod(double frequency, int position, MathFunction function, double piMultiplier)
        {
            return function.Invoke(piMultiplier * Math.PI * frequency * (position / (double)SAMPLE_RATE));
        }

        private void TinkeringAudioForm_Load(object sender, EventArgs e)
        {
            notes = PopulateNotes(440, -16, 8, 2);
            noteDurations = new double[] { 0.5, 0.2, 0.3, 0.4 };
        }

        private void Play_Click(object sender, EventArgs e)
        {
            waveOut = new WaveOut();
            waveOut.Init(convertToWaveProvider16(GenerateRandomMelody(DEFAULT_MELODY_NOTE_COUNT, SinWave), SAMPLE_RATE, CHANNEL_COUNT));
            waveOut.Play();
        }

        private void Save_Click(object sender, EventArgs e)
        {

            string filename = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".wav"
                );

            WaveFileWriter.CreateWaveFile(filename, convertToWaveProvider16(GenerateRandomMelody(DEFAULT_MELODY_NOTE_COUNT, SinWave), SAMPLE_RATE, CHANNEL_COUNT));
        }

        private IWaveProvider convertToWaveProvider16(List<int> sample, int sampleRate, int channelCount)
        {
            // Buffer is doubled for short -> byte conversion
            byte[] byteBuffer = new byte[sample.Count * 2];

            int byteArrayIndex = 0;
            short value;

            for (int i = 0; i < sample.Count; i++)
            {
                if (sample[i] > MAX_VALUE)
                {
                    value = (short)MAX_VALUE;
                }
                else if (sample[i] < -MAX_VALUE)
                {
                    value = (short)-MAX_VALUE;
                }
                else
                {
                    value = (short)sample[i];
                }

                byteBuffer[byteArrayIndex++] = BitConverter.GetBytes(value)[0];
                byteBuffer[byteArrayIndex++] = BitConverter.GetBytes(value)[1];
            }

            IWaveProvider waveProvider = new RawSourceWaveStream(
                new MemoryStream(byteBuffer),
                new WaveFormat(sampleRate, 16, channelCount)
                );

            return waveProvider;
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            
        }
    }
}
