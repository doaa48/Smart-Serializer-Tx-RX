using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterMeter_id;

namespace WaterMeter_id
{
    public class SEL_Getway
    {
        public BLL_Getway bll_Getway = new BLL_Getway();
        DAL_Getway dal_Getway_obj = new DAL_Getway();

        public DataTable GetTable()
        {
            return dal_Getway_obj.Select();
        }

        public bool AddNewGetway()
        {
            return dal_Getway_obj.Insert(bll_Getway);
        }

        public bool UpdateGetway()
        {
            return dal_Getway_obj.Update(bll_Getway);
        }

        public bool DeleteGetway()
        {
            return dal_Getway_obj.Delete(bll_Getway);
        }

        public bool GetwayByNum(int num)
        {
            bll_Getway = dal_Getway_obj.getGetwayData(num);

            if (bll_Getway == null)
            {
                return false;
            }
            return true;
        }
        public DataTable GetwayNum()
        {
            return dal_Getway_obj.GetwayNumColomnSelect();
        }



    }
}
