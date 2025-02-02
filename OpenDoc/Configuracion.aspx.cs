﻿using System;
using System.Data;
using System.Web.UI;
using OpenDoc.Controllers;
using OpenDoc.Models;

namespace OpenDoc
{
    public partial class Configuracion : System.Web.UI.Page
    {
        private static string IDSelected;

        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario user = (Session["USER"] as Usuario);

            if (Request.QueryString["new_user"] != null)
            {
                PN_Success.Visible = true;
                LBL_Success.Text = "Usuario agregado con éxito";
            }

            if (Request.QueryString["edit_user"] != null)
            {
                PN_Success.Visible = true;
                LBL_Success.Text = "Usuario modificado con éxito";
            }

            if (user.tip_usuario != 0)
            {
                PN_ContainerForm.Visible = false;
                PN_Error.Visible = true;
                LBL_Error.Text = "No tienes acceso al área de configuración";
            }
        }

        protected void GV_Usuarios_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            IDSelected = e.KeyValue.ToString();

            if (e.CommandArgs.CommandName == "Editar")
            {
                Response.Redirect("/Usuarios/EditarUsuario.aspx?ID=" + IDSelected);
            }
            else if (e.CommandArgs.CommandName == "Eliminar")
            {
                string username = (GV_Usuarios.GetRow(e.VisibleIndex) as DataRowView).Row.ItemArray[2].ToString();
                LBL_Delete.Text = string.Format("¿Desea eliminar el Usuario {0}?", username);

                ScriptManager.RegisterStartupScript(this, GetType(), "modal", "openModalDelete()", true);
            }
        }

        protected void BTN_AgregarUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Usuarios/AgregarUsuario.aspx");
        }

        protected void BTN_EliminarUsuario_Click(object sender, EventArgs e)
        {
            int result = UsuarioController.Delete(int.Parse(IDSelected));

            if (result == 1)
            {
                PN_Success.Visible = true;
                LBL_Success.Text = "Usuario eliminado con éxito";
                GV_Usuarios.DataBind();
            }
            else
            {
                PN_Error.Visible = true;
                LBL_Error.Text = "Ha ocurrido un error. Ver tabla de Incidentes";
            }
        }
    }
}