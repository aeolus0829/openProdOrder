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
    string strSQL, beginS, beginE, today, yesterday, thisyear, lastyear, dayBefore3Month, todaySql, oldSql, inputDate;
    SqlCommand tbCmd, vwCmd;                                                             
    List<string> vwCond, tbCond;
    System.Diagnostics.Stopwatch sw;

    protected void Page_Load(object sender, EventArgs e)
    {
        today = DateTime.Today.ToShortDateString();
        yesterday = DateTime.Today.AddDays(-1).ToShortDateString();
        thisyear = today.Substring(0, 4);
        lastyear = Convert.ToString(Convert.ToInt16(thisyear) - 1);
        dayBefore3Month = DateTime.Now.AddMonths(-3).ToShortDateString();

        sw = new System.Diagnostics.Stopwatch();

        if (! isFormEnabled)
        {
            Response.Redirect("disabled.html");
        }
        else {
            if (!IsPostBack) clrBtn();
        }        
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
        btnToExcel.Visible = false;
        ddlQA.SelectedIndex = 0;
        rdblQA.SelectedIndex = 0;
        ddlMvt.SelectedIndex = 0;
        gvResult.Dispose();
        gvResult.Visible = false;
        Session.Abandon();
    }
    protected void btnSubmt_Click(object sender, EventArgs e)
    {
        Trace.Write("before buildSqlWithCondition()");
        DataTable dtReslt = geneDtWithSqlCondition();
        Trace.Write("end of buildSqlWithCondition()");

        try
        {
            if (dtReslt != null)
            {
                btnToExcel.Visible = true;
                gvResult.Visible = true;

                dtReslt.DefaultView.Sort = "BPM單號 ASC";

                gvResult.DataSource = dtReslt;
                Trace.Write("before gv bind");
                gvResult.DataBind();
                Trace.Write("end of gv bind");
            }
            else Response.Write("依條件查詢，無任何資料");
        }
        catch (Exception)
        {
            Response.Write("依條件查詢出現錯誤");
        }

        //Response.Redirect("result.aspx");
    }

    private DataTable geneDtWithSqlCondition()
    {
        tbCmd = new SqlCommand();
        vwCmd = new SqlCommand();
        List<string> vwCond = new List<string>();
        List<string> tbCond = new List<string>();
        DataTable dtMain = new DataTable();
        DataTable vwDt = new DataTable();

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

        beginS = bindDayTime(txtBpmBeginS.Text, " 00:00");

        if (!string.IsNullOrEmpty(beginS)) inputDate = cp.formatDateWithDash(txtBpmBeginS.Text);

        if (txtBpmBeginE.Text == "") beginE = bindDayTime(txtBpmBeginS.Text, " 23:59");
        else beginE = bindDayTime(txtBpmBeginE.Text, " 23:59");

        vwCond.Add("BEGIN_TIME >= @vwbgTimeStart");
        vwCmd.Parameters.AddWithValue("vwBgTimeStart", today + " 00:00");
        vwCond.Add("BEGIN_TIME <= @vwbgTimeEnd");
        vwCmd.Parameters.AddWithValue("vwBgTimeEnd", today + " 23:59");


        if (!string.IsNullOrEmpty(beginS))
        {
            tbCond.Add("BEGIN_TIME>=@tbBgTimeStart");
            tbCmd.Parameters.Add("@tbBgTimeStart", SqlDbType.VarChar).Value = beginS;
        }
        else
        {
            tbCond.Add("BEGIN_TIME>=@tbBgTimeStart");
            tbCmd.Parameters.Add("@tbBgTimeStart", SqlDbType.VarChar).Value = dayBefore3Month + " 00:00";
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

        if (txtBpmNo.Text != "")
        {
            vwCond.Add("DOC_NBR LIKE '%@docNbr%'");
            vwCmd.Parameters.Add("@docNbr", SqlDbType.VarChar).Value = txtBpmNo.Text.Trim();
            tbCond.Add("DOC_NBR LIKE '%@docNbr%'");
            tbCmd.Parameters.Add("@docNbr", SqlDbType.VarChar).Value = txtBpmNo.Text.Trim();
        }

        if (txtBpmPo.Text != "")
        {
            vwCond.Add("PO LIKE '%@poNum%'");
            vwCmd.Parameters.Add("@poNum", SqlDbType.VarChar).Value = txtBpmPo.Text.Trim();
            tbCond.Add("PO LIKE '%@poNum%'");
            tbCmd.Parameters.Add("@poNum", SqlDbType.VarChar).Value = txtBpmPo.Text.Trim();
        }

        if (txtMtrlDocNbr.Text != "")
        {
            vwCond.Add("MTRLDOC LIKE '%@mtrlDoc%'");
            vwCmd.Parameters.Add("@mtrlDoc", SqlDbType.VarChar).Value = txtMtrlDocNbr.Text.Trim();
            tbCond.Add("MTRLDOC LIKE '%@mtrlDoc%'");
            tbCmd.Parameters.Add("@mtrlDoc", SqlDbType.VarChar).Value = txtMtrlDocNbr.Text.Trim();
        }

        if (txtOrdMtrl.Text != "")
        {
            vwCond.Add("MTRLDOC LIKE '%@ordMtrl%'");
            vwCmd.Parameters.Add("@ordMtrl", SqlDbType.VarChar).Value = txtOrdMtrl.Text.Trim();
            tbCond.Add("MTRLDOC LIKE '%@ordMtrl%'");
            tbCmd.Parameters.Add("@ordMtrl", SqlDbType.VarChar).Value = txtOrdMtrl.Text.Trim();
        }

        if (txtMatnr.Text != "")
        {
            vwCond.Add("MATERIAL LIKE '%@matnr%'");
            vwCmd.Parameters.Add("@matnr", SqlDbType.VarChar).Value = txtMatnr.Text.Trim();
            tbCond.Add("MATERIAL LIKE '%@matnr%'");
            tbCmd.Parameters.Add("@matnr", SqlDbType.VarChar).Value = txtMatnr.Text.Trim();
        }

        if (txtVndrNm.Text != "")
        {
            vwCond.Add("VENDOR_NAME LIKE '%@vendorNm%'");
            vwCmd.Parameters.Add("@vendorNm", SqlDbType.VarChar).Value = txtVndrNm.Text.Trim();
            tbCond.Add("VENDOR_NAME LIKE '%@vendorNm%'");
            tbCmd.Parameters.Add("@vendorNm", SqlDbType.VarChar).Value = txtVndrNm.Text.Trim();
        }

        if (ddlMvt.SelectedIndex != 0)
        {
            vwCond.Add("MOVE_TYPE LIKE '%@mvT%'");
            vwCmd.Parameters.Add("@mvT", SqlDbType.VarChar).Value = ddlMvt.SelectedValue;
            tbCond.Add("MOVE_TYPE LIKE '%@mvT%'");
            tbCmd.Parameters.Add("@mvT", SqlDbType.VarChar).Value = ddlMvt.SelectedValue;
        }

        if (rdblQA.SelectedIndex != 0)
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

        if (ddlQA.SelectedIndex != 0)
        {
            vwCond.Add(ddlQA.SelectedValue);
            tbCond.Add(ddlQA.SelectedValue);
        }

        todaySql = strSQL + " VW_ICM_Item{0}{1}";
        oldSql = strSQL + " TB_ICM_Item{0}{1}";

        vwCmd.CommandText = bindContition(todaySql, vwCond);
        tbCmd.CommandText = bindContition(oldSql, tbCond);

        if (today == inputDate)
        {
            dtMain = tp.getIncomingMaterial(vwCmd);
        }
        else
        {
            vwDt = tp.getIncomingMaterial(vwCmd);
            dtMain = tp.getIncomingMaterial(tbCmd);

            dtMain.Merge(vwDt);
        }

        return dtMain;
    }

    private string bindContition(string stringSql, List<string> vwCond)
    {
        string sql = string.Format(
            stringSql,
            vwCond.Count > 0 ? " WHERE " : "",
            string.Join(" AND ", vwCond.ToArray())
            );
        return sql;

    }

    private string bindDayTime(string strDate, string strTime)
    {
        if (!string.IsNullOrEmpty(strDate))
        {
            strDate = cp.formatDateWithDash(strDate.Trim());
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
    protected void rdblQA_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        cp.ExportToExcel(gvResult);
    }
}