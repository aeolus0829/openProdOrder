using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using nsTblPrcs;

namespace nsSubPrcs
{
    /// <summary>
    /// Summary description for subPrcs
    /// </summary>
    public class subPrcs
    {
        tblPrcs tp = new tblPrcs();

        public void sbAryls(ArrayList tmpSub2Rcv)
        {
            foreach (string proRaw in tmpSub2Rcv)
            {
                string[] proRaws = proRaw.Split(',');
                string itmNbr = proRaws[0].ToString().Trim();
                string pronbr = proRaws[1].ToString().Trim();
                string cost = sbCost(itmNbr, pronbr);
                string updSub2RcvCostSql = "insert into tmpSub2RcvCost values('" + itmNbr + "','" + pronbr + "','" + cost + "')";
                bool updRslt = tp.execSql(updSub2RcvCostSql);
            }
        }

        public string sbCost(string itmNbr, string pronbr)
        {
            float fltProCost = 0;
            float fltMaterialCost = 0;
            string qrySub2RcvSql = "select pro_nbr from inv_pps where item_nbr = '"+itmNbr+"'";
            ArrayList alCost = tp.getAryLs(qrySub2RcvSql);
            ArrayList alMaterialCost = tp.getAryLs("select matl_cost from inv_itm where item_nbr ='"+itmNbr+"'");
            fltMaterialCost = Convert.ToSingle(alMaterialCost[0]);
            int alCostIndx = alCost.IndexOf(pronbr);
            //string tmpSbCst = "current Index is : " + alCostIndx.ToString();
            if (alCostIndx == 0)
            {
                return alMaterialCost[0].ToString().Trim();
            }
            else if (alCostIndx!=-1 && alCostIndx>0)
            {                
                ArrayList alProCost = tp.getAryLs("select pro_cost from inv_pps where item_nbr='"+itmNbr+"'");
                for (int i = 0; i < alCostIndx; i++)
                {                 
                    float fltUntProCost = Convert.ToSingle(alProCost[i]);
                    fltProCost += fltUntProCost;
                }
            }
            fltProCost += fltMaterialCost;
            return fltProCost.ToString();
        }
    }
}