using System;
using nsTblPrcs;
using nsCmnPrcs;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;


public partial class _Default : System.Web.UI.Page
{
    tblPrcs tp = new tblPrcs();
    cmnPrcs cp = new cmnPrcs();

    bool isFormEnabled = true;
    string strSQL, beginS, beginE, today, yesterday, thisyear, lastyear, dayBefore3Month, todaySql, oldSql, inputDate;
    SqlCommand tbCmd, vwCmd, Cmd;                                                             
    List<string> vwCond, tbCond, Cond;
    DataTable dtXlsx;
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
    }

    protected void btnClr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
    protected void btnSubmt_Click(object sender, EventArgs e)
    {
        DataTable dtReslt = geneDtWithSqlCondition();

        try
        {
            if (dtReslt.Columns.Count>0)
            {
                btnToExcel.Visible = true;
                gvResult.Visible = true;

                switch (rblStyle.SelectedIndex)
                {
                    case 0:
                        dtReslt.DefaultView.Sort = "BPM單號 ASC";
                        break;
                    case 1:
                        dtReslt.DefaultView.Sort = "BPM結單日,採購單號,採購項次 ASC";
                        break;
                }


                gvResult.DataSource = dtReslt;
                gvResult.DataBind();
            }
            else Response.Write("依條件查詢，無任何資料");
        }
        catch (Exception ex)
        {
            Response.Write("依條件查詢出現例外狀況 <p>" + ex);
        }

        //Response.Redirect("result.aspx");
    }

    private DataTable geneDtWithSqlCondition()
    {
        //tbCmd = new SqlCommand();
        //vwCmd = new SqlCommand();
        Cmd = new SqlCommand();
        List<string> Cond = new List<string>();
        DataTable pruneDt = new DataTable();
        DataTable mainDt = new DataTable();
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
            MD_MEMO as '物料文件備註',
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

        todaySql = strSQL + " VW_ICM_Item{0}{1}";
        oldSql = strSQL + " TB_ICM_Item{0}{1}";

        //檢查起單時間初始值
        beginS = bindDayTime(txtBpmBeginS.Text, " 00:00");
        if (txtBpmBeginE.Text == "") beginE = bindDayTime(txtBpmBeginS.Text, " 23:59");
        else beginE = bindDayTime(txtBpmBeginE.Text, " 23:59");

        //檢查各欄位是否有輸入值
        if (txtBpmNo.Text != "")
        {
            Cond.Add("DOC_NBR LIKE '%' + @docNbr + '%'");
            Cmd.Parameters.Add("@docNbr", SqlDbType.VarChar).Value = txtBpmNo.Text.Trim();
        }

        if (txtBpmPo.Text != "")
        {
            Cond.Add("PO LIKE '%' + @poNum + '%'");
            Cmd.Parameters.Add("@poNum", SqlDbType.VarChar).Value = txtBpmPo.Text.Trim();
        }

        if (txtMtrlDocNbr.Text != "")
        {
            Cond.Add("MTRLDOC LIKE '%' + @mtrlDoc + '%'");
            Cmd.Parameters.Add("@mtrlDoc", SqlDbType.VarChar).Value = txtMtrlDocNbr.Text.Trim();
        }

        if (txtOrdMtrl.Text != "")
        {
            Cond.Add("ORD_MATERIAL LIKE '%' + @ordMtrl + '%'");
            Cmd.Parameters.Add("@ordMtrl", SqlDbType.VarChar).Value = txtOrdMtrl.Text.Trim();
        }

        if (txtMatnr.Text != "")
        {
            Cond.Add("MATERIAL LIKE '%' + @matnr + '%'");
            Cmd.Parameters.Add("@matnr", SqlDbType.VarChar).Value = txtMatnr.Text.Trim();
        }

        if (txtVndrNm.Text != "")
        {
            Cond.Add("VENDOR_NAME LIKE '%' + @vendorNm + '%'");
            Cmd.Parameters.Add("@vendorNm", SqlDbType.VarChar).Value = txtVndrNm.Text.Trim();
        }

        if (ddlMvt.SelectedIndex != 0)
        {
            Cond.Add("MOVE_TYPE LIKE '%' + @mvT + '%'");
            Cmd.Parameters.Add("@mvT", SqlDbType.VarChar).Value = ddlMvt.SelectedValue;
        }

        if (rdblQA.SelectedIndex != 0)
        {
            Cond.Add("QAresult LIKE '%' + @QAResult + '%'");
            Cmd.Parameters.Add("@QAResult", SqlDbType.VarChar).Value = rdblQA.SelectedValue;
        }

        if (rbSample.SelectedValue == "Y")
        {
            Cond.Add("RDpersen <> ''");
        }

        if (ddlQA.SelectedIndex != 0)
        {
            Cond.Add(ddlQA.SelectedValue);
        }

        //將共用條件複製一份；省下一半的程式碼
        SqlCommand vwCmd = Cmd.Clone();
        SqlCommand tbCmd = Cmd.Clone();
        List<string> vwCond = new List<string>(Cond); 
        List<string> tbCond = new List<string>(Cond);

        //加速模式未啟用；雖然速度較慢，但保證資料是最新的
        //不過資料量大時，程式可能會噴 timeout exception
        if (!cbCacheMode.Checked) 
        {
            if (string.IsNullOrEmpty(beginS))
            {
                beginS = "2012/01/01 00:00";
                beginE = today + " 23:59";
            } 

            vwCond.Add("BEGIN_TIME>=@vwbgTimeStart");
            vwCmd.Parameters.Add("@vwbgTimeStart", SqlDbType.VarChar).Value = beginS;
            vwCond.Add("BEGIN_TIME<=@vwbgTimeEnd");
            vwCmd.Parameters.Add("@vwbgTimeEnd", SqlDbType.VarChar).Value = beginE;

            vwCmd.CommandText = bindContition(todaySql, vwCond);

            mainDt = tp.getData(vwCmd);
        }
        else //加速模式啟用
        {
            if (!string.IsNullOrEmpty(beginS))
            {
                tbCond.Add("BEGIN_TIME>=@tbBgTimeStart");
                tbCmd.Parameters.Add("@tbBgTimeStart", SqlDbType.VarChar).Value = beginS;
            }

            if (!string.IsNullOrEmpty(beginE))
            {
                tbCond.Add("BEGIN_TIME<=@tbBgTimeEnd");
                tbCmd.Parameters.Add("@tbBgTimeEnd", SqlDbType.VarChar).Value = beginE;
            }

            //view 的部份只搜尋今天的資料
            vwCond.Add("BEGIN_TIME>=@vwbgTimeStart");
            vwCmd.Parameters.Add("@vwbgTimeStart", SqlDbType.VarChar).Value = today + " 00:00";
            vwCond.Add("BEGIN_TIME<=@vwbgTimeEnd");
            vwCmd.Parameters.Add("@vwbgTimeEnd", SqlDbType.VarChar).Value = today + " 23:59";

            vwCmd.CommandText = bindContition(todaySql, vwCond);
            tbCmd.CommandText = bindContition(oldSql, tbCond);

            //檢查起單日-訖 是否有包含今天的日期
            bool startDayContainToday = checkDateRange(beginS);
            bool endDayContainToday = checkDateRange(beginE);

            mainDt = tp.getData(tbCmd);

            //若包含今天的日期，才會去 view 找今天的資料，並合併 dt
            if (endDayContainToday)
            {
                vwDt = tp.getData(vwCmd);
                mainDt.Merge(vwDt);
            }           
        }

        //look sql parameters
        //lookCmdParameters(vwCmd, "vwCmd");
        //lookCmdParameters(tbCmd, "tbCmd");

        //依輸入樣式的選擇進行過濾
        pruneDt = processDt(mainDt);

        return pruneDt;
    }

    private DataTable processDt(DataTable dt)
    {
        DataTable pruneDt = new DataTable();

        switch (rblStyle.SelectedValue)
        {
            case "0":  //無樣式
                return dt;
                break;
            case "1":  //僅留下 104 | 105 異動類型
                dt.Rows.Cast<DataRow>().Where(r =>
                (r.ItemArray[12].ToString().Contains("103")) ||
                (r.ItemArray[12].ToString().Contains("106"))
                ).ToList().ForEach(r => r.Delete());

                pruneDt = removeDataColumns(dt);
                
                break;
        }
        return pruneDt;
    }

    private DataTable removeDataColumns(DataTable dt)
    {
        var keepColNames = new List<string>(){"BPM結單日","物料文件","異動類型", "物料文件備註",
            "採購單號","採購項次","供應商", "物料號碼", "工單號碼", "工單料號", "短文", "收貨數", "單位" };

        var allColumns = dt.Columns.Cast<DataColumn>();
        var allColNames = allColumns.Select(c => c.ColumnName);
        var removeColNames = allColNames.Except(keepColNames);
        var colsToRemove = from r in removeColNames
                           join c in allColumns on r equals c.ColumnName
                           select c;
        while (colsToRemove.Any())
            dt.Columns.Remove(colsToRemove.First());

        return dt;
    }

    private void lookCmdParameters(SqlCommand cmd, string cmdName)
    {
        foreach (SqlParameter p in cmd.Parameters)
        {
            cmd.CommandText = cmd.CommandText.Replace(p.ParameterName, p.Value.ToString());
        }
        Response.Write(cmdName + "<p />" + cmd.CommandText + "<p />");
    }

    private bool checkDateRange(string date)
    {
        DateTime inputDate, today;

        if (string.IsNullOrEmpty(date)) return false;

        try
        {
            inputDate = DateTime.Parse(date);
            today = DateTime.Today;
            return inputDate >= today;
        }
        catch (Exception)
        {
            Response.Write("日期格式錯誤");
            return false;
        }        
    }

    private string bindContition(string SQLString, List<string> condition)
    {
        string sql = string.Format(
            SQLString,
            condition.Count > 0 ? " WHERE " : "",
            string.Join(" AND ", condition.ToArray())
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
 