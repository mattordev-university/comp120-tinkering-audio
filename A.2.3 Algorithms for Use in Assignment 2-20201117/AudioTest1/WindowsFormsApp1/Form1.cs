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
using NAudio;
using NAudio.Wave;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		int sampleRate = 44100;
		int maxValue = (int)Math.Pow(2, 15);

		double volume = 0.08;
		delegate double WaveFunction(double frequency, int position);
		private WaveFunction waveFunction = null;

		private WaveOut waveOut = null;
		IWaveProvider waveProvider = null;

		private List<double> notes;
		double[] noteDurations;
		byte[] currentAudioSample;
		public Form1()
		{
			InitializeComponent();
		}

		List<byte> GenerateSilence(double durationInSeconds)
		{
			List<byte> silence = new List<byte>();

			for (int i = 0; i < (int)(durationInSeconds * sampleRate); i++)
			{
				silence.Add(0);
				silence.Add(0);
			}
			return silence;
		}

		List<byte> GenerateTone(double durationInSeconds, WaveFunction waveFunction, double[] frequencies)
		{
			List<byte> tone = new List<byte>();
			short value;
			for (int i = 0; i < (int)(durationInSeconds * sampleRate); i++)
			{
				value = 0;
				for (int j = 0; j < frequencies.Length; j++)
				{
					value += (short)(maxValue * volume * waveFunction.Invoke(frequencies[j], i));
				}
				tone.Add(BitConverter.GetBytes(value)[0]);
				tone.Add(BitConverter.GetBytes(value)[1]);
			}
			return tone;
		}

		List<byte> GenerateRandomMelody(int countOfNotesToPlay)
		{
			Random rng = new Random();
			List<byte> melody = new List<byte>();
			melody.AddRange(GenerateSilence(0.1));
			double[] frequencies = new double[2];
			for (int i = 0; i < countOfNotesToPlay; i++)
			{
				frequencies[0] = GetRandomElement<double>(notes, rng);
				frequencies[1] = GetRandomElement<double>(notes, rng);

				melody.AddRange(GenerateTone(GetRandomElement<double>(noteDurations,rng), this.waveFunction, frequencies));
			}
			return melody;
		}
		static T GetRandomElement<T>(IEnumerable<T> enumerable,Random rng)
		{
			int index = rng.Next(0, enumerable.Count());
			return enumerable.ElementAt(index);
		}

		private List<double> populateNotes(double baseNote,int startNote,int endNote,int increment)
		{
			double estimator = Math.Pow(2.0,(1.0/12.0));
			List<double> notes = new List<double>();
			for (int i = startNote; i < endNote; i+= increment)
			{
				notes.Add(baseNote * Math.Pow(estimator, i));
			}
			return notes;
		}

		private double SquareWave(double frequency, int position)
		{
			double value = Math.Sin(2.0 * Math.PI * frequency * (position / (double)sampleRate));
			if (value >0)
			{
				return 1.0;
			}
			else
			{
				return -1.0;
			}
		}

		private double SinWave(double frequency, int position)
		{
			return Math.Sin(2.0 * Math.PI * frequency * (position / (double)sampleRate));
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			waveFunction = SquareWave;
			notes = populateNotes(440,-16,8,2);
			noteDurations = new double[] { 0.15, 0.2, 0.3, 0.4 };
		}

		private void button1_Click(object sender, EventArgs e)
		{
			
			byte[] audioSample = GenerateRandomMelody(12).ToArray();

			currentAudioSample = audioSample;
			
			waveProvider = new RawSourceWaveStream(new MemoryStream(audioSample), new WaveFormat(sampleRate, 1));
			waveOut = new WaveOut();
			waveOut.Init(waveProvider);
			waveOut.Play();
		}
		private void button2_Click(object sender, EventArgs e)
		{
			byte[] audioSample;
			if (currentAudioSample != null)
			{
				audioSample = currentAudioSample;
			}
			else
			{
				audioSample = GenerateRandomMelody(12).ToArray();
			}

			waveProvider = new RawSourceWaveStream(new MemoryStream(PhaseInverter(audioSample)), new WaveFormat(sampleRate, 1));
			waveOut = new WaveOut();
			waveOut.Init(waveProvider);
			waveOut.Play();
		}
		private void button3_Click(object sender, EventArgs e)
		{
			byte[] audioSample;
			if (currentAudioSample != null)
			{
				audioSample = currentAudioSample;
			}
			else
			{
				audioSample = GenerateRandomMelody(12).ToArray();
			}

			waveProvider = new RawSourceWaveStream(new MemoryStream(NormaliseSample(audioSample)), new WaveFormat(sampleRate, 1));
			waveOut = new WaveOut();
			waveOut.Init(waveProvider);
			waveOut.Play();
		}
		private void button4_Click(object sender, EventArgs e)
		{
			byte[] audioSample;
			if (currentAudioSample != null)
			{
				audioSample = currentAudioSample;
			}
			else
			{
				audioSample = GenerateRandomMelody(12).ToArray();
			}

			waveProvider = new RawSourceWaveStream(new MemoryStream(ReverseSample(audioSample)), new WaveFormat(sampleRate, 1));
			waveOut = new WaveOut();
			waveOut.Init(waveProvider);
			waveOut.Play();
		}
		private void button5_Click(object sender, EventArgs e)
		{
			byte[] audioSample;
			if (currentAudioSample != null)
			{
				audioSample = currentAudioSample;
			}
			else
			{
				audioSample = GenerateRandomMelody(12).ToArray();
			}

			waveProvider = new RawSourceWaveStream(new MemoryStream(AmplitudeScale(audioSample,0,500,1f)), new WaveFormat(sampleRate, 1));
			waveOut = new WaveOut();
			waveOut.Init(waveProvider);
			waveOut.Play();
		}

		#region Algorithms
		byte[] PhaseInverter(byte[] audioSample)
		{
			List<byte> n = new List<byte>();
			for (int i = 0; i < audioSample.Length-3; i++)
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
			for (int i = audioSample.Length-1; i > 0; i--)
			{
				reversedAudio.Add(audioSample[i]);
			}
			return reversedAudio.ToArray();
		}

		byte[] AmplitudeScale(byte[] audioSample,float minVolume,float maxVolume, float scaleFactor)
		{
			List<byte> alteredAudio = new List<byte>();
			for (int i = 0; i < audioSample.Length-4; i+=4)
			{
				var v = (BitConverter.ToInt32(audioSample,i)) * scaleFactor;
				v = Math.Max(maxVolume, v);
				//v = Math.Min(minVolume, v);
				var bit = BitConverter.GetBytes(v);
				alteredAudio.AddRange(bit);
			}
			return alteredAudio.ToArray();
		}


		#endregion

		
	}
}
