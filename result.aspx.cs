using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using nsTblPrcs;
using nsCmnPrcs;

public partial class result : System.Web.UI.Page
{
    tblPrcs tp = new tblPrcs();
    cmnPrcs cp = new cmnPrcs();

    public string dateFltr { get; set; }
    public string fltr { get; set; }
    public string lastYear { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        btnToExcel.Visible = true;
        hlQry0.Visible = true;

        DateTime firstDayOM = cp.firstDayOfMonth();
        DateTime lastDayOM = cp.lastDayOfMonth();

        dateFltr = Session["dateFltr"].ToString();

        if (Session["ftr"] == null) fltr = "";
        else fltr = Session["ftr"].ToString();

        if (Session["recent"] == null) lastYear = "";
        else lastYear = Session["recent"].ToString();

        string strSQL = @"select DOC_NBR as 'BPM單號',
            convert(varchar,BEGIN_TIME,111) as 'BPM起單日',
            convert(varchar,BEGIN_TIME,108) as 'BPM起單時',
            convert(varchar,END_TIME,111) as 'BPM結單日',
            convert(varchar,END_TIME,108) as 'BPM結單時',
            fromDOC as '來源單號', 
            mtrlDOC as '物料文件',
            case 
                task_status 
                    when '1' then '未簽' 
                    when '2' then '結案' 
                    when '4' then '退簽'
            end as '簽核狀態', 
            SUB_FLOW_NAME AS '站點',
            QAresult AS '檢驗結果',
            RDpersen AS '樣品負責人', 
            excpMIt as 特採狀態, 
            MOVE_TYPE as '異動類型', 
            PO as '採購單號', 
            POITEM as '採購項次', 
            VENDOR_NAME as '供應商',
            MATERIAL as '物料號碼',
            ORDERID as '工單號碼',
            ORD_MATERIAL as '工單料號',
            SHORT_TEXT as '短文',
            ENTRY_QNT as '收貨數',
            case 
                PO_UNIT 
                    when 'ST' then 'PC' 
            end as '單位' from";

        string todaySql = procSQLstring(strSQL, "today");
        string oldSql = procSQLstring(strSQL, "old");

        DataTable dt = tp.combineDt(todaySql, oldSql);

        bindGv(dt);
    }

    private string procSQLstring(string stringSql, string queryOption)
    {
        if (queryOption == "today")
        {
            stringSql += " VW_ICM_Item where (1=1)";
            stringSql += dateFltr;
            stringSql += fltr;
        }
        else
        {
            stringSql += " TB_ICM_Item where (1=1)";            
            stringSql += fltr;
            stringSql += lastYear;
        }

        stringSql += " AND ENTRY_QNT<>0";
        // Session["sql"] = strSQL;

        return stringSql;
    }

    private void bindGv(DataTable dt)
    {
        DataSet ds = new DataSet();
        //DataTable dt = new DataTable();

        try
        {
            if (dt.Rows.Count == 0)
            {
                btnToExcel.Visible = false;
                hlQry0.Visible = false;
            }
            else {
                ds.Tables.Add(dt);
                gvList.DataSource = ds;                
                gvList.DataBind();
            }
            //lblDataCnt.Text = sql;               
            lblDataCnt.Text = "查到的資料共 " + gvList.Rows.Count.ToString() + " 筆";
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
            dt.Dispose();
            ds.Dispose();
        }
    }
   
    protected void gvList_Sorting(object sender, GridViewSortEventArgs e)
    {
        /*
        string sql = Session["sql"].ToString();
        sql += " order by " + e.SortExpression + " " + cp.GetSortDirection(e.SortExpression);
        dbInit(sql);
        */
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        cp.ExportToExcel(gvList);
    }
}