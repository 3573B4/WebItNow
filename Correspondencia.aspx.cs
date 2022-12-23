using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace WebItNow
{
	public partial class Correspondencia : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				//Session["UserId"] = 1;

				this.Form.Attributes.Add("autocomplete", "off");
				// Permisos Usuario
				string UsPrivilegios = System.Web.HttpContext.Current.Session["UsPrivilegios"] as String;

				if (UsPrivilegios == "User")
				{
					BtnActivar.Enabled = false;
					BtnActualizar.Enabled = false;
					BtnRegistrar.Enabled = false;
				}
			}

			//if (Session["UserId"] == null)
			//{
			//	LblMessage.Text = "La sesión ha expirado.";
			//	this.mpeMensaje.Show();

			//	Response.Redirect("Acceso.aspx");
			//}

		}

		protected void BtnActivar_Click(object sender, EventArgs e)
		{
			Txt_Observaciones_1.Enabled = true;
			Txt_Observaciones_2.Enabled = true;
			Txt_Observaciones_3.Enabled = true;
			Txt_Observaciones_4.Enabled = true;
			Txt_Observaciones_5.Enabled = true;
		}
		protected void BtnLimpiar_Click(object sender, EventArgs e)
		{
			Limpia(this.Controls);
		}

		protected void BtnBuscar_Click(object sender, EventArgs e)
		{
			if (Txt_Rfc.Text == "")
			{
				LblMessage.Text = "Debes captura Rfc.";
				this.mpeMensaje.Show();
			}
			else
			{

				ConexionBD Conecta = new ConexionBD();
				Conecta.Abrir();

				SqlCommand cmd1 = new SqlCommand("Select * From Correspondencia Where Rfc like '" + Txt_Rfc.Text + "%'", Conecta.ConectarBD);
				SqlDataReader dr1 = cmd1.ExecuteReader();

				if (dr1.HasRows)
				{
					while (dr1.Read())
					{
						Busca_Incorporacion();

						Txt_Oficio_1.Text = dr1["Oficio_1"].ToString().Trim();
						Txt_Folio_1.Text = dr1["Folio_1"].ToString().Trim();
						Txt_Observaciones_1.Text = dr1["Observacion_1"].ToString().Trim();
						Txt_Oficio_2.Text = dr1["Oficio_2"].ToString().Trim();
						Txt_Folio_2.Text = dr1["Folio_2"].ToString().Trim();
						Txt_Observaciones_2.Text = dr1["Observacion_2"].ToString().Trim();
						Txt_Oficio_3.Text = dr1["Oficio_3"].ToString().Trim();
						Txt_Folio_3.Text = dr1["Folio_3"].ToString().Trim();
						Txt_Observaciones_3.Text = dr1["Observacion_3"].ToString().Trim();
						Txt_Oficio_4.Text = dr1["Oficio_4"].ToString().Trim();
						Txt_Folio_4.Text = dr1["Folio_4"].ToString().Trim();
						Txt_Observaciones_4.Text = dr1["Observacion_4"].ToString().Trim();
						Txt_Oficio_5.Text = dr1["Oficio_5"].ToString().Trim();
						Txt_Folio_5.Text = dr1["Folio_5"].ToString().Trim();
						Txt_Observaciones_5.Text = dr1["Observacion_5"].ToString().Trim();

						//	Variables.wMotivo = int.Parse(dr1["Motivo_Solicitud"].ToString());
						//	Variables.wEtapa = int.Parse(dr1["Etapa"].ToString());

						Busca_Motivo();
						Verifica_Etapa();
					}
				}
				else
				{
					Busca_Incorporacion();
					BtnActualizar.Visible = false;
				}

				Conecta.Cerrar();

			}
		}
		protected void BtnRegistrar_Click(object sender, EventArgs e)
		{
			if (Txt_Rfc.Text != "" && Txt_Nombre.Text != "")
			{
				if (Txt_Oficio_1.Text != "" && Txt_Folio_1.Text != "" && Txt_Observaciones_1.Text != "")
				{
					ConexionBD Conecta = new ConexionBD();
					Conecta.Abrir();

				}
				else
				{
					if (Txt_Oficio_1.Text == "")
					{
						LblMessage.Text = "Debes captura Oficio 1.";
					}
					else if (Txt_Folio_1.Text == "")
					{
						LblMessage.Text = "Debes captura Folio 1.";
					}
					else if (Txt_Observaciones_1.Text == "")
					{
						LblMessage.Text = "Debes captura Observaciones 1.";
					}

					this.mpeMensaje.Show();
				}
			}

		}
		protected void BtnSalir_Click(object sender, EventArgs e)
		{
			Response.Redirect("Menu.aspx");
		}
		public void Limpia(ControlCollection controles)
		{
			foreach (Control control in controles)
			{
				if (control is TextBox)
					((TextBox)control).Text = string.Empty;
				else if (control is DropDownList)
					((DropDownList)control).Items.Clear();
				else if (control is RadioButtonList)
					((RadioButtonList)control).ClearSelection();
				else if (control is CheckBoxList)
					((CheckBoxList)control).ClearSelection();
				else if (control is RadioButton)
					((RadioButton)control).Checked = false;
				else if (control is CheckBox)
					((CheckBox)control).Checked = false;
				else if (control.HasControls())
					//Esta linea detécta un Control que contenga otros Controles
					//Así ningún control se quedará sin ser limpiado.
					Limpia(control.Controls);
			}

			//			foreach (Control Campos_txt in this.Controls)
			//			{
			//				if (Campos_txt is TextBox)
			//				{
			//					(Campos_txt as TextBox).Text = string.Empty;
			//					this.Txt_Rfc.Focus();
			//				}
			//			}

			//			foreach (Control Campos_cbo in this.Controls)
			//			{
			//				if (Campos_cbo is DropDownList)
			//				{
			//					(Campos_cbo as DropDownList).Text = string.Empty;
			//				}
			//			}

			Txt_Oficio_1.Enabled = true;
			Txt_Folio_1.Enabled = true;
			Txt_Observaciones_1.Enabled = true;
			Txt_Oficio_2.Enabled = true;
			Txt_Folio_2.Enabled = true;
			Txt_Observaciones_2.Enabled = true;
			Txt_Oficio_3.Enabled = true;
			Txt_Folio_3.Enabled = true;
			Txt_Observaciones_3.Enabled = true;
			Txt_Oficio_4.Enabled = true;
			Txt_Folio_4.Enabled = true;
			Txt_Observaciones_4.Enabled = true;
			Txt_Oficio_5.Enabled = true;
			Txt_Folio_5.Enabled = true;
			Txt_Observaciones_5.Enabled = true;
			Cbo_Ciclo.Enabled = true;
			Cbo_Periodo.Enabled = true;
			Cbo_Motivos.Enabled = true;

			BtnRegistrar.Visible = true;
			BtnActualizar.Visible = true;

		}
		public void Busca_Motivo()
		{

		}
		public void Busca_Incorporacion()
		{
			ConexionBD Conecta = new ConexionBD();
			Conecta.Abrir();

			SqlCommand cmd1 = new SqlCommand("Select * From Incorporados Where Rfc like '" + Txt_Rfc.Text + "%'", Conecta.ConectarBD);
			SqlDataReader dr1 = cmd1.ExecuteReader();

			if (dr1.HasRows)
			{
				while (dr1.Read())
				{
					Txt_Rfc.Text = dr1["Rfc"].ToString().Trim();
					Txt_Nombre.Text = dr1["Nombre"].ToString().Trim() + " " + dr1["Paterno"].ToString().Trim() + " " + dr1["Materno"].ToString().Trim();
					Cbo_Ciclo.Text = dr1["Ciclo"].ToString().Trim();

					//	Variables.wPeriodo = int.Parse(dr1["Periodo"].ToString());
					Variables.wEstatus = int.Parse(dr1["Estatus"].ToString());

					Busca_Periodo();
					Busca_Ciclo();
					Busca_Estatus();
					Busca_Motivo_Solicitud();
				}
			}
			else
			{
				Txt_Mensaje.Text = "Empleado no incorporado al Fonac ";
			}

			Conecta.Cerrar();
		}
		public void Verifica_Etapa()
		{

		}

		public void Busca_Periodo()
		{

		}

		public void Busca_Periodos()
		{
			ConexionBD Conecta = new ConexionBD();
			Conecta.Abrir();

			SqlCommand cmd1 = new SqlCommand("Select * From Periodo", Conecta.ConectarBD);
			SqlDataReader dr1 = cmd1.ExecuteReader();

			while (dr1.Read())
			{
				Cbo_Periodo.Items.Add(dr1["Descripcion"].ToString().Trim());
			}
			cmd1.Dispose();
			dr1.Dispose();

			Conecta.Cerrar();
		}

		public void Busca_Ciclo()
		{
			ConexionBD Conecta = new ConexionBD();
			Conecta.Abrir();

			SqlCommand cmd1 = new SqlCommand("Select * From Incorporados Where Rfc like '" + Txt_Rfc.Text + "%'", Conecta.ConectarBD);
			SqlDataReader dr1 = cmd1.ExecuteReader();

			while (dr1.Read())
			{
				Cbo_Ciclo.Items.Add(dr1["Ciclo"].ToString().Trim());
			}
			cmd1.Dispose();
			dr1.Dispose();

			Conecta.Cerrar();
		}

		public void Busca_Estatus()
		{
			ConexionBD Conecta = new ConexionBD();
			Conecta.Abrir();

			SqlCommand cmd1 = new SqlCommand("Select * From Estatus Where Estatus = " + Variables.wEstatus, Conecta.ConectarBD);
			SqlDataReader dr1 = cmd1.ExecuteReader();

			while (dr1.Read())
			{
				Txt_Estatus.Text = dr1["Descripcion"].ToString().Trim();
			}
			cmd1.Dispose();
			dr1.Dispose();

			Conecta.Cerrar();
		}

		public void Busca_Motivo_Solicitud()
		{
			ConexionBD Conecta = new ConexionBD();
			Conecta.Abrir();

			SqlCommand cmd1 = new SqlCommand("Select * From Motivo_Solicitud", Conecta.ConectarBD);
			SqlDataReader dr1 = cmd1.ExecuteReader();

			while (dr1.Read())
			{
				Cbo_Motivos.Items.Add(dr1["Descripcion"].ToString().Trim());
			}
			cmd1.Dispose();
			dr1.Dispose();

			Conecta.Cerrar();
		}
		protected void BtnActualizar_Click(object sender, EventArgs e)
		{
			if (Txt_Rfc.Text != "" && Txt_Nombre.Text != "")
			{
				ConexionBD Conecta = new ConexionBD();
				Conecta.Abrir();


				Conecta.Cerrar();

				Limpia(this.Controls);
			}
		}
		public void Llena_grdRfc(string StrRfc)
		{
			try
			{
				ConexionBD Conecta = new ConexionBD();
				Conecta.Abrir();

				SqlCommand cmd1 = new SqlCommand("Select IdUsuario, UsPrivilegios From tbUsuarios " +
				" Where UsPrivilegios =3", Conecta.ConectarBD);
				//		" Limit 10", Conecta.ConectarBD);
				SqlDataReader dr1 = cmd1.ExecuteReader();

				//  dr1.Load(reader);

				GrdRfc.DataSource = dr1;
				GrdRfc.DataBind();
			}
			catch (Exception ex)
			{
				Txt_Mensaje.Text = ex.Message;
			}
		}

		protected void BuscadorRfc_Click(object sender, ImageClickEventArgs e)
		{
			mpePopup.Show();

			Llena_grdRfc(Txt_Rfc.Text);
			Txt_Rfc.Focus();
		}

		protected void BtnBuscador_Click(object sender, EventArgs e)
		{
			mpePopup.Show();
			Llena_grdRfc(Txt_BuscadorRfc.Text);
		}
		protected void GrdRfc_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
			// Se obtiene la fila seleccionada del gridview
			//
			Txt_Rfc.Text = HttpUtility.HtmlDecode(GrdRfc.SelectedRow.Cells[0].Text);
			Txt_Nombre.Text = HttpUtility.HtmlDecode(GrdRfc.SelectedRow.Cells[1].Text);

		}
		protected void GrdRfc_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
				e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdRfc, "Select$" + Convert.ToString(e.Row.RowIndex), true);
		}

	}
}