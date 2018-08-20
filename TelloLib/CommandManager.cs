using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelloLib
{
    public class CommandManager
    {
        private FlyState FlyState { get; }
        private readonly CommunicationManager _communicationManager;

        public CommandManager(FlyState flystate, CommunicationManager communicationManager)
        {
            FlyState = flystate;
            _communicationManager = communicationManager;
        }

        public void TakeOff()
        {
            AssertConnected();
            _communicationManager.SendCommand("takeoff");
            FlyState.IsFlying = true;
        }

        public void Land()
        {
            AssertConnected();
            _communicationManager.SendCommand("land");
            // TODO make this better (be able to know if its actually landing or if it has landed, make an enum : TakingOff, Landing, Landed, Flying, ...)
            FlyState.IsFlying = false;
        }

        public void Fly(FlightDirection direction, int distance = 20)
        {
            Utils.AssertValue(distance, 20, 500, "distance parameter can range from value 20 to 500 !");
            AssertConnected();
            string flightCommand;
            switch (direction)
            {
                case FlightDirection.Up:
                    flightCommand = "up";
                    break;
                case FlightDirection.Down:
                    flightCommand = "down";
                    break;
                case FlightDirection.Left:
                    flightCommand = "left";
                    break;
                case FlightDirection.Right:
                    flightCommand = "right";
                    break;
                case FlightDirection.Forward:
                    flightCommand = "forward";
                    break;
                case FlightDirection.Backward:
                    flightCommand = "back";
                    break;
                default:
                    throw new NotImplementedException("Make sure all the enum's possibilities are checked.");
            }
            _communicationManager.SendCommand(flightCommand + " " + distance);
        }

        public void Turn(bool clockWise, int degrees = 1)
        {
            Utils.AssertValue(degrees, 1, 3600, "degrees parameter can range from value 1 to 3600");
            AssertConnected();

            var turnCommand = clockWise ? "cw" : "ccw";
            _communicationManager.SendCommand(turnCommand + " " + degrees);
        }

        private void AssertConnected()
        {
            if (!_communicationManager.IsConnected())
            {
                throw new Exception("You have to be connected to Tello to send it commands !");
            }
        }

        
    }
}
