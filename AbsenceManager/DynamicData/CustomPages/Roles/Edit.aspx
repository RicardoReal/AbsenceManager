<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Edit.aspx.cs" Inherits="AbsenceManager.DynamicData.CustomPages.Roles.Edit" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:CustomValidator ID="customValida" ErrorMessage="Error." runat="server" Display="None"
        ForeColor="Red"></asp:CustomValidator>
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
        Editar
        <%= table.DisplayName %></h2>
    <asp:UpdatePanel ID="UpdatePanelEdit" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:FormView runat="server" ID="FormView1" DataSourceID="DetailsDataSource" DefaultMode="Edit"
                OnItemCommand="FormView1_ItemCommand" OnItemUpdated="FormView1_ItemUpdated" RenderOuterTable="false">
                <EditItemTemplate>
                    <table id="detailsTable" class="DDDetailsTableAlt" cellpadding="6">
                        <asp:DynamicEntity runat="server" Mode="Edit" />
                        <tr class="td">
                            <td colspan="2">
                                <asp:LinkButton runat="server" CommandName="Update" Text="Update" />
                                <asp:LinkButton runat="server" CommandName="Cancel" Text="Close" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
                <EmptyDataTemplate>
                    <div class="DDNoItem">
                        Item no longer exists.</div>
                </EmptyDataTemplate>
            </asp:FormView>
            <asp:EntityDataSource ID="DetailsDataSource" runat="server" EnableUpdate="true" Include="RoleApplicationScreen" />
            <asp:QueryExtender TargetControlID="DetailsDataSource" ID="DetailsQueryExtender"
                runat="server">
                <asp:DynamicRouteExpression />
            </asp:QueryExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
