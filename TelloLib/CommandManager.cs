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

        private void AssertConnected()
        {
            if (!_communicationManager.IsConnected())
            {
                throw new Exception("You have to be connected to Tello to send it commands !");
            }
        }
    }
}
