using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using nsTblPrcs;
using nsCmnPrcs;
        
public partial class result : System.Web.UI.Page
{
    
    tblPrcs tp = new tblPrcs();
    cmnPrcs cp = new cmnPrcs();

    protected void Page_Load(object sender, EventArgs e)
    {   
        //接收來自Default.aspx字串
        string Result_connectDB = Session["connectDB"] as string;
        //呼叫 tblPrcs tp 程式裡的 string connectionString
        //將connectDB 放置 connectionString
        tp.connectionString = Result_connectDB;

        btnToExcel.Visible = true;
        hlQry0.Visible = true;

        //取得本月份的第一天，例：2014/7/1
        DateTime fd = cp.firstDayOfMonth();
        //取得本月份的最後一天，例：2014/7/31
        DateTime ld = cp.lastDayOfMonth();

        /* 與下行的差異是來源單號、物料文件兩個欄位
         * string strSQL = "select DOC_NBR as 'BPM單號',fromDOC as '來源單號',convert(varchar,BEGIN_TIME,111) as 'BPM起單日',convert(varchar,BEGIN_TIME,108) as 'BPM起單時',case task_status when '1' then '未簽' when '2' then '結案' when '4' then '退簽' end as '簽核狀態', SUB_FLOW_NAME AS '站點',excpMIt as 特採狀態, mtrlDOC as '物料文件',MOVE_TYPE as '異動類型', PO as '採購單號', POITEM as '採購項次', VENDOR_NAME as '供應商',MATERIAL as 'SAP料號',ORDERID as '工單號碼',ORD_MATERIAL as 'SAP工單料號',SHORT_TEXT as '短文',ENTRY_QNT as '收貨數',case PO_UNIT when 'ST' then 'PC' end as '單位', QAresult AS '檢驗結果' from VW_ICM_Item where (1=1)";
         */
        string strSQL = "select DOC_NBR as 'BPM單號',convert(varchar,BEGIN_TIME,111) as 'BPM起單日',convert(varchar,BEGIN_TIME,108) as 'BPM起單時',convert(varchar,END_TIME,111) as 'BPM結單日',convert(varchar,END_TIME,108) as 'BPM結單時',fromDOC as '來源單號', mtrlDOC as '物料文件',case task_status when '1' then '未簽' when '2' then '結案' when '4' then '退簽' end as '簽核狀態', SUB_FLOW_NAME AS '站點',QAresult AS '檢驗結果',RDpersen AS '樣品負責人', excpMIt as 特採狀態, MOVE_TYPE as '異動類型', PO as '採購單號', POITEM as '採購項次', VENDOR_NAME as '供應商',MATERIAL as '物料號碼',ORDERID as '工單號碼',ORD_MATERIAL as '工單料號',SHORT_TEXT as '短文',ENTRY_QNT as '收貨數',case PO_UNIT when 'ST' then 'PC' end as '單位' from VW_ICM_Item where (1=1)";
        if (Session["sqlFltr"] != null) strSQL += Session["sqlFltr"].ToString();
        else strSQL += " AND BEGIN_TIME >= '"+fd.ToShortDateString() +"00:00' AND BEGIN_TIME <= '"+ld.ToShortDateString()+"00:00'";
        strSQL += " AND ENTRY_QNT<>0";
        Session["sql"] = strSQL;

        //ArrayList alDS = tp.getAryLs(strSQL);
        dbInit(strSQL);
    }

    private void dbInit(string sql)
    {

        DataSet ds = tp.qrySql(sql);
        try
        {
            sql += " ORDER BY BPM單號";

            gvList.DataSource = ds;

            //lblDataCnt.Text = sql;
            //gvList.Sort("BPM單號", SortDirection.Ascending);            
            
            gvList.DataBind();
            
            if (gvList.Rows.Count == 0) {
                btnToExcel.Visible = false;
                hlQry0.Visible = false;
            }
            //lblDataCnt.Text = "查到的資料共 " + gvList.Rows.Count.ToString() + " 筆";
        }
        catch (Exception ex)
        { 
                HttpContext.Current.Response.Write(ex + " 錯誤<br/>");
                HttpContext.Current.Response.Write("<ul><li>請回前一頁檢查輸入條件有沒有問題</li>");
                HttpContext.Current.Response.Write("<li>本頁請不要按[重新整理]</li>");
                //Response.Write(sql + "<br/>");
            }
            finally
            {
                ds.Clear();
            
        }
    }
   
    protected void gvList_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sql = Session["sql"].ToString();
        sql += " order by " + e.SortExpression + " " + cp.GetSortDirection(e.SortExpression);
        dbInit(sql);
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        cp.ExportToExcel(gvList);
    }
}