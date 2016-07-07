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

    protected void Page_Load(object sender, EventArgs e)
    {
        //True or False
        if (! isFormEnabled)
        {
            //目前程式停用中，請連絡資訊組
            Response.Redirect("sapReport_false.aspx");
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
        txtBpmEndE.Text = "";
        txtBpmEndS.Text = "";
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
        string lastYear = today.Substring(0, 4);
        string beginS, beginE, endS, endE;
        lastYear = Convert.ToString(Convert.ToInt16(lastYear) - 1);

        SqlCommand cmd = new SqlCommand();
        List<string> cond = new List<string>();

        beginS = procDateTimeFormat(txtBpmBeginS.Text) + " 00:00";
        if (txtBpmBeginE.Text=="") beginE = procDateTimeFormat(txtBpmBeginS.Text) + " 23:59";
        else beginE = procDateTimeFormat(txtBpmBeginE.Text) + " 23:59";

        endS = procDateTimeFormat(txtBpmEndS.Text) + " 00:00";
        if (txtBpmEndS.Text=="") endE = procDateTimeFormat(txtBpmEndS.Text) + " 23:59";
        else endE = procDateTimeFormat(txtBpmEndE.Text) + " 23:59";

        if (beginS!= "")
        {            
            cond.Add("BEGIN_TIME>=@bgTime");
            cmd.Parameters.Add("@bgTime",SqlDbType.VarChar).Value=beginS;
        }
        else
        {
            lastYear += "/01/01 00:00";
            if (cbRecent.Checked)
            {
                cond.Add("BEGIN_TIME>=@bgTime");
                cmd.Parameters.Add("@bgTime", SqlDbType.VarChar).Value = lastYear;
            }
        }

        /*
        if (txtBpmBeginS.Text != "") Session["dateFltr"] = " AND BEGIN_TIME>='" + cp.convertToDatef(txtBpmBeginS.Text.Trim()) + " 00:00'";
        else
        {
            if (cbRecent.Checked) Session["recent"] += " AND BEGIN_TIME >='" + lastYear + "/01/01 00:00'";
            Session["dateFltr"] += " AND BEGIN_TIME>='" + today + " 00:00'";
        }
        */

        cond.Add("BEGIN_TIME<=@bgTime");
        cmd.Parameters.Add("@bgTime", SqlDbType.VarChar).Value = beginE;

        /* 
        if (txtBpmBeginE.Text != "")  Session["dateFltr"] += " AND BEGIN_TIME<='" + cp.convertToDatef(txtBpmBeginE.Text.Trim()) + " 23:59'";
        else txtBpmBeginE.Text = txtBpmBeginS.Text;

        if (txtBpmEndS.Text != "") Session["dateFltr"] += " AND END_TIME>='" + cp.convertToDatef(txtBpmEndS.Text.Trim()) + " 00:00'";
        else Session["dateFltr"] += " AND END_TIME>='" + today + " 00:00'";

        */

        if (endS!= "")
        {
            cond.Add("END_TIME>=@edTime");
            cmd.Parameters.Add("@edTime", SqlDbType.VarChar).Value = endS;
            cond.Add("END_TIME<=@edTime");
            cmd.Parameters.Add("@edTime", SqlDbType.VarChar).Value = endE;
        }

        /*
        if (txtBpmEndE.Text != "") Session["dateFltr"] += " AND END_TIME<='" + cp.convertToDatef(txtBpmEndE.Text.Trim()) + " 23:59'"; 
        else txtBpmEndE.Text = txtBpmEndS.Text;
        */

        if (txtBpmNo.Text!= "")
        {
            cond.Add("DOC_NBR LIKE '%@docNbr%'");
            cmd.Parameters.Add("@docNbr", SqlDbType.VarChar).Value = txtBpmNo.Text.Trim();
        }

        // if (txtBpmNo.Text != "") Session["ftr"] = " AND DOC_NBR LIKE '%" + txtBpmNo.Text.Trim() + "%'";

        if (txtBpmPo.Text!= "")
        {
            cond.Add("PO LIKE '%@poNum%'");
            cmd.Parameters.Add("@poNum", SqlDbType.VarChar).Value = txtBpmPo.Text.Trim();
        }

        //if (txtBpmPo.Text != "") Session["ftr"] += " AND PO LIKE '%" + txtBpmPo.Text.Trim() + "%'";

        if (txtMtrlDocNbr.Text!= "")
        {
            cond.Add("MTRLDOC LIKE '%@mtrlDoc%'");
            cmd.Parameters.Add("@mtrlDoc", SqlDbType.VarChar).Value = txtMtrlDocNbr.Text.Trim();
        }

        //if (txtMtrlDocNbr.Text != "") Session["ftr"] += " AND MTRLDOC LIKE '%" + txtMtrlDocNbr.Text.Trim() + "%'";

        if (txtOrdMtrl.Text!= "")
        {
            cond.Add("MTRLDOC LIKE '%@ordMtrl%'");
            cmd.Parameters.Add("@ordMtrl", SqlDbType.VarChar).Value = txtOrdMtrl.Text.Trim();
        }

        //if (txtOrdMtrl.Text != "") Session["ftr"] += " AND ORD_MATERIAL LIKE '%" + txtOrdMtrl.Text.Trim() + "%'";

        if (txtMatnr.Text!= "")
        {
            cond.Add("MATERIAL LIKE '%@matnr%'");
            cmd.Parameters.Add("@matnr", SqlDbType.VarChar).Value = txtMatnr.Text.Trim();
        }

        //if (txtMatnr.Text != "") Session["ftr"] += " AND MATERIAL like '%" + txtMatnr.Text.Trim() + "%'";

        if (txtVndrNm.Text!= "")
        {
            cond.Add("VENDOR_NAME LIKE '%@vendorNm%'");
            cmd.Parameters.Add("@vendorNm", SqlDbType.VarChar).Value = txtVndrNm.Text.Trim();
        }

        //if (txtVndrNm.Text != "") Session["ftr"] += " AND VENDOR_NAME like '%" + txtVndrNm.Text.Trim() + "%'";

        if (ddlMvt.SelectedIndex!=0)
        {
            cond.Add("MOVE_TYPE LIKE '%@mvT%'");
            cmd.Parameters.Add("@mvT", SqlDbType.VarChar).Value = ddlMvt.SelectedValue;
        }

        //if (ddlMvt.SelectedIndex != 0) Session["ftr"] += " AND MOVE_TYPE='" + ddlMvt.SelectedValue + "'";

        if (rdblQA.SelectedIndex!=0)
        {
            cond.Add("QAresult LIKE '%@QAResult%'");
            cmd.Parameters.Add("@QAResult", SqlDbType.VarChar).Value = rdblQA.SelectedValue;

        }

        //if (rdblQA.SelectedIndex != 0) Session["ftr"] += " AND QAresult ='" + rdblQA.SelectedValue + "'";

        if (rbSample.SelectedValue == "Y")
        {
            cond.Add("RDpersen <> ''");
        }

        //if (rbSample.SelectedValue == "Y") Session["ftr"] += " AND RDpersen <>''";

        if (ddlQA.SelectedIndex!=0)
        {
            cond.Add(ddlQA.SelectedValue);            
        }

        //if (ddlQA.SelectedIndex != 0) Session["ftr"] += " AND '" + ddlQA.SelectedValue + "'";

        Session["tList"] = cond;

        Response.Redirect("result.aspx");
    }

    private string procDateTimeFormat(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            text = cp.convertToDatef(text.Trim());
        }
        else
        {
            text = null;
        }
        return text;
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