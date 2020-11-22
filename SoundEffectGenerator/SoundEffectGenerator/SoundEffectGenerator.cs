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

    public partial class EffectGenerator : Form
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
        private List<int> TempMelody;

        public EffectGenerator()
        {
            InitializeComponent();
        }

        private void SoundEffectGenerator(object sender, EventArgs e)
        {
            notes = PopulateNotes(440, -3, 8, 1);
            noteDurations = new double[] { 0.5, 0.2, 0.3, 0.4 };

            
        }

        #region GENERATION
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

        /// <summary>
        /// This generates a random melody, and returns the melody for use in other places
        /// </summary>
        /// <param name="countOfNotesToPlay">
        /// This is the amount of notes that will play.
        /// </param>
        /// <param name="waveFunction">
        /// This is the TYPE of note that will play, whether that is sawtooth or sinwave etc.
        /// </param>
        /// <returns></returns>
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
        #endregion

        /// <summary>
        /// Gets a random element, uses an enumerable and a random
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This generates a squarewave
        /// </summary>
        /// <param name="frequency">
        /// the frequency of the wave to be created.
        /// </param>
        /// <param name="position">
        /// The position in the wave.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// The position in the wave
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="position"></param>
        /// <param name="function"></param>
        /// <param name="piMultiplier"></param>
        /// <returns>returns the sum of a calculation</returns>
        private double PositionInWavePeriod(double frequency, int position, MathFunction function, double piMultiplier)
        {
            return function.Invoke(piMultiplier * Math.PI * frequency * (position / (double)SAMPLE_RATE));
        }

        #region ALGORITHMS
        byte[] PhaseInverter(byte[] audioSample)
        {
            List<byte> n = new List<byte>();
            for (int i = 0; i < audioSample.Length - 3; i++)
            {
                var currentBytes = BitConverter.ToInt32(audioSample, i);
                if (currentBytes == 0)
                {
                    var invertedBytes = BitConverter.GetBytes(currentBytes);
                    n.Add(invertedBytes[0]);
                    //n.Add(invertedBytes[1]);
                }
                else
                {
                    var invertedBytes = BitConverter.GetBytes(currentBytes / currentBytes);
                    n.Add(invertedBytes[0]);
                    //n.Add(invertedBytes[1]);
                }

            }
            return n.ToArray();
        }

        byte[] NormaliseSample(byte[] audioSample)
        {
            int n = 0;
            for (int i = 0; i < audioSample.Length; i++)
            {
                n = Math.Max(n, audioSample[i]);
            }
            int o = 32765 / n;
            for (int i = 0; i < audioSample.Length; i++)
            {
                byte p = 0;
                p = (byte)(o * audioSample[i]);
                audioSample[i] = p;
            }
            return audioSample;
        }

        byte[] ReverseSample(byte[] audioSample)
        {
            List<byte> reversedAudio = new List<byte>();
            for (int i = audioSample.Length - 1; i > 0; i--)
            {
                reversedAudio.Add(audioSample[i]);
            }
            return reversedAudio.ToArray();
        }

        byte[] AmplitudeScale(byte[] audioSample, float minVolume, float maxVolume, float scaleFactor)
        {
            List<byte> alteredAudio = new List<byte>();
            for (int i = 0; i < audioSample.Length - 4; i += 4)
            {
                var v = (BitConverter.ToInt32(audioSample, i)) * scaleFactor;
                v = Math.Max(maxVolume, v);
                //v = Math.Min(minVolume, v);
                var bit = BitConverter.GetBytes(v);
                alteredAudio.AddRange(bit);
            }
            return alteredAudio.ToArray();
        }
        #endregion


        #region BUTTONFUNCTIONS
        /// <summary>
        /// This function is what allows the program to function. It uses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generate_Click(object sender, EventArgs e)
        {
            TempMelody = GenerateRandomMelody(DEFAULT_MELODY_NOTE_COUNT, SinWave);
            waveOut = new WaveOut();
            waveOut.Init(convertToWaveProvider16(TempMelody, SAMPLE_RATE, CHANNEL_COUNT));
            waveOut.Play();
            UpdateChart(chart1, TempMelody);
        }

        /// <summary>
        /// This is how the program saves audio out, using the WaveFileWriter to create the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {

            string filename = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".wav"
                );

            WaveFileWriter.CreateWaveFile(filename, convertToWaveProvider16(GenerateRandomMelody(DEFAULT_MELODY_NOTE_COUNT, SinWave), SAMPLE_RATE, CHANNEL_COUNT));
        }
        #endregion


        private void UpdateChart(Chart chart, List<int> data)
        {
            Series dataSeries = new Series();
            dataSeries.ChartType = SeriesChartType.Line;
            dataSeries.Points.DataBindY(data);

            chart.Series.Clear();
            chart.ChartAreas[0].AxisX.Maximum = SAMPLE_RATE;
            chart.Series.Add(dataSeries);
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

    }
}
