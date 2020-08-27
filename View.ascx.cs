// ***********************************************************************************
// Connect UsersLibrary
// 
// Copyright (C) 2013-2014 DNN-Connect Association, Philipp Becker
// http://dnn-connect.org
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
// 
// ***********************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Connect.Libraries.UserManagement;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Profile;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;
using Telerik.Web.UI;

namespace Connect.Modules.UserManagement.AccountUpdate
{
    public partial class View : ConnectUsersModuleBase, IActionable
    {
        public View()
        {

            /* TODO ERROR: Skipped RegionDirectiveTrivia */
            this.Init += Page_Init;
            this.PreRender += Page_PreRender;
        }

        private bool blnPageNeedsToPostback = false;

        protected void Page_Init(object sender, EventArgs e)
        {

            // DotNetNuke.Framework.AJAX.RegisterScriptManager()

            var argplhControls = plhProfile;
            ProcessFormTemplate(ref argplhControls, GetTemplate(ModuleTheme, Libraries.UserManagement.Constants.TemplateName_Form, CurrentLocale, false), User);
            plhProfile = argplhControls;
            Button btnUpdate = (Button)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_UpdateButton);
            if (btnUpdate is object)
            {
                btnUpdate.Click += btnUpdate_Click;
                if (blnPageNeedsToPostback)
                {
                    AJAX.RegisterPostBackControl(btnUpdate);
                }
            }

