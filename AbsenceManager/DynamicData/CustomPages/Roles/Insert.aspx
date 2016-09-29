<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Insert.aspx.cs"
    Inherits="AbsenceManager.DynamicData.CustomPages.Roles.Insert" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:CustomValidator ID="customValida" ErrorMessage="Estimated time cannot be saved with Status [Not Set]."
        runat="server" Display="None" ForeColor="Red"></asp:CustomValidator>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
        HeaderText="List of validation errors" CssClass="DDValidator" />
    <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="FormView1"
        Display="None" CssClass="DDValidator" />
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true">
        <DataControls>
            <asp:DataControlReference ControlID="FormView1" />
        </DataControls>
    </asp:DynamicDataManager>
    <h2 class="DDSubHeader">
        Add new entry to table
        <%= table.DisplayName %></h2>
    <asp:UpdatePanel ID="UpdatePanelInsert" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:FormView runat="server" ID="FormView1" DataSourceID="DetailsDataSource" DefaultMode="Insert"
                OnItemCommand="FormView1_ItemCommand" OnItemInserted="FormView1_ItemInserted"
                RenderOuterTable="false">
                <InsertItemTemplate>
                    <table id="detailsTable" class="DDDetailsTableAlt" cellpadding="6">
                        <asp:DynamicEntity runat="server" Mode="Insert" />
                        <tr class="td">
                            <td colspan="2">
                                <asp:LinkButton ID="lnkInsert" runat="server" CommandName="Insert" Text="Insert" />
                                <asp:LinkButton ID="lnkClose" runat="server" CommandName="Cancel" Text="Close" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
            </asp:FormView>
            <asp:EntityDataSource ID="DetailsDataSource" runat="server" EnableInsert="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
