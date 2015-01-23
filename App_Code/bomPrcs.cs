using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

/// <summary>
/// Summary description for bomPrcs
/// </summary>
namespace nsBom
{
    public class bomPrcs
    {
        public ArrayList getBomDtl(string pID, string proNbr,string useQty)
        {
            SqlConnection sqlConnection1 = new SqlConnection("Data Source=sbsdb;Initial Catalog=JW_DBFDB;Uid=jw;Pwd=jw");
            //string sql = "SELECT distinct * FROM bom_views WHERE 母件代號='" + pID.Trim() + "' AND 子件代號='" + cID.Trim() + "'";
            string sql = "SELECT distinct *,用量*"+useQty+" AS 數量 FROM bom_views WHERE 母件代號='" + pID.Trim() + "'";
            string strRslts = null;
            string rdrTmp = null;
            int qty = 0;
            if (!String.IsNullOrEmpty(proNbr)) sql += " AND 製程='" + proNbr.Trim() + "'";
            //else sql+=" AND 製程 is Null";
            ArrayList results = new ArrayList();
            SqlDataReader rdr;

            SqlCommand cmd = new SqlCommand(sql, sqlConnection1);

            sqlConnection1.Open();
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {

                    for (int r = 0; r < 7; r++)
                    {
                        //計算用量*數量=需求量
                        if (r == 4)
                        {
                            qty = Convert.ToInt32(rdr[4]);
                            string[] useQtyTmp = useQty.Split('.');
                            int intUseQty = Convert.ToInt32(useQtyTmp[0]);
                            rdrTmp = Convert.ToString(qty * Convert.ToInt32(intUseQty));
                            strRslts += qty.ToString() + ",";
                        }
                        if (r != 4 && r <= 6) strRslts += rdr[r].ToString().Trim() + ",";
                    }
                    strRslts += rdrTmp;
                    results.Add(strRslts);
                    strRslts = null;
                }
            }

            sqlConnection1.Close();

            return results;

        }

        public ArrayList getBom(string pID, string proNbr,string useQty)
        {
            int count = 0;
            string blevel = null, clevel = null, dlevel = null, elevel = null, finalTmp;

            ArrayList bresults = new ArrayList();
            ArrayList cresults = new ArrayList();
            ArrayList dresults = new ArrayList();
            ArrayList eresults = new ArrayList();

            ArrayList final = new ArrayList();

            bresults = getBomDtl(pID, proNbr,useQty);

            foreach (string bresult in bresults)
            {
                string[] bresultRows = new string[6];
                bresultRows = bresult.Split(',');
                cresults = getBomDtl(bresultRows[1], null,useQty);
                if (blevel == null) blevel = "2";
                //else blevel = "&nbsp";
                else blevel = " ";
                clevel = null; elevel = null;
                finalTmp = blevel + "-----" + ",";
                for (int b = 0; b < 8; b++)
                {
                    if (b <= 6) finalTmp += bresultRows[b] + ",";
                    if (b == 7) finalTmp += bresultRows[b];
                }
                final.Add(finalTmp);
                foreach (string cresult in cresults)
                {
                    string[] cresultRows = new string[6];
                    cresultRows = cresult.Split(',');
                    dresults = getBomDtl(cresultRows[1], null,useQty);
                    if (clevel == null) clevel = "3";
                    //else clevel = "&nbsp";
                    else clevel = " ";
                    blevel = null; dlevel = null;
                    finalTmp = clevel + "---" + ",";
                    for (int c = 0; c < 8; c++)
                    {
                        if (c <= 6) finalTmp += cresultRows[c] + ",";
                        if (c == 7) finalTmp += cresultRows[c];
                    }
                    final.Add(finalTmp);
                    foreach (string dresult in dresults)
                    {
                        string[] dresultRows = new string[6];
                        dresultRows = dresult.Split(',');
                        eresults = getBomDtl(dresultRows[1], null,useQty);
                        if (dlevel == null) dlevel = "4";
                        //else dlevel = "&nbsp";
                        else dlevel = " ";
                        clevel = null; elevel = null;
                        finalTmp = dlevel + "--" + ",";
                        for (int d = 0; d < 8; d++)
                        {
                            if (d <= 6) finalTmp += dresultRows[d] + ",";
                            if (d == 7) finalTmp += dresultRows[d];
                        }
                        final.Add(finalTmp);
                        foreach (string eresult in eresults)
                        {
                            string[] eresultRows = new string[6];
                            eresultRows = eresult.Split(',');
                            if (elevel == null) elevel = "5";
                            //else elevel = "&nbsp";
                            else elevel = " ";
                            blevel = null; clevel = null;
                            finalTmp = elevel + "-" + ",";
                            for (int e = 0; e < 8; e++)
                            {
                                if (e <= 6) finalTmp += eresultRows[e] + ",";
                                if (e == 7) finalTmp += eresultRows[e];
                            }
                            final.Add(finalTmp);
                            count++;
                        }
                        count++;
                    }
                    count++;
                }
                count++;
            }
            return final;
        }

