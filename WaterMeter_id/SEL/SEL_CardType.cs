using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_CardType
    {
        DAL_CardType CardType_Obj = new DAL_CardType();
        public bool Add_CardType(BLL_CardType dataSt)
        {
          return  CardType_Obj.Insert(dataSt);
        }
        public bool Update_CardType(BLL_CardType dataSt)
        {
            return CardType_Obj.Update(dataSt);
        }
        public bool Delet_CardType(BLL_CardType dataSt)
        {
            return CardType_Obj.Delete(dataSt);
        }
        public DataTable Get_CardTypeTable()
        {
            return CardType_Obj.Select();
        }

        public DataTable GetSelectedCardType(string cardType)
        {
            return CardType_Obj.SearchId(cardType);
        }

    }
}
