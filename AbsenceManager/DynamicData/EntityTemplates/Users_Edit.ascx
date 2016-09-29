<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Users_Edit.ascx.cs"
    Inherits="AbsenceManager.DynamicData.EntityTemplates.Users_Edit" %>
<tr class="td" style="vertical-align: top;">
    <td>
        <table>
            <tr class="td">
                <td class="DDLightHeader">Nr. Funcionário
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl2" runat="server" DataField="NrFuncionario"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Nome
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="Control_Nome" runat="server" DataField="Nome" OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Username
                </td>
                <td>
                    <asp:DynamicControl ID="Control_Username" runat="server" DataField="UserName" OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Password
                </td>
                <td>
                    <asp:DynamicControl ID="Control_Password" runat="server" DataField="Password" OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Activo
                </td>
                <td>
                    <asp:DynamicControl ID="DynamicControl7" runat="server" DataField="Activo" OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Is Administrator
                </td>
                <td>
                    <asp:DynamicControl ID="Control_IsAdmin" runat="server" DataField="IsAdmin" OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Role
                </td>
                <td>
                    <asp:DynamicControl ID="DynamicControl1" runat="server" DataField="Role" OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Departamento
                </td>
                <td>
                    <asp:DynamicControl ID="DynamicControl3" runat="server" DataField="Departamento"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Empresa
                </td>
                <td>
                    <asp:DynamicControl ID="DynamicControl4" runat="server" DataField="Empresa" OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <div id="CustoHoraDiv" runat="server">
                <tr class="td">
                    <td class="DDLightHeader">Custo Hora
                    </td>
                    <td>
                        <asp:DynamicControl ID="DynamicControl6" runat="server" DataField="CustoHora" OnInit="DynamicControl_Init" />
                    </td>
                </tr>
            </div>
            <tr class="td">
                <td class="DDLightHeader">Dias de Férias
                </td>
                <td>
                    <asp:DynamicControl ID="DynamicControl5" runat="server" DataField="NrDiasFerias"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
        </table>
    </td>
    <td>
        <table cellpadding="6">
            <tr class="td">
                <td class="DDLightHeader">Fardamento Nr.
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl8" runat="server" DataField="NrFardamento"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Sapato
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl9" runat="server" DataField="Sapato"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Calça
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl10" runat="server" DataField="Calça"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Polo
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl11" runat="server" DataField="Polo"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Casaco
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl12" runat="server" DataField="Casaco"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Bata
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl13" runat="server" DataField="Bata"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
        </table>
    </td>
    <td>
        <table cellpadding="6">
            <tr class="td">
                <td class="DDLightHeader">Data Admissão
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl14" runat="server" DataField="DataAdmissao"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Morada
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl15" runat="server" DataField="Morada"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Telefone
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl16" runat="server" DataField="Telefone"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Email
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl17" runat="server" DataField="Email"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">Data Nascimento
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl18" runat="server" DataField="DataNascimento"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
            <tr class="td">
                <td class="DDLightHeader">NIF
                </td>
                <td class="DDLightHeaderInnerField">
                    <asp:DynamicControl ID="DynamicControl20" runat="server" DataField="NIF"
                        OnInit="DynamicControl_Init" />
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr class="td" style="vertical-align: top;" runat="server" id="Tr2">
    <td colspan="2">
        <table cellpadding="6" width="100%">
            <td class="DDLightHeader">Ficheiros
            </td>
            <td colspan="3">
                <asp:GridView ID="GridViewFiles" runat="server" AutoGenerateColumns="false" ShowHeader="true" EmptyDataText="No Files Uploaded."
                    CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" />
                        <asp:BoundField DataField="DirectoryName" HeaderText="Directoria" />
                        <asp:BoundField DataField="CreationTime" HeaderText="Data Upload" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("FullName") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("FullName") %>' runat="server" OnClick="DeleteFile" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:FileUpload ID="FileUploadControl" runat="server" EnableViewState="true" />
                        <asp:Button runat="server" ID="UploadButton" Text="Upload File" OnClick="UploadButton_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="UploadButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </table>
    </td>
</tr>
<!-- DOCUMENTO IDENTIFICAÇÃO -->
<tr class="td" style="vertical-align: top;" runat="server" id="DocId_Tr">
    <td colspan="2">
        <table cellpadding="6" width="100%">
            <td class="DDLightHeader">Doc Identificacao </td>
            <td colspan="3">
                <asp:GridView ID="DocID_GridView" runat="server" AutoGenerateColumns="false" ShowHeader="true" EmptyDataText="No Files Uploaded."
                    CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6" AutoGenerateEditButton="true" OnRowEditing="DocID_GridView_RowEditing"
                    OnRowCancelingEdit="DocID_GridView_RowCancelingEdit" OnRowUpdating="DocID_GridView_RowUpdating" >
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" ReadOnly="true" />
                        <asp:BoundField DataField="Validade" HeaderText="Validade" DataFormatString="{0:d}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="DocId_lnkDownload" Text="Download" CommandName="Download" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="DocId_DownloadFile"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="DocId_lnkDownload" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="DocId_lnkDelete" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="DocId_DeleteFile" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:FileUpload ID="DocID_FileUpload" runat="server" EnableViewState="true" />
                        <asp:Button runat="server" ID="DocID_UploadButton" Text="Upload File" OnClick="DocId_UploadButton_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="DocID_UploadButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </table>
    </td>
