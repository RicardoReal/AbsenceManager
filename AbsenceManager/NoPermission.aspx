<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoPermission.aspx.cs" Inherits="AbsenceManager.NoPermission" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta http-equiv="content-type" content="text/html; charset=windows-1250" />
        <title>Absence Manager Information</title>
    </head>
    <body>
        <table align="center" style="border: 1px solid #C0C0C0; width: 100%;">
            <tr>
                <td align="right" colspan="2">
                    <asp:Image ID="ImageLogo" runat="server" ImageUrl="~/Images/LOGO_SOCIEMBAL.png" AlternateText="Sociembal" ToolTip="Sociembal" />
                </td>
            </tr>
            <tr>
                <td style="width: 128px;">
                    <asp:Image ID="ImageWarning" runat="server" 
                        ImageUrl="~/Images/nopermission.png" AlternateText="Warning" 
                        ToolTip="Warning" />
                </td>
                <td>
                    <p style="font: Trebuchet, sans-serif; font-size: large; font-family: 'Trebuchet MS';">
                        <strong>Absence Manager</strong>
                    </p>
                    <p style="font: Trebuchet, sans-serif; font-size: small; font-family: 'Trebuchet MS';">
                        <strong>You do not have permission to acess the selected page.</strong></p>
                    <p style="font: Trebuchet, sans-serif; font-size: small; font-family: 'Trebuchet MS';">
                        <br />
                        Please contact the administration team.
                    </p>
                    <p style="font: Trebuchet, sans-serif; font-size: small; font-family: 'Trebuchet MS';">
                        Press the back button to return to previous page.
                    </p>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Image ID="ImageLogica" runat="server" ImageUrl="~/Images/AbsenceManager_Logo.png" AlternateText="Absence Manager" ToolTip="Absence Manager" />
                </td>
            </tr>
        </table>
    </body>
</html>