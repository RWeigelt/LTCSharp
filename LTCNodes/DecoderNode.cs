using System;
using System.Collections.Generic;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace LTC.Nodes
{
	public class DecoderNode : IDisposable
	{
		class DecodeInstance : IDisposable
		{
			LTCSharp.Decoder FDecoder;
			WasapiCapture FCapture;
			int FChannel;

			public DecodeInstance(MMDevice device, uint channels, uint channel, double framerate)
			{
				if (device == null)
					throw (new Exception("No device selected"));

				FChannel = (int) channel;
				FCapture = new WasapiCapture(device);
				FCapture.WaveFormat = new WaveFormat(44100, 8, (int) channels);
				channels = (uint) FCapture.WaveFormat.Channels;

				if (channel >= channels)
				{
					throw (new Exception("Capture channel index out of range"));
				}

				FDecoder = new LTCSharp.Decoder(FCapture.WaveFormat.SampleRate, (int) framerate, 32);

				FCapture.DataAvailable += FCapture_DataAvailable;
				FCapture.StartRecording();
			}

			unsafe void FCapture_DataAvailable(object sender, WaveInEventArgs e)
			{
				lock (FDecoder)
				{
					byte[] downSampled = new byte[e.BytesRecorded / 2];
					for (int i = 0; i < e.BytesRecorded / 2; i++)
					{
						downSampled[i] = e.Buffer[i * 2 + FChannel];
					}

					FDecoder.Write(downSampled, e.BytesRecorded / 2, 0);
				}
			}

			public LTCSharp.Timecode Timecode
			{
				get
				{
					return FDecoder.Read().getTimecode();
				}
			}

			public void Dispose()
			{
				FCapture.StopRecording();
			}
		}

		public List<LTCSharp.Timecode> OutTimecode { get; private set; } = new List<LTCSharp.Timecode>();
		public List<string> OutStatus { get; private set; } = new List<string>();

		private List<DecodeInstance> _instances = new List<DecodeInstance>();

		public void Update(IList<MMDevice> devices, IList<int> framerates, IList<uint> channels, IList<uint> channelIndices)
		{
			foreach (var instance in _instances)
			{
				instance?.Dispose();
			}
			_instances.Clear();

			int count = devices.Count;
			var newTimecodes = new List<LTCSharp.Timecode>(count);
			var newStatuses = new List<string>(count);

			for (int i = 0; i < count; i++)
			{
				try
				{
					var instance = new DecodeInstance(devices[i], channels[i], channelIndices[i], framerates[i]);
					_instances.Add(instance);
					newStatuses.Add("OK");
					newTimecodes.Add(null);
				}
				catch (Exception e)
				{
					_instances.Add(null);
					newStatuses.Add(e.Message);
					newTimecodes.Add(null);
				}
			}

			OutTimecode = newTimecodes;
			OutStatus = newStatuses;
		}

		public void ReadTimecodes()
		{
			for (int i = 0; i < _instances.Count; i++)
			{
				if (_instances[i] != null)
				{
					try
					{
						OutTimecode[i] = _instances[i].Timecode;
					}
					catch
					{
					}
				}
				else
				{
					OutTimecode[i] = null;
				}
			}
		}

		public void Dispose()
		{
			foreach (var instance in _instances)
			{
				instance?.Dispose();
			}
		}
	}
}

