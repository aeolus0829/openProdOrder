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
        //public string connectionString = "Data Source=192.168.0.25;Initial Catalog=UOF;User ID=SA;Password=Jinud-98A";
        public string connectionString;
        public bool result = false;

        public DataTable mkTbl(DataTable dt,string[] dtClmnHdrs, ArrayList dataRslts)
        {       
            //DataTable dt = new DataTable(); 
            //ArrayList finalTbl = new ArrayList();
            
            string[] dataRsltRows= null ;
            
            for (int i = 0; i < dtClmnHdrs.Count(); i++)
            { 
                dt.Columns.Add(dtClmnHdrs[i]);
            }

            foreach (string dataRslt in dataRslts)
            {
                dataRsltRows = dataRslt.Split(',');

                DataRow row = dt.NewRow();
                for (int i = 0; i < dataRsltRows.Count(); i++)
                {
                    row[dtClmnHdrs[i]] = dataRsltRows[i];
                }            
                dt.Rows.Add(row);
            }
            return dt;
        }

        public void crtTbl(string tblNm)
        {
            string sql = null;
            switch (tblNm)
            {
                case "tmpOrdr":
                    sql = "create table tmpOrdr(ordrNbr char(20),itmNbr char(20),cusAlias char(20),ordrQty char(20),plnDt date, status char(20))";
                break;
                case "tmpBom":
                    sql = "create table tmpBom(ordrNbr char(20),itmNbr char(20),itmNm char(30),unitNm char(10),useQty char(10),qty char(20))";
                break; 
                case "tmpNtInQty":
                    sql = "create table tmpNtInQty(ordrNbr char(20),itmNbr char(20),purNtIn char(20),subNtIn char(20))";
                break;
                case "tmpStkQty":
                    sql = "create table tmpStkQty (ordrNbr char(20),itmNbr char(20),stkQty char(20))";
                break;
                    /*
                case "tmpShrtQty":
                    sql = "create table tmpShrtQty (itmNbr char(20),stkQty char(20))";
                break;
                     */
            }
            result = execSql(sql);
            //alertMsg(result,tblNm+" 資料表建立");
        }

        public void drpTbl(string tblNm)
        {
            string sql = "IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tblNm + "' AND TABLE_SCHEMA = 'dbo') BEGIN DROP TABLE dbo." + tblNm + " END";
            result = execSql(sql);
            //alertMsg(result, tblNm+" 資料表刪除");
        }

        public void trnctTbl(string tblNm)
        {
            string sql = "IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tblNm + "' AND TABLE_SCHEMA = 'dbo') BEGIN truncate TABLE dbo." + tblNm + " END";
            result = execSql(sql);
            //alertMsg(result, tblNm+" 資料表清空");
        }

        public string insrtData(string tblNm, ArrayList aryDatas,string ordrNbr)
        {
            string[] rowData = null;
            string sql = null;

            foreach (string aryData in aryDatas)
            {                
                if (tblNm=="tmpBom") sql += "insert into " + tblNm.Trim() + " values('"+ordrNbr+"','";
                else sql += "insert into " + tblNm.Trim() + " values('";                
                
                rowData = aryData.ToString().Split(',');
                for (int i = 0; i < rowData.Count(); i++)
                {
                    if (i<(rowData.Count()-1)) sql+=rowData[i]+"','";                    
                    else sql += rowData[i] + "');";
                }
                //execSql(sql);
            }            
            //alertMsg(result,tblNm + " 資料新增");
            return sql;
        }

        public string updtData(string tblNm, ArrayList aryDatas, Array aryHdrs)
        {
            string[] rowData = null;
            string[] hdrData = null;
            string sql = null;

            foreach (string aryData in aryDatas)
            {
                sql += "update " + tblNm.Trim() + " set ";

                rowData = aryData.ToString().Split(',');
                hdrData = aryHdrs.ToString().Split(',');

                for (int i = 0; i < rowData.Count(); i++)
                {
                    if (i < (rowData.Count() - 1)) sql += hdrData[i]+"='"+rowData[i] + "',";
                    else sql += rowData[i] + "';";
                }
            }
            //alertMsg(result,tblNm + " 資料新增");
            return sql;
        }

        public bool execSql(string strSQL)
        {
            SqlConnection conn;
            SqlCommand comm;            
            ArrayList results = new ArrayList();
            bool result = false;
            int affectRows = 0;

            conn = new SqlConnection(connectionString);
            comm = new SqlCommand(strSQL, conn);
            try
            {                
                conn.Open();
                affectRows = comm.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex + " 錯誤<br/>");
                HttpContext.Current.Response.Write("<ul><li>請回前一頁檢查輸入條件有沒有問題</li>");
                HttpContext.Current.Response.Write("<li>本頁請不要按[重新整理]</li>");
                //Response.Write(strSQL + "<br/>");
            }
            finally
            {
                conn.Close();
            }
            if ((affectRows == -1 ) || (affectRows>0)) result = true;
            else result = false;
            return result;
        }

        public DataSet qrySql(string strSQL)
        {
            SqlConnection conn;
            SqlCommand comm;
            //SqlDataReader rdr=null;

            //DataTable dt = new DataTable();            
            DataSet ds = new DataSet();

            conn = new SqlConnection(connectionString);
            comm = new SqlCommand(strSQL, conn);

            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(comm);
                /*
                da.Fill(ds, "WKF");
                DataTable dt = ds.Tables["WKF"];
                ds.Tables.Add(dt);
                DataTable prDt = new DataTable();
                ArrayList alRows;
                //string doc_nbr, fromDoc_nbr, mtrlDoc;

                foreach (DataRow row in dt.Rows)
                {
                    /*
                    doc_nbr = row[0].ToString();
                    fromDoc_nbr = row[1].ToString();
                    mtrlDoc = row[7].ToString();
                     

                    for (int i = 0; i < row.Table.Columns.Count; i++)
                    { 
                        switch (i);
                        {
                            case 0:
                            break;
                            case 1:
                            break;
                            case 7:
                            break;
                            default:
                        }                        
                    }

                }
                /*
                for (int i = 0; i < ds.Tables["WKF"].Rows.Count; i++)
                {
                    //strUserID = ObjDS.Tables["RegInfo"].Rows[i]["UserID"].ToString();//資料一筆一筆存入StrUserID
                    


                }
            */
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex + " 錯誤<br/>");
                HttpContext.Current.Response.Write("<ul><li>請回前一頁檢查輸入條件有沒有問題</li>");
                HttpContext.Current.Response.Write("<li>本頁請不要按[重新整理]</li>");
                //Response.Write(strSQL + "<br/>");
            }
            finally
            {
                conn.Close();
            }
            return ds;
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

        public ArrayList getAryLs(string strSQL)
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
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex + " 錯誤<br/>");
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
