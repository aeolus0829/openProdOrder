using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace nsTblPrcs
{
    /// <summary>
    /// Summary description for tblPrcs
    /// </summary>
    public class tblPrcs
    {
        // public string connectionString = "Data Source=192.168.0.16;Initial Catalog=jw_dbfdb;Uid=jw;Pwd=jw;";
        //public string connectionString = "Data Source=192.168.0.16;Initial Catalog=PRD;User ID=archer;Password=ko123vr4";
        public string connectionString = "Data Source=192.168.0.25;Initial Catalog=UOF;User ID=SA;Password=Jinud-98A";

        public bool result = false;

        public DataTable getData(SqlCommand comm)
        {
            SqlConnection conn;
            SqlDataReader dr;
            DataTable dt = new DataTable();

            conn = new SqlConnection(connectionString);
            comm.Connection = conn;
            try
            {
                conn.Open();
                dr = comm.ExecuteReader();
                if (dr.HasRows) dt.Load(dr);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("getIncomingMaterial() error. 查詢的資料在取回時出了問題<br />");
                HttpContext.Current.Response.Write(comm.CommandText);
                HttpContext.Current.Response.Write("<br />");
                HttpContext.Current.Response.Write(ex);
                /* debug*/
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        protected void alertMsg(bool result,string data)
        {
            Literal rslt = new Literal();
            if (result==false)
            {
                rslt.Text = "<script>alert('"+data+"失敗')</script>";
            }
            else
            {
                rslt.Text = "<script>alert('"+data+"成功')</script>";
            }
            //新增畫面控制項：alert()
            Page page = (Page)HttpContext.Current.Handler;
            page.Controls.Add(rslt);
        }


    } //    public class tblPrcs
}
