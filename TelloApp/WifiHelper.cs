using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace TelloApp
{
    class WifiHelper
    {
        private WiFiAdapter WiFiAdapter { get; set; }
        private string TelloSSID { get; }
        private string NetworkSSID { get; }

        public WifiHelper(string telloSSID, string networkSSID)
        {
            TelloSSID = telloSSID;
            NetworkSSID = networkSSID;
        }

        private async Task InitializeFirstAdapter()
        {
            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                throw new Exception("WiFiAccessStatus not allowed");
            }
            else
            {
                var wifiAdapterResults =
                  await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

                if (wifiAdapterResults.Count >= 1)
                {
                    this.WiFiAdapter =
                      await WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);
                }
                else
                {
                    throw new Exception("WiFi Adapter not found.");
                }
            }
        }

        private async Task ScanForNetworks()
        {
            if (this.WiFiAdapter != null)
            {
                await this.WiFiAdapter.ScanAsync();
            }
        }

        async public Task ConnectToTelloWifi()
        {
            await InitializeFirstAdapter();
            WiFiAdapter.Disconnect();
            await ScanForNetworks();

            var report = WiFiAdapter.NetworkReport;

            var telloNetwork = report.AvailableNetworks.First(availableNetwork => availableNetwork.Ssid.Equals(TelloSSID));

            var connectionResult = await WiFiAdapter.ConnectAsync(telloNetwork, WiFiReconnectionKind.Manual);
            if (connectionResult.ConnectionStatus == WiFiConnectionStatus.Success)
            {
                Debug.WriteLine("Connected to Tello network with success");
            }
            else
            {
                Debug.WriteLine("Failed to connect to Tello network : " + connectionResult.ConnectionStatus.ToString());
            }
        }

        public void DisconnectFromTelloWifi()
        {
            WiFiAdapter.Disconnect();
            //await ScanForNetworks();

            //var report = WiFiAdapter.NetworkReport;

            //var sfrNetwork = report.AvailableNetworks.First(availableNetwork => availableNetwork.Ssid.Equals(SfrSsid));
            //WiFiAdapter.ConnectAsync(sfrNetwork, WiFiReconnectionKind.Manual);

            //var connectionResult = await WiFiAdapter.ConnectAsync(sfrNetwork, WiFiReconnectionKind.Manual);

            //if (connectionResult.ConnectionStatus == WiFiConnectionStatus.Success)
            //{
            //    Debug.WriteLine("Connected to SFR network with success");
            //}
            //else
            //{
            //    Debug.WriteLine("Failed to reconnect to SFR network : " + connectionResult.ConnectionStatus.ToString());
            //}
        }

    }
}
