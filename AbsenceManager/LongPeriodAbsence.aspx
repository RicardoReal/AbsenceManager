<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="LongPeriodAbsence.aspx.cs"
    Inherits="AbsenceManager.LongPeriodAbsence" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:CustomValidator ID="customValida" ErrorMessage="Error." runat="server" Display="None"
        ForeColor="Red"></asp:CustomValidator>
   <%-- <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
        HeaderText="List of validation errors" CssClass="DDValidator" />
    <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="detailsTable"
        Display="None" CssClass="DDValidator" />--%>
    <%--<asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true">
        <DataControls>
            <asp:DataControlReference ControlID="FormView1" />
        </DataControls>
    </asp:DynamicDataManager>--%>
    <h2 class="DDSubHeader">
        Novo Período de Ausência</h2>
    <div id="ContentPlaceHolder1_UpdatePanel1">
        <table id="detailsTable" class="DDDetailsTable" cellpadding="6">
            <tbody>
                <tr class="td">
                    <td class="DDLightHeader">
                        Funcionário
                    </td>
                    <td>
                        <asp:DropDownList class="DDDropDown" ID="DropDownFuncionarios" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="td">
                    <td class="DDLightHeader">
                        Data Início
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="JQDTextBox" Text=""
                            Columns="20"></asp:TextBox>
                    </td>
                </tr>
                <tr class="td">
                    <td class="DDLightHeader">
                        Data Fim
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="JQDTextBox" Text="" Columns="20"></asp:TextBox>
                    </td>
                </tr>
                <tr class="td">
                    <td class="DDLightHeader">
                        Tipo Ausência
                    </td>
                    <td>
                        <asp:DropDownList class="DDDropDown" ID="DropDownTipoAusência" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="td">
                    <td colspan="2">
                        <asp:LinkButton ID="InsertButton" runat="server" OnClick="Insert_OnClick">Insert</asp:LinkButton>
                        <asp:LinkButton ID="Cancel" runat="server" OnClick="Cancel_OnClick">Cancel</asp:LinkButton>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:CustomValidator ID="customValida" ErrorMessage="Error." runat="server" Display="None"
                ForeColor="Red"></asp:CustomValidator>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" CssClass="DDValidator" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="FormView1"
                Display="None" CssClass="DDValidator" />
            <asp:FormView runat="server" ID="FormView1" DataSourceID="DetailsDataSource" DefaultMode="Insert"
                OnItemCommand="FormView1_ItemCommand" OnItemInserted="FormView1_ItemInserted"
                RenderOuterTable="false">
                <InsertItemTemplate>
                    <table id="detailsTable" class="DDDetailsTable" cellpadding="6" width="100%">
                        <asp:DynamicEntity ID="DynamicEntity1" runat="server" Mode="Insert" />
                        <tr class="td">
                            <td colspan="2">
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Insert" Text="Insert" />
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Cancel" Text="Cancel"
                                    CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
            </asp:FormView>
            <asp:EntityDataSource ID="DetailsDataSource" runat="server" EnableInsert="true" />
        </ContentTemplate>--%>
    </asp:UpdatePanel>
</asp:Content>
