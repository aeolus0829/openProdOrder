using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

namespace nsOrdrPrcs
{
    /// <summary>
    /// Summary description for ordrPrcs
    /// </summary>
    public class ordrPrcs
    {
        SqlConnection sqlConnection1 = new SqlConnection("Data Source=sbsdb;Initial Catalog=JW_DBFDB;Uid=jw;Pwd=jw");

        public ArrayList getPlnDt(string sqlFtr)
        {
            ArrayList results = new ArrayList();
            string strRow = null;

            string sql = "select distinct 單據號碼,產品編號,客戶代號,數量,CONVERT(varchar(100), 交貨日期, 23) AS 交貨日期,狀態 from 訂單出貨狀況 where 狀態!='本單已作廢' ";
            //訂單表頭.狀態 !='本單已結案'";
            if (!string.IsNullOrEmpty(sqlFtr)) sql += sqlFtr;
            SqlDataReader rdr;
            SqlCommand cmd = new SqlCommand(sql, sqlConnection1);

            try
            {
                sqlConnection1.Open();
                rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        for (int r = 0; r <= rdr.FieldCount; r++)
                        {
                            if (r < rdr.FieldCount - 1) strRow += rdr[r].ToString().Trim() + ",";
                            if (r == rdr.FieldCount - 1) strRow += rdr[r].ToString().Trim();
                        }
                        results.Add(strRow);
                        strRow = null;
                    }

                }
                else HttpContext.Current.Response.Write("依查詢條件找不到資料，請回上一頁重新輸入查詢條件!");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(sql+"<br/>");
                HttpContext.Current.Response.Write(ex);
            }
            finally
            {
                sqlConnection1.Close();
            }
            return results;
        }
    }
}