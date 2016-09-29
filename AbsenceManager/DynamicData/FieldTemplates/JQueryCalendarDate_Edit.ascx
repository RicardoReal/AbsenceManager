<%@ Control Language="C#" CodeBehind="JQueryCalendarDate_Edit.ascx.cs" Inherits="AbsenceManager.JQueryCalendarDate_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" CssClass="JQDTextBox" Text='<%# FieldValueString %>'
    Columns="20"></asp:TextBox>
<asp:Literal runat="server" ID="Literal1" Text="<%# FieldValueString %>" />
<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" />
<asp:CustomValidator runat="server" ID="DateValidator" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" EnableClientScript="false" Enabled="false"
    OnServerValidate="DateValidator_ServerValidate" />