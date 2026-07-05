using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace ExampleDecode
{
	class Program
	{
		static LTCSharp.Decoder FDecoder;

		[STAThread]
		static void Main(string[] args)
		{
			WaveInExample();
			//FileLoadExample();
		}

		static void waveIn_DataAvailable(object sender, WaveInEventArgs e)
		{
			var waveIn = (WasapiCapture)sender;
			var fmt = waveIn.WaveFormat;

			lock (FDecoder)
			{
				int channels = fmt.Channels;
				int bytesPerSample = fmt.BitsPerSample / 8;
				int bytesPerFrame = channels * bytesPerSample;
				int frameCount = e.BytesRecorded / bytesPerFrame;

				byte[] mono = new byte[frameCount];

				if (fmt.Encoding == WaveFormatEncoding.IeeeFloat && fmt.BitsPerSample == 32)
				{
					// WASAPI shared mode default: 32-bit float — convert to unsigned 8-bit
					for (int i = 0; i < frameCount; i++)
					{
						float sum = 0f;
						for (int ch = 0; ch < channels; ch++)
						{
							sum += BitConverter.ToSingle(e.Buffer, i * bytesPerFrame + ch * 4);
						}
						float avg = sum / channels;                    // -1.0 .. +1.0
						mono[i] = (byte)Math.Clamp((int)(avg * 127f + 128f), 0, 255);
					}
				}
				else if (fmt.BitsPerSample == 16)
				{
					// 16-bit PCM signed: mix channels, convert to unsigned 8-bit
					for (int i = 0; i < frameCount; i++)
					{
						int sum = 0;
						for (int ch = 0; ch < channels; ch++)
						{
							sum += BitConverter.ToInt16(e.Buffer, i * bytesPerFrame + ch * 2);
						}
						int avg = sum / channels;                      // -32768 .. +32767
						mono[i] = (byte)Math.Clamp(avg / 256 + 128, 0, 255);
					}
				}
				else if (fmt.BitsPerSample == 8)
				{
					// 8-bit PCM unsigned: average channels
					for (int i = 0; i < frameCount; i++)
					{
						int sum = 0;
						for (int ch = 0; ch < channels; ch++)
							sum += e.Buffer[i * bytesPerFrame + ch];
						mono[i] = (byte)(sum / channels);
					}
				}
				else
				{
					Console.WriteLine($"Unsupported capture format: {fmt}");
					return;
				}

				FDecoder.Write(mono, frameCount, 0);
			}
		}

		static void WaveInExample()
		{
			var waveIn = new WasapiCapture();
			// Use the device's native mix format — WASAPI shared mode always delivers
			// the mix format regardless of what WaveFormat is set to, so we let it
			// capture natively and convert in the DataAvailable handler.
			Console.WriteLine("Device format: " + waveIn.WaveFormat.ToString());
			FDecoder = new LTCSharp.Decoder(waveIn.WaveFormat.SampleRate, 25, 32);
			waveIn.DataAvailable += waveIn_DataAvailable;
			waveIn.StartRecording();

			while (true)
			{
				int queueLength;
				lock (FDecoder)
				{
					queueLength = FDecoder.GetQueueLength();
				}

				if (queueLength > 0)
				{
					lock (FDecoder)
					{
						// Drain all available frames in one lock acquisition
						while (FDecoder.GetQueueLength() > 0)
						{
							try
							{
								var frame = FDecoder.Read();
								var timecode = frame.getTimecode();
								Console.WriteLine(timecode.ToString());
							}
							catch (Exception e)
							{
								Console.Write(e);
							}
						}
					}
				}
				else
				{
					// Sleep WITHOUT holding the lock so waveIn_DataAvailable
					// can always write audio data to the decoder unblocked.
					Thread.Sleep(10);
				}
			}
		}

		static void FileLoadExample()
		{
			OpenFileDialog selectFileDialog = new OpenFileDialog();
			selectFileDialog.Filter = "wav files (*.wav)|*.wav";
			selectFileDialog.RestoreDirectory = true;
			selectFileDialog.ShowDialog();
			var wavePlayer = new WaveFileReader(selectFileDialog.FileName);
			Console.WriteLine("File format: " + wavePlayer.WaveFormat.ToString());

			FDecoder = new LTCSharp.Decoder(wavePlayer.WaveFormat.SampleRate, 25, 32);

			int size = 1600;
			byte[] buffer = new byte[size];
			int total = 0;
			while (wavePlayer.Position < wavePlayer.Length)
			{
				var task = wavePlayer.Read(buffer, 0, size);

				FDecoder.WriteAsU16(buffer, size / 2, total);

				total += size / 2;

				try
				{
					var frame = FDecoder.Read();
					var timecode = frame.getTimecode();
					Console.WriteLine(wavePlayer.CurrentTime.ToString() + "\t" + timecode.ToString());
				}
				catch
				{
					//no frames available
				}
			}

			Console.WriteLine("END OF FILE");
		}
	}
}
