using System;
using nsTblPrcs;
using nsCmnPrcs;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    tblPrcs tp = new tblPrcs();
    cmnPrcs cp = new cmnPrcs();
    bool isFormEnabled = true;
    public string strSQL { get; set; }
    public List<string> vwCond { get; set; }
    public List<string> tbCond { get; set; }



    protected void Page_Load(object sender, EventArgs e)
    {
        if (! isFormEnabled)
        {
            Response.Redirect("disabled.html");
        }
        else {
            if (!IsPostBack) clrBtn();
        }

        strSQL = @"
            select DOC_NBR as 'BPM單號',
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
    }

    protected void btnClr_Click(object sender, EventArgs e)
    {
        clrBtn();
    }
    protected void clrBtn()
    {
        txtBpmBeginS.Text="";
        txtBpmBeginE.Text = "";
        txtBpmNo.Text="";
        txtBpmPo.Text="";
        txtMatnr.Text="";
        txtVndrNm.Text = "";
        ddlQA.SelectedIndex = 0;
        rdblQA.SelectedIndex = 0;
        ddlMvt.SelectedIndex = 0;
        Session.Abandon();
    }
    protected void btnSubmt_Click(object sender, EventArgs e)
    {
        string today = DateTime.Today.ToShortDateString();
        string yesterday = DateTime.Today.AddDays(-1).ToShortDateString();
        string thisyear = today.Substring(0, 4);
        string beginS, beginE, endS, endE;
        string lastyear = Convert.ToString(Convert.ToInt16(thisyear) - 1);

        SqlCommand tbCmd = new SqlCommand();
        SqlCommand vwCmd = new SqlCommand();

        SqlCommand vwCmd1 = new SqlCommand();

        List<string> vwCond = new List<string>();
        List<string> tbCond = new List<string>();

        /*
        string todaySql1 = strSQL + " VW_ICM_Item WHERE BEGIN_TIME >= '2016/7/13 00:00' AND BEGIN_TIME <= '2016/7/13 23:59'";
        vwCmd1.CommandText = todaySql1;

        SqlConnection conn = new SqlConnection(tp.connectionString);
        SqlDataReader dr;
        vwCmd1.Connection = conn;
        conn.Open();
        dr = vwCmd1.ExecuteReader();
        gvDebug.DataSource = dr;
        gvDebug.DataBind();
        */

        beginS = procDateTimeFormat(txtBpmBeginS.Text, " 00:00");
        if (txtBpmBeginE.Text=="") beginE = procDateTimeFormat(txtBpmBeginS.Text, " 23:59");
        else beginE = procDateTimeFormat(txtBpmBeginE.Text, " 23:59");

        vwCond.Add("BEGIN_TIME >= @vwbgTimeStart");
        vwCmd.Parameters.AddWithValue("vwBgTimeStart", today + " 00:00");
        vwCond.Add("BEGIN_TIME <= @vwbgTimeEnd");
        vwCmd.Parameters.AddWithValue("vwBgTimeEnd", today + " 23:59");

        if (! string.IsNullOrEmpty(beginS))
        {            
            tbCond.Add("BEGIN_TIME>=@tbBgTimeStart");
            tbCmd.Parameters.Add("@tbBgTimeStart", SqlDbType.VarChar).Value=beginS;
        }
        else
        {
            string thisYearBegin = thisyear + "/06/01 00:00";
            tbCond.Add("BEGIN_TIME>=@tbBgTimeStart");
            tbCmd.Parameters.Add("@tbBgTimeStart", SqlDbType.VarChar).Value = thisYearBegin;
        }

        if (!string.IsNullOrEmpty(beginE))
        {
            tbCond.Add("BEGIN_TIME<=@tbBgTimeEnd");
            tbCmd.Parameters.Add("@tbBgTimeEnd", SqlDbType.VarChar).Value = beginE;
        }
        else
        {
            string yesterdayEnd = yesterday + " 23:59";
            tbCond.Add("BEGIN_TIME<=@tbBgTimeEnd");
            tbCmd.Parameters.Add("@tbBgTimeEnd", SqlDbType.VarChar).Value = yesterdayEnd;
        }

        if (! string.IsNullOrEmpty(endS))
        {
            tbCond.Add("END_TIME>=@edTimeStart");
            tbCmd.Parameters.Add("@edTimeStart", SqlDbType.VarChar).Value = endS;
            tbCond.Add("END_TIME<=@edTimeEnd");
            tbCmd.Parameters.Add("@edTimeEnd", SqlDbType.VarChar).Value = endE;
        }

        if (txtBpmNo.Text!= "")
        {
            vwCond.Add("DOC_NBR LIKE '%@docNbr%'");
            vwCmd.Parameters.Add("@docNbr", SqlDbType.VarChar).Value = txtBpmNo.Text.Trim();
            tbCond.Add("DOC_NBR LIKE '%@docNbr%'");
            tbCmd.Parameters.Add("@docNbr", SqlDbType.VarChar).Value = txtBpmNo.Text.Trim();
        }

        if (txtBpmPo.Text!= "")
        {
            vwCond.Add("PO LIKE '%@poNum%'");
            vwCmd.Parameters.Add("@poNum", SqlDbType.VarChar).Value = txtBpmPo.Text.Trim();
            tbCond.Add("PO LIKE '%@poNum%'");
            tbCmd.Parameters.Add("@poNum", SqlDbType.VarChar).Value = txtBpmPo.Text.Trim();
        }

        if (txtMtrlDocNbr.Text!= "")
        {
            vwCond.Add("MTRLDOC LIKE '%@mtrlDoc%'");
            vwCmd.Parameters.Add("@mtrlDoc", SqlDbType.VarChar).Value = txtMtrlDocNbr.Text.Trim();
            tbCond.Add("MTRLDOC LIKE '%@mtrlDoc%'");
            tbCmd.Parameters.Add("@mtrlDoc", SqlDbType.VarChar).Value = txtMtrlDocNbr.Text.Trim();
        }

        if (txtOrdMtrl.Text!= "")
        {
            vwCond.Add("MTRLDOC LIKE '%@ordMtrl%'");
            vwCmd.Parameters.Add("@ordMtrl", SqlDbType.VarChar).Value = txtOrdMtrl.Text.Trim();
            tbCond.Add("MTRLDOC LIKE '%@ordMtrl%'");
            tbCmd.Parameters.Add("@ordMtrl", SqlDbType.VarChar).Value = txtOrdMtrl.Text.Trim();
        }

        if (txtMatnr.Text!= "")
        {
            vwCond.Add("MATERIAL LIKE '%@matnr%'");
            vwCmd.Parameters.Add("@matnr", SqlDbType.VarChar).Value = txtMatnr.Text.Trim();
            tbCond.Add("MATERIAL LIKE '%@matnr%'");
            tbCmd.Parameters.Add("@matnr", SqlDbType.VarChar).Value = txtMatnr.Text.Trim();
        }

        if (txtVndrNm.Text!= "")
        {
            vwCond.Add("VENDOR_NAME LIKE '%@vendorNm%'");
            vwCmd.Parameters.Add("@vendorNm", SqlDbType.VarChar).Value = txtVndrNm.Text.Trim();
            tbCond.Add("VENDOR_NAME LIKE '%@vendorNm%'");
            tbCmd.Parameters.Add("@vendorNm", SqlDbType.VarChar).Value = txtVndrNm.Text.Trim();
        }

        if (ddlMvt.SelectedIndex!=0)
        {
            vwCond.Add("MOVE_TYPE LIKE '%@mvT%'");
            vwCmd.Parameters.Add("@mvT", SqlDbType.VarChar).Value = ddlMvt.SelectedValue;
            tbCond.Add("MOVE_TYPE LIKE '%@mvT%'");
            tbCmd.Parameters.Add("@mvT", SqlDbType.VarChar).Value = ddlMvt.SelectedValue;
        }

        if (rdblQA.SelectedIndex!=0)
        {
            vwCond.Add("QAresult LIKE '%@QAResult%'");
            vwCmd.Parameters.Add("@QAResult", SqlDbType.VarChar).Value = rdblQA.SelectedValue;
            tbCond.Add("QAresult LIKE '%@QAResult%'");
            tbCmd.Parameters.Add("@QAResult", SqlDbType.VarChar).Value = rdblQA.SelectedValue;

        }

        if (rbSample.SelectedValue == "Y")
        {
            vwCond.Add("RDpersen <> ''");
            tbCond.Add("RDpersen <> ''");
        }

        if (ddlQA.SelectedIndex!=0)
        {
            vwCond.Add(ddlQA.SelectedValue);
            tbCond.Add(ddlQA.SelectedValue);
        }

        string todaySql = strSQL + " VW_ICM_Item{0}{1}";
        string oldSql = strSQL + " TB_ICM_Item{0}{1}";

        vwCmd.CommandText = procSql(todaySql, vwCond);
        tbCmd.CommandText = procSql(oldSql, tbCond);

        DataTable vwDt = tp.getIncomingMaterial(vwCmd);
        DataTable tbDt = tp.getIncomingMaterial(tbCmd);

        tbDt.Merge(vwDt,true);

        tbDt.DefaultView.Sort = "BPM單號 ASC";

        gvDebug.DataSource = tbDt;
        gvDebug.DataBind();

        //Response.Redirect("result.aspx");
    }

    private string procSql(string stringSql, List<string> vwCond)
    {
        string sql = string.Format(
            stringSql,
            vwCond.Count > 0 ? " WHERE " : "",
            string.Join(" AND ", vwCond.ToArray())
            );
        return sql;

    }

    private string procDateTimeFormat(string strDate, string strTime)
    {
        if (!string.IsNullOrEmpty(strDate))
        {
            strDate = cp.convertToDatef(strDate.Trim());
            strDate += strTime;
        }
        else
        {
            strDate = null;
        }
        return strDate;
    }

    protected void ddlStorelocation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txtBpmBeginE_TextChanged(object sender, EventArgs e)
    {
        
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        
    }
    protected void rdblQA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

}