        // 不計算 BOM 階層
        public ArrayList getSmplBom(string pID, string proNbr, string useQty)
        {
            int count = 0;
            //string blevel = null, clevel = null, dlevel = null, elevel = null, 
            string finalTmp=null;

            ArrayList bresults = new ArrayList();
            ArrayList cresults = new ArrayList();
            ArrayList dresults = new ArrayList();
            ArrayList eresults = new ArrayList();

            ArrayList final = new ArrayList();

            bresults = getSmplBomDtl(pID, proNbr, useQty);

            foreach (string bresult in bresults)
            {
                string[] bresultRows = new string[5];
                bresultRows = bresult.Split(',');
                cresults = getSmplBomDtl(bresultRows[0], null, useQty);
                for (int b = 0; b < 5; b++)
                {
                    if (b <= 3) finalTmp += bresultRows[b] + ",";
                    if (b == 4) finalTmp += bresultRows[b];
                }
                final.Add(finalTmp);
                finalTmp = null;
                /*
                foreach (string cresult in cresults)
                {
                    string[] cresultRows = new string[5];
                    cresultRows = cresult.Split(',');
                    dresults = getSmplBomDtl(cresultRows[0], null, useQty);
                    for (int c = 0; c < 5; c++)
                    {
                        if (c <= 3) finalTmp += cresultRows[c] + ",";
                        if (c == 4) finalTmp += cresultRows[c];
                    }
                    final.Add(finalTmp);
                    finalTmp = null;
                    
                    foreach (string dresult in dresults)
                    {
                        string[] dresultRows = new string[5];
                        dresultRows = dresult.Split(',');
                        eresults = getSmplBomDtl(dresultRows[0], null, useQty);

                        for (int d = 0; d < 5; d++)
                        {
                            if (d <= 3) finalTmp += dresultRows[d] + ",";
                            if (d == 4) finalTmp += dresultRows[d];
                        }
                        final.Add(finalTmp);
                        finalTmp = null;
                        foreach (string eresult in eresults)
                        {
                            string[] eresultRows = new string[5];
                            eresultRows = eresult.Split(',');
                            for (int e = 0; e < 5; e++)
                            {
                                if (e <= 3) finalTmp += eresultRows[e] + ",";
                                if (e == 4) finalTmp += eresultRows[e];
                            }
                            final.Add(finalTmp);
                            finalTmp = null;
                            count++;
                        }
                        count++;
                    }
                    count++;
                }*/
                count++;
            }
            return final;
        }

        public ArrayList getSmplBomDtl(string pID, string proNbr, string useQty)
        {
            SqlConnection sqlConnection1 = new SqlConnection("Data Source=sbsdb;Initial Catalog=JW_DBFDB;Uid=jw;Pwd=jw");
            //string sql = "SELECT distinct *,用量*" + useQty + " AS 數量 FROM bom_views WHERE 母件代號='" + pID.Trim() + "'";
            string sql = "Select distinct 子件代號,子件名稱,用量,單位,用量*" + useQty + " AS 數量 from bom_views WHERE 母件代號='" + pID.Trim() + "' AND 用料估算='1'";
            string strRslts = null;
            string rdrTmp = null;
            int qty = 0;
            if (!String.IsNullOrEmpty(proNbr)) sql += " AND 製程='" + proNbr.Trim() + "'";
            //else sql+=" AND 製程 is Null";
            ArrayList results = new ArrayList();
            SqlDataReader rdr;

            SqlCommand cmd = new SqlCommand(sql, sqlConnection1);

            sqlConnection1.Open();
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {

                    for (int r = 0; r < 5; r++)
                    {
                        //計算用量*數量=需求量
                        if (r == 4)
                        {
                            qty = Convert.ToInt32(rdr[2]);
                            string[] useQtyTmp = useQty.Split('.');
                            int intUseQty = Convert.ToInt32(useQtyTmp[0]);
                            rdrTmp = Convert.ToString(qty * Convert.ToInt32(intUseQty));
                            strRslts += qty.ToString() + ",";
                        }
                        if (r != 2 && r <= 3) strRslts += rdr[r].ToString().Trim() + ",";
                    }
                    strRslts += rdrTmp;
                    results.Add(strRslts);
                    strRslts = null;
                }
            }

            sqlConnection1.Close();

            return results;

        }

    }
}