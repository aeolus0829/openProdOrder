using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using nsTblPrcs;
using nsCmnPrcs;
using System.Data.SqlClient;
using System.Collections.Generic;

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

        //bindGv(dt);
    }

    private void bindGv(DataTable dt)
    {
        DataSet ds = new DataSet();

        try
        {
            if (dt.Rows.Count == 0)
            {
                btnToExcel.Visible = false;
                hlQry0.Visible = false;
            }
            else {
                //ds.Tables.Add(dt);
                gvList.DataSource = dt;
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