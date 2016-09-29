using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using AbsenceManager.Security;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace AbsenceManager.DynamicData.EntityTemplates
{
    public partial class Users_Edit : System.Web.DynamicData.EntityTemplateUserControl
    {
        private MetaColumn currentColumn;

        protected override void OnLoad(EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            if (Mode == DataBoundControlMode.Insert) CustoHoraDiv.Visible = false;

            foreach (MetaColumn column in Table.GetScaffoldColumns(Mode, ContainerType))
            {
                currentColumn = column;
            }

            if (!IsPostBack)
            {
                if (Mode == DataBoundControlMode.Insert)
                {
                    Tr2.Visible = false;
                    DocId_Tr.Visible = false;
                }
                else GetFilesAndFolders();
            }


        }

        public void GetFilesAndFolders()
        {
            long? Id = long.Parse(Context.Request.QueryString["ID"]);
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/Uploads/Funcionarios/"));
            FileInfo[] fileInfo = dirInfo.GetFiles(Id + "_*.*", SearchOption.AllDirectories);
            GridViewFiles.DataSource = fileInfo;
            GridViewFiles.DataBind();

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                DocID_GridView.DataSource = ent.UserDocuments.Where(x => x.ID == u.DocIdentificacao).ToList();
                DocID_GridView.DataBind();
            }

        }

        protected void DynamicControl_Init(object sender, EventArgs e)
        {
            DynamicControl dynamicControl = (DynamicControl)sender;

            string colName = (from cols in Table.Columns
                              where cols.Name == dynamicControl.DataField
                              select cols.ShortDisplayName).First();

            if (colName == "Is Administrator")
                dynamicControl.Mode = PermissionsManager.UserIsAdmin() ? DataBoundControlMode.Edit : DataBoundControlMode.ReadOnly;
            else
                dynamicControl.Mode = DataBoundControlMode.Edit;
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    long FuncionarioID = long.Parse(Context.Request.QueryString["ID"]);
                    string filename = Path.GetFileName(FileUploadControl.FileName);
                    FileUploadControl.SaveAs(Server.MapPath("~/Uploads/Funcionarios/") + FuncionarioID + "_" + filename);
                    Response.Redirect(Request.Url.AbsoluteUri);
                    //StatusLabel.Text = "Upload status: File uploaded!";
                }
                catch (Exception ex)
                {
                    //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            try
            {
                string filePath = (sender as LinkButton).CommandArgument;
                //Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                Response.WriteFile(filePath);
                Response.End();
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DownloadFile");
            }
        }

        protected void DeleteFile(object sender, EventArgs e)
        {
            try
            {
                string filePath = (sender as LinkButton).CommandArgument;
                File.Delete(filePath);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DeleteFile");
            }
        }

        #region DOCID EVENTS

        protected void DocId_Bind()
        {
            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                DocID_GridView.DataSource = ent.UserDocuments.Where(x => x.ID == u.DocIdentificacao).ToList();
                DocID_GridView.DataBind();
            }
        }

        protected void DocId_UploadButton_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(DocID_FileUpload.PostedFile.FileName);
            string contentType = DocID_FileUpload.PostedFile.ContentType;

            byte[] bytes = new byte[DocID_FileUpload.PostedFile.InputStream.Length];
            DocID_FileUpload.PostedFile.InputStream.Read(bytes, 0, bytes.Length);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = new UserDocument();
                ud.Data = bytes;
                ud.ContentType = contentType;
                ud.Name = filename;

                ent.UserDocuments.AddObject(ud);

                u.DocIdentificacao = ud.ID;

                ent.SaveChanges();
            }
            DocId_Bind();
        }

        protected void DocId_DownloadFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName, contentType;

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    bytes = ud.Data;
                    contentType = ud.ContentType;
                    fileName = ud.Name;

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.ContentType = contentType;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    HttpContext.Current.Response.BinaryWrite(bytes.ToArray());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DownloadFile");
            }
        }

        protected void DocId_DeleteFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                    User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();
                    u.DocIdentificacao = null;

                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    ent.DeleteObject(ud);
                    ent.SaveChanges();
                }
                DocId_Bind();
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DeleteFile");
            }
        }

        protected void DocID_GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DocID_GridView.EditIndex = e.NewEditIndex;
            DocId_Bind();
        }

        protected void DocID_GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string data = ((TextBox)DocID_GridView.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            DateTime val = Convert.ToDateTime(data);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = ent.UserDocuments.Where(x => x.ID == u.DocIdentificacao).FirstOrDefault();
                ud.Validade = val;

                ent.SaveChanges();
            }

            DocID_GridView.EditIndex = -1;
            DocId_Bind();
        }

        protected void DocID_GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DocID_GridView.EditIndex = -1;
            DocId_Bind();
        }

        #endregion

        #region FICHA APTIDÃO EVENTS

        protected void FichaApt_Bind()
        {
            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                FichaApt_GridView.DataSource = ent.UserDocuments.Where(x => x.ID == u.FichaAptidao).ToList();
                FichaApt_GridView.DataBind();
            }
        }

        protected void FichaApt_UploadButton_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(FichaApt_FileUpload.PostedFile.FileName);
            string contentType = FichaApt_FileUpload.PostedFile.ContentType;

            byte[] bytes = new byte[FichaApt_FileUpload.PostedFile.InputStream.Length];
            FichaApt_FileUpload.PostedFile.InputStream.Read(bytes, 0, bytes.Length);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = new UserDocument();
                ud.Data = bytes;
                ud.ContentType = contentType;
                ud.Name = filename;

                ent.UserDocuments.AddObject(ud);

                u.FichaAptidao = ud.ID;

                ent.SaveChanges();
            }
            FichaApt_Bind();
        }

        protected void FichaApt_DownloadFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName, contentType;

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    bytes = ud.Data;
                    contentType = ud.ContentType;
                    fileName = ud.Name;

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.ContentType = contentType;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    HttpContext.Current.Response.BinaryWrite(bytes.ToArray());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DownloadFile");
            }
        }

        protected void FichaApt_DeleteFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                    User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();
                    u.FichaAptidao = null;

                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    ent.DeleteObject(ud);
                    ent.SaveChanges();
                }
                FichaApt_Bind();
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DeleteFile");
            }
        }

        protected void FichaApt_GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            FichaApt_GridView.EditIndex = e.NewEditIndex;
            FichaApt_Bind();
        }

        protected void FichaApt_GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string data = ((TextBox)FichaApt_GridView.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            DateTime val = Convert.ToDateTime(data);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = ent.UserDocuments.Where(x => x.ID == u.FichaAptidao).FirstOrDefault();
                ud.Validade = val;

                ent.SaveChanges();
            }

            FichaApt_GridView.EditIndex = -1;
            FichaApt_Bind();
        }

        protected void FichaApt_GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            FichaApt_GridView.EditIndex = -1;
            FichaApt_Bind();
        }

        #endregion

        #region CURRICULO EVENTS

        protected void CV_Bind()
        {
            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                CV_GridView.DataSource = ent.UserDocuments.Where(x => x.ID == u.Curriculo).ToList();
                CV_GridView.DataBind();
            }
        }

        protected void CV_UploadButton_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(CV_FileUpload.PostedFile.FileName);
            string contentType = CV_FileUpload.PostedFile.ContentType;

            byte[] bytes = new byte[CV_FileUpload.PostedFile.InputStream.Length];
            CV_FileUpload.PostedFile.InputStream.Read(bytes, 0, bytes.Length);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = new UserDocument();
                ud.Data = bytes;
                ud.ContentType = contentType;
                ud.Name = filename;

                ent.UserDocuments.AddObject(ud);

                u.Curriculo = ud.ID;

                ent.SaveChanges();
            }
            CV_Bind();
        }

        protected void CV_DownloadFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName, contentType;

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    bytes = ud.Data;
                    contentType = ud.ContentType;
                    fileName = ud.Name;

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.ContentType = contentType;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    HttpContext.Current.Response.BinaryWrite(bytes.ToArray());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DownloadFile");
            }
        }

        protected void CV_DeleteFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                    User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();
                    u.Curriculo = null;

                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    ent.DeleteObject(ud);
                    ent.SaveChanges();
                }
                CV_Bind();
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DeleteFile");
            }
        }

        protected void CV_GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CV_GridView.EditIndex = e.NewEditIndex;
            CV_Bind();
        }

        protected void CV_GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string data = ((TextBox)CV_GridView.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            DateTime val = Convert.ToDateTime(data);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = ent.UserDocuments.Where(x => x.ID == u.Curriculo).FirstOrDefault();
                ud.Validade = val;

                ent.SaveChanges();
            }

            CV_GridView.EditIndex = -1;
            CV_Bind();
        }

        protected void CV_GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CV_GridView.EditIndex = -1;
            CV_Bind();
        }

        #endregion

        #region CERTIFICADO HABILITAÇÕES

        protected void CertHab_Bind()
        {
            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                CertHab_GridView.DataSource = ent.UserDocuments.Where(x => x.ID == u.CertHabilitacoes).ToList();
                CertHab_GridView.DataBind();
            }
        }

        protected void CertHab_UploadButton_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(CertHab_FileUpload.PostedFile.FileName);
            string contentType = CertHab_FileUpload.PostedFile.ContentType;

            byte[] bytes = new byte[CertHab_FileUpload.PostedFile.InputStream.Length];
            CertHab_FileUpload.PostedFile.InputStream.Read(bytes, 0, bytes.Length);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = new UserDocument();
                ud.Data = bytes;
                ud.ContentType = contentType;
                ud.Name = filename;

                ent.UserDocuments.AddObject(ud);

                u.CertHabilitacoes = ud.ID;

                ent.SaveChanges();
            }
            CertHab_Bind();
        }

        protected void CertHab_DownloadFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName, contentType;

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    bytes = ud.Data;
                    contentType = ud.ContentType;
                    fileName = ud.Name;

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.ContentType = contentType;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    HttpContext.Current.Response.BinaryWrite(bytes.ToArray());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DownloadFile");
            }
        }

        protected void CertHab_DeleteFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                    User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();
                    u.CertHabilitacoes = null;

                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    ent.DeleteObject(ud);
                    ent.SaveChanges();
                }
                CertHab_Bind();
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DeleteFile");
            }
        }

        protected void CertHab_GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CertHab_GridView.EditIndex = e.NewEditIndex;
            CertHab_Bind();
        }

        protected void CertHab_GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string data = ((TextBox)CertHab_GridView.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            DateTime val = Convert.ToDateTime(data);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = ent.UserDocuments.Where(x => x.ID == u.CertHabilitacoes).FirstOrDefault();
                ud.Validade = val;

                ent.SaveChanges();
            }

            CertHab_GridView.EditIndex = -1;
            CertHab_Bind();
        }

        protected void CertHab_GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CertHab_GridView.EditIndex = -1;
            CertHab_Bind();
        }

        #endregion

        #region REGISTO CRIMINAL

        protected void RegCrim_Bind()
        {
            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                RegCrim_GridView.DataSource = ent.UserDocuments.Where(x => x.ID == u.RegistoCriminal).ToList();
                RegCrim_GridView.DataBind();
            }
        }

        protected void RegCrim_UploadButton_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(RegCrim_FileUpload.PostedFile.FileName);
            string contentType = RegCrim_FileUpload.PostedFile.ContentType;

            byte[] bytes = new byte[RegCrim_FileUpload.PostedFile.InputStream.Length];
            RegCrim_FileUpload.PostedFile.InputStream.Read(bytes, 0, bytes.Length);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = new UserDocument();
                ud.Data = bytes;
                ud.ContentType = contentType;
                ud.Name = filename;

                ent.UserDocuments.AddObject(ud);

                u.RegistoCriminal = ud.ID;

                ent.SaveChanges();
            }
            RegCrim_Bind();
        }

        protected void RegCrim_DownloadFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName, contentType;

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    bytes = ud.Data;
                    contentType = ud.ContentType;
                    fileName = ud.Name;

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.ContentType = contentType;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    HttpContext.Current.Response.BinaryWrite(bytes.ToArray());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DownloadFile");
            }
        }

        protected void RegCrim_DeleteFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);

            try
            {
                using (AM_Entities ent = new AM_Entities())
                {
                    long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                    User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();
                    u.RegistoCriminal = null;

                    UserDocument ud = ent.UserDocuments.Where(x => x.ID == id).FirstOrDefault();

                    ent.DeleteObject(ud);
                    ent.SaveChanges();
                }
                RegCrim_Bind();
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "Funcionarios_DeleteFile");
            }
        }

        protected void RegCrim_GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RegCrim_GridView.EditIndex = e.NewEditIndex;
            RegCrim_Bind();
        }

        protected void RegCrim_GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string data = ((TextBox)RegCrim_GridView.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            DateTime val = Convert.ToDateTime(data);

            using (AM_Entities ent = new AM_Entities())
            {
                long? UserId = long.Parse(Context.Request.QueryString["ID"]);

                User u = ent.Users.Where(x => x.ID == UserId).FirstOrDefault();

                UserDocument ud = ent.UserDocuments.Where(x => x.ID == u.RegistoCriminal).FirstOrDefault();
                ud.Validade = val;

                ent.SaveChanges();
            }

            RegCrim_GridView.EditIndex = -1;
            RegCrim_Bind();
        }

        protected void RegCrim_GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RegCrim_GridView.EditIndex = -1;
            RegCrim_Bind();
        }

        #endregion
    }
}