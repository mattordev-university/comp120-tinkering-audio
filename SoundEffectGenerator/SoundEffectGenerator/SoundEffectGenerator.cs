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
        private List<int> EffectsMelody;

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
        List<int> PhaseInverter(List<int> audioSample)
        {
            List<int> n = new List<int>();
			for (int i = 0; i < audioSample.Count; i++)
			{
                n.Add((audioSample[i] * -1));
			}
            return n;
        }

        List<int> NormaliseSample(List<int> audioSample)
        {
            int n = 0;
            for (int i = 0; i < audioSample.Count; i++)
            {
                n = Math.Max(n, audioSample[i]);
            }
            int o = 32765 / n;
            for (int i = 0; i < audioSample.Count; i++)
            {
                int p = 0;
                p = (o * audioSample[i]);
                audioSample[i] = p;
            }
            return audioSample;
        }

        List<int> ReverseSample(List<int> audioSample)
        {
            List<int> reversedAudio = new List<int>();
            for (int i = audioSample.Count - 1; i > 0; i--)
            {
                reversedAudio.Add(audioSample[i]);
            }
            return reversedAudio;
        }

        List<int> AmplitudeScale(List<int> audioSample, float minVolume, float maxVolume, float scaleFactor)
        {
            List<int> alteredAudio = new List<int>();
            for (int i = 0; i < audioSample.Count - 4; i += 4)
            {
                var v = audioSample[i] * scaleFactor;
                v = Math.Max(maxVolume, v);
                alteredAudio.Add((int)v);
            }
            return alteredAudio;
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
			if (TempMelody == null) //Checks if there is a melody, if not creates new one
			{
                TempMelody = GenerateRandomMelody(DEFAULT_MELODY_NOTE_COUNT, SinWave);
            }
            EffectsMelody = new List<int>();
            EffectsMelody = TempMelody;

            // if statments to check whether some of the effect checkboxes are checked.
			if (PhaseInverterEffect.Checked)
			{
                EffectsMelody = PhaseInverter(EffectsMelody);
			}
			if (NormaliseSampleEffect.Checked)
			{
                EffectsMelody = NormaliseSample(EffectsMelody);
			}
			if (ReverseSampleEffect.Checked)
			{
                EffectsMelody = ReverseSample(EffectsMelody);
            }
            //playAudio
            waveOut = new WaveOut();
            waveOut.Init(convertToWaveProvider16(EffectsMelody, SAMPLE_RATE, CHANNEL_COUNT));
            waveOut.Play();
            UpdateChart(chart1, TempMelody);
        }
        private void GenerateNotes_Click(object sender, EventArgs e)
        {
            // Here we make a new temporary melody, for use in other places using the GenerateRandomMelody Function
            TempMelody = GenerateRandomMelody(DEFAULT_MELODY_NOTE_COUNT, SinWave);
            waveOut = new WaveOut();
            waveOut.Init(convertToWaveProvider16(TempMelody, SAMPLE_RATE, CHANNEL_COUNT));
            //Play the generated tone
            waveOut.Play();
            // Update our chart to display the notes.
            UpdateChart(chart1, TempMelody);
        }
        private void playSound_Click(object sender, EventArgs e)
        { 
            // check if our TempMelody is not nothing
			if (TempMelody != null)
			{
                waveOut = new WaveOut();
                waveOut.Init(convertToWaveProvider16(TempMelody, SAMPLE_RATE, CHANNEL_COUNT));
                waveOut.Play();
                UpdateChart(chart1, TempMelody);
            }
        }

        /// <summary>
        /// This is how the program saves audio out, using the WaveFileWriter to create the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSound_Click(object sender, EventArgs e)
        {
            string filename = FileDirectoryTextBox.Text + FileNameTextBox.Text + ".wav";

            WaveFileWriter.CreateWaveFile(filename, convertToWaveProvider16(GenerateRandomMelody(DEFAULT_MELODY_NOTE_COUNT, SinWave), SAMPLE_RATE, CHANNEL_COUNT));
        }
        //Broken At This Time
        //private void LoadSound_Click(object sender, EventArgs e)
        //{
        //    string filename = FileDirectoryTextBox.Text+ FileNameTextBox.Text+ ".wav";

        //    var wf = new WaveFileReader(filename);
        //    List<int> audioSample = new List<int>();
        //    byte[] buffer = new byte[wf.Length * 2];
        //    wf.Read(buffer,0,(int)wf.Length);
        //    for (int i = 0; i < buffer.Length; i++)
        //    {
        //        audioSample.Add(BitConverter.ToInt32(buffer, i));
        //        audioSample.Add(BitConverter.ToInt32(buffer, i));
        //    }
        //    TempMelody = new List<int>();
        //    TempMelody = audioSample;
            
        //}

        #endregion


        private void UpdateChart(Chart chart, List<int> data)
        {
            // Here we make a new series for use
            Series dataSeries = new Series();
            // We then set that series type to a line, this is so the graph becomes a line graph
            dataSeries.ChartType = SeriesChartType.Line;
            // We bind our y data points to the passed in list
            dataSeries.Points.DataBindY(data);

            // clear the chart
            chart.Series.Clear();
            // Set the max X to be the SAMPLE_RATE
            chart.ChartAreas[0].AxisX.Maximum = SAMPLE_RATE;
            // Add the dataseries to the chart to display
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
