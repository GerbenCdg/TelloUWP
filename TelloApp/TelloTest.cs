using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelloLib;
using System.Diagnostics;
using TelloLib.CustomEventArgs;

namespace TelloApp
{
    class TelloTest
    {
        private Tello Tello { get; set; }
        private WifiHelper WifiHelper { get; set; }

        async public void TestTello()
        {
            WifiHelper = new WifiHelper("TELLO-C53FBC", "SFR_6A30");

            Debug.WriteLine("Connecting to Tello network...");
            await WifiHelper.ConnectToTelloWifi();

            Tello = new Tello();

            Tello.CommunicationManager.ConnectionStatusHandler += ConnectionStatusHandler;
            

            Tello.FlightManager.Start();
            
        }

        public void StopTello()
        {
            Tello.FlightManager.Stop();
            WifiHelper.DisconnectFromTelloWifi();
        }

        private void ConnectionStatusHandler(object sender, ConnectionStatusEventArgs e)
        {
            switch (e.ConnectionStatus)
            {
                case ConnectionStatus.Disconnected:
                    break;
                case ConnectionStatus.Connecting:
                    Debug.WriteLine("Connecting ...");
                    break;
                case ConnectionStatus.Connected:
                    Debug.WriteLine("Connected");

                    try
                    {
                        Tello.CommandManager.TakeOff();
                        Debug.WriteLine("Taking off...");

                       // await Task.Delay(5000);

                        //Tello.CommandManager.Land();
                        //Debug.WriteLine("Landing...");
                    }
                    catch (Exception e1)
                    {
                        Debug.WriteLine(e1.Message);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
