using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

namespace nsStkPrcs
{
    /// <summary>
    /// Summary description for stkPrcs
    /// </summary>
    public class stkPrcs
    {
        public string connectionString = "Data Source=SBSDB;Initial Catalog=jw_dbfdb;Uid=jw;Pwd=jw;";
        public string getPurNtInQty(string itmNbr)
        {
            string strSQL = "SELECT SUM(尚欠數量) as 未交量 from 採購收料狀況表 where (STATUS='NA' OR STATUS='IN') and 產品編號='"+itmNbr+"'";
            ArrayList purNtInQty = new ArrayList();

            purNtInQty = BindGrid(strSQL);

            return purNtInQty[0].ToString();
        }

        public string getSubNtInQty(string itmNbr)
        {
            string strSQL = "SELECT SUM(尚欠數量) as 未交量 from 加工收料明細表 where (STATUS='NA' OR STATUS='IN') and 產品編號='"+itmNbr+"'";
            ArrayList subNtInQty = new ArrayList();

            subNtInQty = BindGrid(strSQL);

            return subNtInQty[0].ToString();
        }

        public string getStkQty(string itmNbr)
        {
            string strSQL = "SELECT SUM(庫存數) as 庫存數 from 庫存狀況表 where (倉庫代號='A' OR 倉庫代號='B') AND 料號='"+itmNbr+"'";
            ArrayList stkQty = new ArrayList();
             
            stkQty = BindGrid(strSQL);

            return stkQty[0].ToString();
        }

        public ArrayList BindGrid(string strSQL)
        {
            SqlConnection conn;
            SqlCommand comm;
            SqlDataReader rdr;
            string strRow = null;
            ArrayList results = new ArrayList();

            conn = new SqlConnection(connectionString);
            comm = new SqlCommand(strSQL, conn);
            try
            {
                conn.Open();
                rdr = comm.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        for (int r = 0; r <= rdr.FieldCount; r++)
                        {                            
                            if (r < rdr.FieldCount - 1) strRow += rdr[r].ToString().Trim() + ",";
                            if (r == rdr.FieldCount - 1) strRow += rdr[r].ToString().Trim();
                        }
                        if (string.IsNullOrEmpty(strRow)) strRow = "0";
                        results.Add(strRow);
                        strRow = null;
                    }
                    rdr.Close();
                }
                else HttpContext.Current.Response.Write("依查詢條件找不到資料，請回上一頁重新輸入查詢條件!");
            }                
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex+" 錯誤<br/>");
                HttpContext.Current.Response.Write("<ul><li>請回前一頁檢查輸入條件有沒有問題</li>");
                HttpContext.Current.Response.Write("<li>本頁請不要按[重新整理]</li>");
                //Response.Write(strSQL + "<br/>");
            }
            finally
            {
                conn.Close();
            }
            return results;
        }

    }
}