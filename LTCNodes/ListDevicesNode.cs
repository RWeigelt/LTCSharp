using System.Collections.Generic;
using System.Linq;
using NAudio.CoreAudioApi;

namespace LTC.Nodes
{
	public class ListDevicesNode
	{
		public List<MMDevice> Devices { get; private set; } = new List<MMDevice>();
		public List<string> Names { get; private set; } = new List<string>();
		public List<string> States { get; private set; } = new List<string>();

		public void Refresh(DataFlow deviceType = DataFlow.All, DeviceState deviceState = DeviceState.Active)
		{
			var enumerator = new MMDeviceEnumerator();
			var devices = enumerator.EnumerateAudioEndPoints(deviceType, deviceState).ToArray();

			Devices = devices.ToList();
			Names = devices.Select(d => d.FriendlyName).ToList();
			States = devices.Select(d => d.State.ToString()).ToList();
		}
	}
}

