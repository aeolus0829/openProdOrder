<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 70%;
        }
        .style3
        {
            width: 116px;
        }
        .style5
        {
            width: 122px;
        }
        .auto-style1 {
            width: 219px;
        }
        .auto-style2 {
            width: 122px;
            height: 23px;
        }
        .auto-style3 {
            width: 219px;
            height: 23px;
        }
        .auto-style4 {
            width: 116px;
            height: 23px;
        }
        .auto-style5 {
            height: 23px;
            width: 290px;
        }
        .auto-style6 {
            width: 290px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSubmt" visible="True">
    <div>
    
        <h1>
            BPM進料狀況表</h1>
        <table class="style1">
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    BPM起單日-起</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtBpmBeginS" runat="server" TabIndex="1"></asp:TextBox>
                </td>
                <td bgcolor="#99CCFF" class="style3">                    
                    BPM起單日-訖</td>
                <td class="auto-style6">
                    <asp:TextBox ID="txtBpmBeginE" runat="server" 
                        ontextchanged="txtBpmBeginE_TextChanged" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    BPM結案日-起</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtBpmEndS" runat="server" TabIndex="3"></asp:TextBox>
                </td>
                <td bgcolor="#99CCFF" class="style3">
                    BPM結案日-訖</td>
                <td class="auto-style6">
                    <asp:TextBox ID="txtBpmEndE" runat="server" TabIndex="4"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    BPM單號</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtBpmNo" runat="server" TabIndex="5"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">                    
                </td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    採購單號 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtBpmPo" runat="server" TabIndex="6"></asp:TextBox>
                </td>
                <td bgcolor="#99CCFF" class="style3">
                    物料文件號碼 %</td>
                <td class="auto-style6">
                    <asp:TextBox ID="txtMtrlDocNbr" runat="server" TabIndex="7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    供應商名稱 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtVndrNm" runat="server" TabIndex="8"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    物料號碼 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtMatnr" runat="server" TabIndex="9"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    工單料號 %</td>
                <td class="auto-style1">
                    <asp:TextBox ID="txtOrdMtrl" runat="server" TabIndex="10"></asp:TextBox>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    單據狀態</td>
                <td class="auto-style1">
                    <asp:DropDownList ID="ddlQA" runat="server" TabIndex="11">
                        <asp:ListItem Selected="True">全部</asp:ListItem>
                        <asp:ListItem Value="task_status='1'">未結案</asp:ListItem>
                        <asp:ListItem Value="excpMit='特採中'">特採中</asp:ListItem>
                        <asp:ListItem Value="task_status='2'">結案</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td bgcolor="#99CCFF" class="style3">
                    樣品</td>
                <td class="auto-style6">
                    <asp:RadioButtonList ID="rbSample" runat="server" RepeatDirection="Horizontal" TabIndex="12">
                        <asp:ListItem Value="Y">是</asp:ListItem>
                        <asp:ListItem Selected="True" Value="N">否</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="auto-style2">
                    異動類型</td>
                <td class="auto-style3">
                    <asp:DropDownList ID="ddlMvt" runat="server" TabIndex="13">
                        <asp:ListItem Selected="True">全部</asp:ListItem>
                        <asp:ListItem>103</asp:ListItem>
                        <asp:ListItem>105</asp:ListItem>
                        <asp:ListItem>104</asp:ListItem>
                        <asp:ListItem>106</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td bgcolor="White" class="auto-style4">
                    </td>
                <td class="auto-style5">
                    </td>
            </tr>
            <tr>
                <td bgcolor="#99CCFF" class="style5">
                    檢驗結果</td>
                <td class="auto-style1">
                    <asp:RadioButtonList ID="rdblQA" runat="server" TabIndex="14" OnSelectedIndexChanged="rdblQA_SelectedIndexChanged">
                        <asp:ListItem Selected="True">全部</asp:ListItem>
                        <asp:ListItem>合格</asp:ListItem>
                        <asp:ListItem>不合格</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td bgcolor="White" class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style5">
                    &nbsp;</td>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="style3">
                    &nbsp;</td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style5">
                    &nbsp;</td>
                <td class="auto-style1">
                    <asp:Button ID="btnSubmt" runat="server" onclick="btnSubmt_Click" Text="查詢" 
                        style="height: 26px; width: 42px;" TabIndex="50" />
                </td>
                <td class="style3">
                    <asp:Button ID="btnClr" runat="server" onclick="btnClr_Click" Text="清除" 
                        TabIndex="51" />
                </td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    <br />
    註1: BPM文件日期 2014/7/11後才有檢驗結果及特採狀態可查詢，格式 20131231<br />
        註2: 日期若起訖為同一天，輸入起始日期就好<br />
    註3: 欄位標示為 % 表示輸入部份資料即可<br />
        <br />
        <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="/bpm/">回BPM報表畫面</asp:HyperLink>
        <br />
        <br />
    </form>
</body>
</html>
