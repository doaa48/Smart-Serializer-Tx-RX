using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterMeter_id.BLL;
using WaterMeter_id.DAL;

namespace WaterMeter_id
{
    public class SEL_Configurations
    {
        DAL_Configurations dAL_Configurations_obj = new DAL_Configurations();
        DAL_PriceSchduler DAL_PriceSchduler_Obj = new DAL_PriceSchduler();
        DAL_Offtimes DAL_Offtimes_Obj = new DAL_Offtimes();
        public BLL_Configurations bLL_Configurations_Data = new BLL_Configurations();
        public string[] GetPriceSchdulerNames()
        {
            return DAL_PriceSchduler_Obj.SelectPriceScheduleNames();
        }
        
        public string[] GetOFFTimeNames()
        {
            return DAL_Offtimes_Obj.SelectOFFTimeNames();
        }
        public bool GetConfigurations()
        {
            bool Status = false;
            bLL_Configurations_Data = dAL_Configurations_obj.Select();

            if (bLL_Configurations_Data != null)
            {
                Status= true;
            }

            else
            {
                bLL_Configurations_Data = new BLL_Configurations();

                bLL_Configurations_Data.Confuguration_TryToRemoveTheMeterCover = 0;
                bLL_Configurations_Data.Confuguration_PlaceTheMeterInMagneticField = 0;
                bLL_Configurations_Data.Confuguration_RemoveTheBattery = 0;
                bLL_Configurations_Data.Confuguration_InterruptionOfCommunicationBetweenMechanicalMeterAndControlUnit = 0;
                bLL_Configurations_Data.Confuguration_InterruptionOfCommunicationBetweenControlUnitAndMotor_ValveClosingAndOpening = 0;
                bLL_Configurations_Data.Confuguration_OfftimeID = DAL_Offtimes_Obj.GetMaxOFFTimeID();
                bLL_Configurations_Data.Confuguration_PriceschdulerID = DAL_PriceSchduler_Obj.GetMaxPriceScheduleID();
                bLL_Configurations_Data.Confuguration_ClientMeterAction = 0;
                bLL_Configurations_Data.Confuguration_ClientMaxOverDraft = 0;
                bLL_Configurations_Data.Confuguration_ClientOffWarningLimit = 0;
                bLL_Configurations_Data.Confuguration_EnableVavePeriod = 0;


                if (dAL_Configurations_obj.Insert(bLL_Configurations_Data))
                {
                    bLL_Configurations_Data = dAL_Configurations_obj.Select();
                    Status = true;
                }
            }
            return Status;
        }
        public bool UpdateData()
        {

            bLL_Configurations_Data.Confuguration_OfftimeID = DAL_Offtimes_Obj.GetOFFTimeID(bLL_Configurations_Data.Confuguration_OfftimeName);
            bLL_Configurations_Data.Confuguration_PriceschdulerID = DAL_PriceSchduler_Obj.GetPriceSchedulerID(bLL_Configurations_Data.Confuguration_PriceschdulerName);
            return dAL_Configurations_obj.Update(bLL_Configurations_Data); //blll
        }
    }
}
