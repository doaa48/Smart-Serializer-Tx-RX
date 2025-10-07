using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_Configurations
    {
        public int Configuration_ID { get; set; }
        public int Confuguration_TryToRemoveTheMeterCover { get; set; }
        public int Confuguration_PlaceTheMeterInMagneticField { get; set; }
        public int Confuguration_RemoveTheBattery { get; set; }
        public int Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit { get; set; }
        public int Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening { get; set; }

        public int Confuguration_OfftimeID { get; set; }

        public int Confuguration_PriceschdulerID { get; set; }
        public int Confuguration_ClientMeterAction { get; set; }
        public int Confuguration_ClientMaxOverDraft { get; set; }
        public int Confuguration_ClientOffWarningLimit { get; set; }
        public int Confuguration_EnableVavePeriod { get; set; }

        public string Confuguration_OfftimeName { get; set; }

        public string Confuguration_PriceschdulerName { get; set; }

    }
}
