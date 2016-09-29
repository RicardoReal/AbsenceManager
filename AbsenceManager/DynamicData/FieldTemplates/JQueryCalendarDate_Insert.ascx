<%@ Control Language="C#" CodeBehind="JQueryCalendarDate_Insert.ascx.cs" Inherits="AbsenceManager.JQueryCalendarDate_InsertField" %>
<asp:TextBox ID="TextBox1" runat="server" CssClass="JQDTextBox" Text='<%# FieldValueEditString %>'
    Columns="20"></asp:TextBox>
<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" />
<asp:CustomValidator runat="server" ID="DateValidator" CssClass="DDControl DDValidator"
    ControlToValidate="TextBox1" Display="Static" EnableClientScript="false" Enabled="false"
    OnServerValidate="DateValidator_ServerValidate" />