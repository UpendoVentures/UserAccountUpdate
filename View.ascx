<%@ Control Language="c#" AutoEventWireup="true" Inherits="Connect.Modules.UserManagement.AccountUpdate.View" Codebehind="View.ascx.cs" %>

<div class="connect_accountform">

    <asp:Panel ID="pnlError" runat="server" CssClass="dnnFormMessage dnnFormError" Visible="false">
        <asp:Literal id="lblError" runat="server"></asp:Literal>
    </asp:Panel> 
    <asp:Panel ID="pnlSuccess" runat="server" CssClass="dnnFormMessage dnnFormSuccess" Visible="false">
        <asp:Literal id="lblSucess" runat="server"></asp:Literal>
    </asp:Panel>     
    
    <asp:PlaceHolder ID="plhProfile" runat="server"></asp:PlaceHolder>       
                     
</div>