</tr>
<!-- FICHA APTIDAO -->
<tr class="td" style="vertical-align: top;" runat="server" id="FichaApt_Tr">
    <td colspan="2">
        <table cellpadding="6" width="100%">
            <td class="DDLightHeader">Ficha Aptidão</td>
            <td colspan="3">
                <asp:GridView ID="FichaApt_GridView" runat="server" AutoGenerateColumns="false" ShowHeader="true" EmptyDataText="No Files Uploaded."
                    CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6" AutoGenerateEditButton="true" OnRowEditing="FichaApt_GridView_RowEditing"
                    OnRowCancelingEdit="FichaApt_GridView_RowCancelingEdit" OnRowUpdating="FichaApt_GridView_RowUpdating" >
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" ReadOnly="true" />
                        <asp:BoundField DataField="Validade" HeaderText="Validade" DataFormatString="{0:d}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="FichaApt_lnkDownload" Text="Download" CommandName="Download" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="FichaApt_DownloadFile"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="FichaApt_lnkDownload" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="FichaApt_lnkDelete" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="FichaApt_DeleteFile" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:FileUpload ID="FichaApt_FileUpload" runat="server" EnableViewState="true" />
                        <asp:Button runat="server" ID="FichaApt_UploadButton" Text="Upload File" OnClick="FichaApt_UploadButton_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="FichaApt_UploadButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </table>
    </td>
</tr>
<!-- CURRÍCULO -->
<tr class="td" style="vertical-align: top;" runat="server" id="CV_Tr">
    <td colspan="2">
        <table cellpadding="6" width="100%">
            <td class="DDLightHeader">Currículo </td>
            <td colspan="3">
                <asp:GridView ID="CV_GridView" runat="server" AutoGenerateColumns="false" ShowHeader="true" EmptyDataText="No Files Uploaded."
                    CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6" AutoGenerateEditButton="true" OnRowEditing="CV_GridView_RowEditing"
                    OnRowCancelingEdit="CV_GridView_RowCancelingEdit" OnRowUpdating="CV_GridView_RowUpdating" >
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" ReadOnly="true" />
                        <asp:BoundField DataField="Validade" HeaderText="Validade" DataFormatString="{0:d}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="CV_lnkDownload" Text="Download" CommandName="Download" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="CV_DownloadFile"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="CV_lnkDownload" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="CV_lnkDelete" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="CV_DeleteFile" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:FileUpload ID="CV_FileUpload" runat="server" EnableViewState="true" />
                        <asp:Button runat="server" ID="CV_UploadButton" Text="Upload File" OnClick="CV_UploadButton_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="CV_UploadButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </table>
    </td>
</tr>
<!-- CERTIFICADO HABILITACOES -->
<tr class="td" style="vertical-align: top;" runat="server" id="Tr4">
    <td colspan="2">
        <table cellpadding="6" width="100%">
            <td class="DDLightHeader">Cert. Habilitações </td>
            <td colspan="3">
                <asp:GridView ID="CertHab_GridView" runat="server" AutoGenerateColumns="false" ShowHeader="true" EmptyDataText="No Files Uploaded."
                    CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6" AutoGenerateEditButton="true" OnRowEditing="CertHab_GridView_RowEditing"
                    OnRowCancelingEdit="CertHab_GridView_RowCancelingEdit" OnRowUpdating="CertHab_GridView_RowUpdating" >
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" ReadOnly="true" />
                        <asp:BoundField DataField="Validade" HeaderText="Validade" DataFormatString="{0:d}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="CertHab_lnkDownload" Text="Download" CommandName="Download" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="CertHab_DownloadFile"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="CertHab_lnkDownload" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="CertHab_lnkDelete" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="CertHab_DeleteFile" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:FileUpload ID="CertHab_FileUpload" runat="server" EnableViewState="true" />
                        <asp:Button runat="server" ID="CertHab_UploadButton" Text="Upload File" OnClick="CertHab_UploadButton_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="CertHab_UploadButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </table>
    </td>
</tr>
<!-- REGISTO CRIMINAL-->
<tr class="td" style="vertical-align: top;" runat="server" id="Tr5">
    <td colspan="2">
        <table cellpadding="6" width="100%">
            <td class="DDLightHeader">Registo Criminal </td>
            <td colspan="3">
                <asp:GridView ID="RegCrim_GridView" runat="server" AutoGenerateColumns="false" ShowHeader="true" EmptyDataText="No Files Uploaded."
                    CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6" AutoGenerateEditButton="true" OnRowEditing="RegCrim_GridView_RowEditing"
                    OnRowCancelingEdit="RegCrim_GridView_RowCancelingEdit" OnRowUpdating="RegCrim_GridView_RowUpdating" >
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" ReadOnly="true" />
                        <asp:BoundField DataField="Validade" HeaderText="Validade" DataFormatString="{0:d}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="RegCrim_lnkDownload" Text="Download" CommandName="Download" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="RegCrim_DownloadFile"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="RegCrim_lnkDownload" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="RegCrim_lnkDelete" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' runat="server" OnClick="RegCrim_DeleteFile" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:FileUpload ID="RegCrim_FileUpload" runat="server" EnableViewState="true" />
                        <asp:Button runat="server" ID="RegCrim_UploadButton" Text="Upload File" OnClick="RegCrim_UploadButton_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="RegCrim_UploadButton" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </table>
    </td>
</tr>