            Button btnDelete = (Button)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_DeleteButton);
            if (btnDelete is object)
            {
                btnDelete.Click += btnDelete_Click;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Control argContainer = plhProfile;
            ManageRegionLabel(ref argContainer);
            plhProfile = (PlaceHolder)argContainer;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteAccount();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private void UpdateAccount()
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = false;
            var strMessages = new List<string>();
            string strUpdated = "";
            bool blnUpdateUsername = false;
            bool blnUpdateFirstname = false;
            bool blnUpdateLastname = false;
            bool blnUpdateDisplayname = false;
            bool blnUpdatePassword = false;
            bool blnUpdateEmail = false;
            bool blnUpdatePasswordQuestionAndAnswer = false;
            TextBox txtEmail = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_Email);
            blnUpdateEmail = txtEmail is object;
            if (blnUpdateEmail)
            {
                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_Email, plhProfile))
                {
                    strMessages.Add("Error_InvalidEmail");
                    Control argobjControl = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Email, ref argobjControl);
                    plhProfile = (PlaceHolder)argobjControl;
                }
                else
                {
                    Control argobjControl1 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_Email, ref argobjControl1, true);
                    plhProfile = (PlaceHolder)argobjControl1;
                }
            }

            TextBox txtPasswordCurrent = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_PasswordCurrent);
            TextBox txtPassword1 = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_Password1);
            TextBox txtPassword2 = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_Password2);
            TextBox txtPasswordQuestion = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_PasswordQuestion);
            TextBox txtPasswordAnswer = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_PasswordAnswer);
            blnUpdatePassword = txtPasswordCurrent is object && txtPassword1 is object && txtPassword2 is object;
            blnUpdatePasswordQuestionAndAnswer = txtPasswordQuestion is object && txtPasswordAnswer is object && txtPasswordCurrent is object;
            if (blnUpdatePassword)
            {
                if (string.IsNullOrEmpty(txtPassword1.Text) && string.IsNullOrEmpty(txtPassword2.Text))
                {
                    blnUpdatePassword = false;
                }
            }

            if (blnUpdatePassword)
            {
                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_PasswordCurrent, plhProfile))
                {
                    strMessages.Add("Error_MissingPasswordCurrent");
                    Control argobjControl2 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_PasswordCurrent, ref argobjControl2);
                    plhProfile = (PlaceHolder)argobjControl2;
                }
                else
                {
                    Control argobjControl3 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_PasswordCurrent, ref argobjControl3, true);
                    plhProfile = (PlaceHolder)argobjControl3;
                }

                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_Password1, plhProfile))
                {
                    strMessages.Add("Error_MissingPassword");
                    Control argobjControl4 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Password1, ref argobjControl4);
                    plhProfile = (PlaceHolder)argobjControl4;
                }
                else
                {
                    Control argobjControl5 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_Password1, ref argobjControl5, true);
                    plhProfile = (PlaceHolder)argobjControl5;
                }

                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_Password2, plhProfile))
                {
                    strMessages.Add("Error_MissingPassword");
                    Control argobjControl6 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Password2, ref argobjControl6);
                    plhProfile = (PlaceHolder)argobjControl6;
                }
                else
                {
                    Control argobjControl7 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_Password2, ref argobjControl7, true);
                    plhProfile = (PlaceHolder)argobjControl7;
                }
            }

            if (blnUpdatePasswordQuestionAndAnswer)
            {
                if (string.IsNullOrEmpty(txtPasswordAnswer.Text) && string.IsNullOrEmpty(txtPasswordQuestion.Text))
                {
                    blnUpdatePasswordQuestionAndAnswer = false;
                }
            }

            if (blnUpdatePasswordQuestionAndAnswer)
            {
                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_Password1, plhProfile))
                {
                    strMessages.Add("Error_MissingPasswordForQuestionAndAnswer");
                    Control argobjControl8 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Password1, ref argobjControl8);
                    plhProfile = (PlaceHolder)argobjControl8;
                }
                else
                {
                    Control argobjControl9 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_Password1, ref argobjControl9, true);
                    plhProfile = (PlaceHolder)argobjControl9;
                }

                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_PasswordAnswer, plhProfile))
                {
                    strMessages.Add("Error_MissingPasswordAnswerAndQuestion");
                    Control argobjControl10 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_PasswordAnswer, ref argobjControl10);
                    plhProfile = (PlaceHolder)argobjControl10;
                }
                else
                {
                    Control argobjControl11 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_PasswordAnswer, ref argobjControl11, true);
                    plhProfile = (PlaceHolder)argobjControl11;
                }

                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_PasswordQuestion, plhProfile))
                {
                    strMessages.Add("Error_MissingPasswordAnswerAndQuestion");
                    Control argobjControl12 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_PasswordQuestion, ref argobjControl12);
                    plhProfile = (PlaceHolder)argobjControl12;
                }
                else
                {
                    Control argobjControl13 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_PasswordQuestion, ref argobjControl13, true);
                    plhProfile = (PlaceHolder)argobjControl13;
                }
            }

            TextBox txtFirstName = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_Firstname);
            blnUpdateFirstname = txtFirstName is object;
            if (blnUpdateFirstname)
            {
                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_Firstname, plhProfile))
                {
                    strMessages.Add("Error_MissingFirstname");
                    Control argobjControl14 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Firstname, ref argobjControl14);
                    plhProfile = (PlaceHolder)argobjControl14;
                }
                else
                {
                    Control argobjControl15 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_Firstname, ref argobjControl15, true);
                    plhProfile = (PlaceHolder)argobjControl15;
                }
            }

            TextBox txtLastName = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_Lastname);
            blnUpdateLastname = txtLastName is object;
            if (blnUpdateLastname)
            {
                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_Lastname, plhProfile))
                {
                    strMessages.Add("Error_MissingLastname");
                    Control argobjControl16 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Lastname, ref argobjControl16);
                    plhProfile = (PlaceHolder)argobjControl16;
                }
                else
                {
                    Control argobjControl17 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_Lastname, ref argobjControl17, true);
                    plhProfile = (PlaceHolder)argobjControl17;
                }
            }

            if (CompareFirstNameLastName && blnUpdateFirstname & blnUpdateLastname)
            {
                if ((txtLastName.Text.ToLower().Trim() ?? "") == (txtFirstName.Text.ToLower().Trim() ?? ""))
                {
                    strMessages.Add("Error_LastnameLikeFirstname");
                    Control argobjControl18 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Firstname, ref argobjControl18);
                    plhProfile = (PlaceHolder)argobjControl18;
                }
            }

            TextBox txtDisplayName = (TextBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_Displayname);
            blnUpdateDisplayname = txtDisplayName is object;
            if (blnUpdateDisplayname)
            {
                if (!IsValidUserAttribute(Libraries.UserManagement.Constants.User_Displayname, plhProfile))
                {
                    strMessages.Add("Error_MissingDisplayName");
                    Control argobjControl19 = plhProfile;
                    AddErrorIndicator(Libraries.UserManagement.Constants.User_Displayname, ref argobjControl19);
                    plhProfile = (PlaceHolder)argobjControl19;
                }
                else
                {
                    Control argobjControl20 = plhProfile;
                    RemoveErrorIndicator(Libraries.UserManagement.Constants.User_Displayname, ref argobjControl20, true);
                    plhProfile = (PlaceHolder)argobjControl20;
                }
            }

            bool blnProfileErrorAdded = false;
            foreach (string itemProp in GetPropertiesFromTempate(GetTemplate(ModuleTheme, Libraries.UserManagement.Constants.TemplateName_Form, CurrentLocale, false)))
            {
                try
                {
                    var prop = ProfileController.GetPropertyDefinitionByName(PortalId, itemProp.Substring(2)); // itemprop comes in the form U:Propertyname or P:Propertyname
                    if (prop is object)
                    {
                        Control argobjControl23 = plhProfile;
                        if (!IsValidProperty(UserInfo, prop, ref argobjControl23))
                        {
                            if (blnProfileErrorAdded == false)
                            {
                                strMessages.Add("Error_MissingProfileField");
                                blnProfileErrorAdded = true;
                            }

                            Control argobjControl21 = plhProfile;
                            AddErrorIndicator(prop.PropertyDefinitionId.ToString(), ref argobjControl21);
                            plhProfile = (PlaceHolder)argobjControl21;
                        }
                        else
                        {
                            Control argobjControl22 = plhProfile;
                            RemoveErrorIndicator(prop.PropertyDefinitionId.ToString(), ref argobjControl22, prop.Required);
                            plhProfile = (PlaceHolder)argobjControl22;
                        }

                        plhProfile = (PlaceHolder)argobjControl23;
                    }
                }
                catch
                {
                }
            }

            if (strMessages.Count > 0)
            {
                pnlError.Visible = true;
                lblError.Text = "<ul>";
                foreach (string strMessage in strMessages)
                    lblError.Text += "<li>" + Localization.GetString(strMessage, LocalResourceFile) + "</li>";
                lblError.Text += "</ul>";
                return;
            }

            var oUser = UserController.GetCurrentUserInfo();
            var oldAccount = UserController.GetCurrentUserInfo();
            if (blnUpdateEmail)
            {
                if (UsernameMode == UsernameUpdateMode.Email)
                {
                    try
                    {
                        UserController.ChangeUsername(oUser.UserID, txtEmail.Text);
                    }
                    catch (Exception ex)
                    {
                        // in use already, do not update e-mail adress
                        pnlError.Visible = true;
                        lblError.Text = "<ul><li>" + Localization.GetString("DuplicateEmail.Text", LocalResourceFile) + "</li></ul>";
                        return;
                    }
                }

                oUser.Email = txtEmail.Text;
                if ((oUser.Email ?? "") != (oldAccount.Email ?? ""))
                {
                    strUpdated += Localization.GetString(Libraries.UserManagement.Constants.User_Email, LocalResourceFile) + ", ";
                }
            }

            // try updating password
            if (blnUpdatePassword)
            {
                if ((txtPassword1.Text ?? "") == (txtPassword2.Text ?? ""))
                {
                    if (UserController.ValidatePassword(txtPassword1.Text))
                    {
                        if (!UserController.ChangePassword(oUser, txtPasswordCurrent.Text, txtPassword1.Text))
                        {
                            pnlError.Visible = true;
                            lblError.Text = "<ul><li>" + Localization.GetString("PasswordUpdateError", LocalResourceFile) + "</li></ul>";
                            return;
                        }

                        strUpdated += Localization.GetString(Libraries.UserManagement.Constants.User_Password1, LocalResourceFile) + ", ";
                    }
                    else
                    {
                        int MinLength = 0;
                        int MinNonAlphaNumeric = 0;
                        try
                        {
                            MinLength = DotNetNuke.Security.Membership.MembershipProvider.Instance().MinPasswordLength;
                        }
                        catch
                        {
                        }

                        try
                        {
                            MinNonAlphaNumeric = DotNetNuke.Security.Membership.MembershipProvider.Instance().MinNonAlphanumericCharacters;
                        }
                        catch
                        {
                        }

                        string strPolicy = string.Format(Localization.GetString("PasswordPolicy_MinLength", LocalResourceFile), MinLength.ToString());
                        if (MinNonAlphaNumeric > 0)
                        {
                            strPolicy += string.Format(Localization.GetString("PasswordPolicy_MinNonAlphaNumeric", LocalResourceFile), MinNonAlphaNumeric.ToString());
                        }

                        pnlError.Visible = true;
                        lblError.Text = "<ul><li>" + string.Format(Localization.GetString("InvalidPassword", LocalResourceFile), strPolicy) + "</li></ul>";
                        return;
                    }
                }
                else
                {
                    pnlError.Visible = true;
                    lblError.Text = "<ul><li>" + Localization.GetString("PasswordsDontMatch.Text", LocalResourceFile) + "</li></ul>";
                    return;
                }
            }

            if (blnUpdatePasswordQuestionAndAnswer)
            {
                UserController.ChangePasswordQuestionAndAnswer(oUser, txtPasswordCurrent.Text, txtPasswordQuestion.Text, txtPasswordAnswer.Text);
                oUser.Membership.PasswordQuestion = txtPasswordQuestion.Text;
                oUser.Membership.PasswordAnswer = txtPasswordAnswer.Text;
            }

            UserController.UpdateUser(PortalId, oUser);
            var propertiesCollection = new ProfilePropertyDefinitionCollection();
            Control argContainer = plhProfile;
            UpdateProfileProperties(ref argContainer, ref oUser, ref propertiesCollection, ref strUpdated, GetPropertiesFromTempate(GetTemplate(ModuleTheme, Libraries.UserManagement.Constants.TemplateName_Form, CurrentLocale, false)));
            plhProfile = (PlaceHolder)argContainer;
            oUser = ProfileController.UpdateUserProfile(oUser, propertiesCollection);

            // make sure first and lastname are in sync with user and profile object
            if (blnUpdateFirstname == true)
            {
                if ((oldAccount.FirstName ?? "") != (txtFirstName.Text ?? ""))
                {
                    strUpdated += Localization.GetString(Libraries.UserManagement.Constants.User_Firstname, LocalResourceFile) + ", ";
                }

                oUser.Profile.FirstName = txtFirstName.Text;
                oUser.FirstName = txtFirstName.Text;
            }
            else if (!string.IsNullOrEmpty(oUser.Profile.FirstName))
            {
                oUser.FirstName = oUser.Profile.FirstName;
            }

            if (blnUpdateLastname == true)
            {
                if ((oldAccount.LastName ?? "") != (txtLastName.Text ?? ""))
                {
                    strUpdated += Localization.GetString(Libraries.UserManagement.Constants.User_Lastname, LocalResourceFile) + ", ";
                }

                oUser.Profile.LastName = txtLastName.Text;
                oUser.LastName = txtLastName.Text;
            }
            else if (!string.IsNullOrEmpty(oUser.Profile.LastName))
            {
                oUser.LastName = oUser.Profile.LastName;
            }

            if (blnUpdateDisplayname)
            {
                if ((oldAccount.DisplayName ?? "") != (txtDisplayName.Text ?? ""))
                {
                    strUpdated += Localization.GetString(Libraries.UserManagement.Constants.User_Displayname, LocalResourceFile) + ", ";
                }

                oUser.DisplayName = txtDisplayName.Text;
            }
            else
            {
                switch (DisplaynameMode)
                {
                    case DisplaynameUpdateMode.Email:
                        {
                            if (blnUpdateEmail)
                            {
                                oUser.DisplayName = txtEmail.Text.Trim();
                            }

                            break;
                        }

                    case DisplaynameUpdateMode.FirstLetterLastname:
                        {
                            if (blnUpdateLastname && blnUpdateFirstname)
                            {
                                oUser.DisplayName = oUser.FirstName.Trim().Substring(0, 1) + ". " + oUser.LastName;
                            }

                            break;
                        }

                    case DisplaynameUpdateMode.FirstnameLastname:
                        {
                            if (blnUpdateLastname && blnUpdateFirstname)
                            {
                                oUser.DisplayName = oUser.FirstName + " " + oUser.LastName;
                            }

                            break;
                        }

                    case DisplaynameUpdateMode.Lastname:
                        {
                            if (blnUpdateLastname)
                            {
                                oUser.DisplayName = oUser.LastName;
                            }

                            break;
                        }
                }
            }

            // update profile
            UserController.UpdateUser(PortalId, oUser);

            // add to role
            if (AddToRoleOnSubmit != Null.NullInteger)
            {
                try
                {
                    var rc = new RoleController();
                    if (AddToRoleStatus.ToLower() == "pending")
                    {
                        rc.AddUserRole(PortalId, oUser.UserID, AddToRoleOnSubmit, RoleStatus.Pending, false, DateTime.Now, Null.NullDate);
                    }
                    else
                    {
                        rc.AddUserRole(PortalId, oUser.UserID, AddToRoleOnSubmit, RoleStatus.Approved, false, DateTime.Now, Null.NullDate);
                    }
                }
                catch
                {
                }
            }

            // remove from role
            if (RemoveFromRoleOnSubmit != Null.NullInteger)
            {
                try
                {
                    var rc = new RoleController();
                    var r = rc.GetRole(RemoveFromRoleOnSubmit, PortalId);
                    RoleController.DeleteUserRole(oUser, r, PortalSettings, false);
                }
                catch
                {
                }
            }

            CheckBox chkTest = (CheckBox)FindMembershipControlsRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_RoleMembership);
            if (chkTest is object)
            {
                // at least on role membership checkbox found. Now lookup roles that could match
                var rc = new RoleController();
                ArrayList roles;
                roles = rc.GetPortalRoles(PortalId);
                foreach (RoleInfo objRole in roles)
                {
                    bool blnPending = false;
                    CheckBox chkRole = (CheckBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_RoleMembership + objRole.RoleName.Replace(" ", ""));
                    if (chkRole is null)
                    {
                        chkRole = (CheckBox)FindControlRecursive(plhProfile, plhProfile.ID + "_" + Libraries.UserManagement.Constants.ControlId_RoleMembership + objRole.RoleName.Replace(" ", "") + "_Pending");
                        blnPending = true;
                    }

                    if (chkRole is object)
                    {
                        if (blnPending)
                        {
                            rc.AddUserRole(PortalId, oUser.UserID, objRole.RoleID, RoleStatus.Pending, false, DateTime.Now, Null.NullDate);
                        }
                        else
                        {
                            rc.AddUserRole(PortalId, oUser.UserID, objRole.RoleID, RoleStatus.Approved, false, DateTime.Now, Null.NullDate);
                        }
                    }
                }
            }

            // notify admin   
            if (!string.IsNullOrEmpty(NotifyRole) && strUpdated.Length > 0)
            {
                string strBody = GetTemplate(ModuleTheme, Libraries.UserManagement.Constants.TemplateName_EmailToAdmin, CurrentLocale, false);
                strBody = strBody.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias);
                strBody = strBody.Replace("[PORTALNAME]", PortalSettings.PortalName);
                strBody = strBody.Replace("[USERID]", oUser.UserID.ToString());
                strBody = strBody.Replace("[DISPLAYNAME]", oUser.DisplayName);
                if (DotNetNuke.Security.Membership.MembershipProvider.Instance().PasswordRetrievalEnabled)
                {
                    strBody = strBody.Replace("[PASSWORD]", DotNetNuke.Security.Membership.MembershipProvider.Instance().GetPassword(oUser, ""));
                }

                strBody = strBody.Replace("[USERNAME]", oUser.Username);
                strBody = strBody.Replace("[FIRSTNAME]", oUser.FirstName);
                strBody = strBody.Replace("[LASTNAME]", oUser.LastName);
                strBody = strBody.Replace("[UPDATED]", strUpdated.Substring(0, strUpdated.LastIndexOf(",")).Replace(":", ""));
                strBody = strBody.Replace("[USERURL]", DotNetNuke.Common.Globals.NavigateURL(UsermanagementTab, "", "uid=" + oUser.UserID.ToString(), "RoleId=" + PortalSettings.RegisteredRoleId.ToString()));
                strBody = strBody.Replace("[RECIPIENTUSERID]", oUser.UserID.ToString());
                strBody = strBody.Replace("[USERID]", oUser.UserID.ToString());
                var ctrlRoles = new RoleController();
                var NotificationUsers = ctrlRoles.GetUsersByRoleName(PortalId, NotifyRole);
                foreach (UserInfo NotificationUser in NotificationUsers)
                {
                    try
                    {
                        strBody = strBody.Replace("[RECIPIENTUSERID]", NotificationUser.UserID.ToString());
                        strBody = strBody.Replace("[USERID]", NotificationUser.UserID.ToString());
                        DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, NotificationUser.Email, "", string.Format(Localization.GetString("NotifySubject_ProfileUpdate.Text", LocalResourceFile), PortalSettings.PortalName), strBody, "", "HTML", "", "", "", "");
                    }
                    catch
                    {
                    }
                }
            }

            if (NotifyUser && strUpdated.Length > 0)
            {
                string strBody = GetTemplate(ModuleTheme, Libraries.UserManagement.Constants.TemplateName_EmailToUser, CurrentLocale, false);
                strBody = strBody.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias);
                strBody = strBody.Replace("[PORTALNAME]", PortalSettings.PortalName);
                strBody = strBody.Replace("[USERID]", oUser.UserID.ToString());
                strBody = strBody.Replace("[DISPLAYNAME]", oUser.DisplayName);
                if (DotNetNuke.Security.Membership.MembershipProvider.Instance().PasswordRetrievalEnabled)
                {
                    strBody = strBody.Replace("[PASSWORD]", DotNetNuke.Security.Membership.MembershipProvider.Instance().GetPassword(oUser, ""));
                }

                strBody = strBody.Replace("[USERNAME]", oUser.Username);
                strBody = strBody.Replace("[FIRSTNAME]", oUser.FirstName);
                strBody = strBody.Replace("[LASTNAME]", oUser.LastName);
                strBody = strBody.Replace("[UPDATED]", strUpdated.Substring(0, strUpdated.LastIndexOf(",")).Replace(":", ""));
                strBody = strBody.Replace("[USERURL]", DotNetNuke.Common.Globals.NavigateURL(TabId));
                strBody = strBody.Replace("[RECIPIENTUSERID]", oUser.UserID.ToString());
                strBody = strBody.Replace("[USERID]", oUser.UserID.ToString());
                try
                {
                    DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, oUser.Email, "", string.Format(Localization.GetString("NotifySubject_UserDetails", LocalResourceFile), PortalSettings.PortalName), strBody, "", "HTML", "", "", "", "");
                }
                catch
                {
                }
            }

            lblSucess.Text = "<ul><li>" + Localization.GetString("AccountUpdateSuccess.Text", LocalResourceFile) + "</li></ul>";
            pnlSuccess.Visible = true;
            if ((ExternalInterface ?? "") != (Null.NullString ?? ""))
            {
                object objInterface = null;
                if (ExternalInterface.Contains(","))
                {
                    string strClass = ExternalInterface.Split(char.Parse(","))[0].Trim();
                    string strAssembly = ExternalInterface.Split(char.Parse(","))[1].Trim();
                    objInterface = Activator.CreateInstance(strAssembly, strClass).Unwrap();
                }

                if (objInterface is object)
                {
                    var argServer = Server;
                    var argResponse = Response;
                    var argRequest = Request;
                    ((Libraries.UserManagement.Interfaces.iAccountUpdate)objInterface).FinalizeAccountUpdate(ref argServer, ref argResponse, ref argRequest, oUser);
                }
            }

            if (Request.QueryString["ReturnURL"] is object)
            {
                Response.Redirect(Server.UrlDecode(Request.QueryString["ReturnURL"]), true);
            }

            if (RedirectAfterSubmit != Null.NullInteger)
            {
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(RedirectAfterSubmit));
            }
        }

        private void DeleteAccount()
        {
            var oUser = UserController.GetCurrentUserInfo();
            if (UserController.DeleteUser(ref oUser, false, false))
            {
                lblSucess.Text = "<ul><li>" + Localization.GetString("AccountDeleted.Text", LocalResourceFile) + "</li></ul>";
                pnlSuccess.Visible = true;
            }
            else
            {
                lblError.Text = "<ul><li>" + Localization.GetString("AccountNotDeleted.Text", LocalResourceFile) + "</li></ul>";
                pnlError.Visible = true;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                var Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                Actions.Add(GetNextActionID(), Localization.GetString("ManageTemplates.Action", LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("ManageTemplates"), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}