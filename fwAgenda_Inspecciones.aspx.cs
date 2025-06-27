using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwAgenda_Inspecciones : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    DesHabilitar_Controles();

                    GetResponsable();

                    if (DateInput.Text == "")
                    {
                        DateInput.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    DateTime fecha = DateTime.ParseExact(DateInput.Text, "yyyy-MM-dd", null);

                    int year = fecha.Year;
                    int month = fecha.Month;
                    int day = fecha.Day;

                    DayPilotCalendar1.StartDate = DayPilot.Utils.Week.FirstDayOfWeek(new DateTime(year, month, day));
                    DayPilotCalendar1.DataSource = dbGetEvents(DayPilotCalendar1.StartDate, DayPilotCalendar1.Days);
                    DayPilotCalendar1.DataBind();

                }

                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }

            //int IdResponsable = Convert.ToInt32(ddlResponsable.SelectedValue);
            //GetDatos_Agenda(IdResponsable, "Responsable");

            //* * Agrega THEAD y TBODY a GridView.
            // GrdArch_Agenda.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        private DataTable dbGetEvents(DateTime start, int days)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            int IdResponsable = Convert.ToInt32(ddlResponsable.SelectedValue);

            //string strQuery = "SELECT Id_Inspecciones], [Ref_Siniestro] + '/' + [Nom_Asegurado] as Ref_Siniestro, [eventstart], [eventend] " +
            //                  "  FROM ITM_61] " +
            //                  " WHERE NOT (([eventend] <= @start) OR ([eventstart] >= @end))";

            //if (IdResponsable != 0)
            //{
            //    strQuery = strQuery + " AND [IdResponsable_Inspeccion] = " + IdResponsable + " ";
            //}

            string strQuery = "SELECT Id_Inspecciones, CONCAT(Ref_Siniestro, '/', Nom_Asegurado) AS Ref_Siniestro, eventstart, eventend " +
                              "  FROM ITM_61 " +
                              " WHERE NOT ((eventend <= @start) OR (eventstart >= @end))";

            if (IdResponsable != 0)
            {
                strQuery = strQuery + " AND IdResponsable_Inspeccion = " + IdResponsable + " ";
            }

            MySqlCommand cmd = new MySqlCommand(strQuery, dbConn.Connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            da.SelectCommand.Parameters.AddWithValue("start", start);
            da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days));

            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt;
        }

        protected void Page_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Variables.wUserName = string.Empty;
            Variables.wPassword = string.Empty;
        }

        protected void GetResponsable()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdResponsable_Inspeccion, Nom_Responsable " +
                                        " FROM ITM_64 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                ddlResponsable.DataSource = dt;

                ddlResponsable.DataValueField = "IdResponsable_Inspeccion";
                ddlResponsable.DataTextField = "Nom_Responsable";

                ddlResponsable.DataBind();
                ddlResponsable.Items.Insert(0, new ListItem("Visualizar Todo..", "0"));

                dbConn.Close();

                //Conecta.Cerrar();
                //cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetDatos_Agenda(int iValor, string sColumna)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sFecIni = string.Empty;
                string sFecFin = string.Empty;

                // Cadena de fecha en formato "yyyy-MM-dd"
                //string fechaString = FecProgramada.Value;

                string fechaString = DateInput.Text;

                // Especifica el formato de la fecha original
                string formatoOriginal = "yyyy-MM-dd";

                // Convierte la cadena de fecha al tipo DateTime
                DateTime fecha;

                if (DateTime.TryParseExact(fechaString, formatoOriginal, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                {
                    // Convierte el DateTime al formato deseado "dd/MM/yyyy"
                    sFecIni = fecha.ToString("dd/MM/yyyy");

                    DateTime fechaFin = fecha.AddDays(14);
                    sFecFin = fechaFin.ToString("dd/MM/yyyy");

                    Console.WriteLine("Fecha en nuevo formato: " + fechaFin);
                }
                else
                {
                    Console.WriteLine("Formato de fecha incorrecto");
                }

                string strQuery = "SELECT t0.Fecha_Programada, t1.Hora, t0.Ref_Siniestro, t3.Nom_Responsable, t2.NomTpo_Inspeccion " +
                                  "  FROM ITM_61 t0, ITM_62 t1, ITM_63 t2, ITM_64 t3" +
                                  " WHERE t0.IdHora_Programada = t1.IdHora_Programada" +
                                  "  AND t0.IdTpo_Inspeccion = t2.IdTpo_Inspeccion " +
                                  "  AND t0.IdResponsable_Inspeccion = t3.IdResponsable_Inspeccion ";

                if (iValor != 0)
                {
                    strQuery = strQuery + "  AND t0.IdResponsable_Inspeccion = " + iValor + " ";
                }

                strQuery = strQuery + "  AND t0.fecha_programada >= '" + sFecIni + "' " +
                                      "  AND t0.fecha_programada <= '" + sFecFin + "' " +
                                      " ORDER BY t0.Fecha_Programada, t0.IdHora_Programada ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdArch_Agenda.ShowHeaderWhenEmpty = true;
                    GrdArch_Agenda.EmptyDataText = "No hay resultados.";
                }

                GrdArch_Agenda.DataSource = dt;
                GrdArch_Agenda.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdArch_Agenda.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();

                //Conecta.Cerrar();
                //cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void GrdArch_Agenda_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdArch_Agenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdArch_Agenda_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DesHabilitar_Controles()
        {

        }

        protected void GrdArch_Agenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlResponsable_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.ParseExact(DateInput.Text, "yyyy-MM-dd", null);

            int year = fecha.Year;
            int month = fecha.Month;
            int day = fecha.Day;

            DayPilotCalendar1.StartDate = DayPilot.Utils.Week.FirstDayOfWeek(new DateTime(year, month, day));
            DayPilotCalendar1.DataSource = dbGetEvents(DayPilotCalendar1.StartDate, DayPilotCalendar1.Days);
            DayPilotCalendar1.DataBind();
        }

        protected void DateInput_TextChanged(object sender, EventArgs e)
        {

            DateTime fecha = DateTime.ParseExact(DateInput.Text, "yyyy-MM-dd", null);

            int year = fecha.Year;
            int month = fecha.Month;
            int day = fecha.Day;

            DayPilotCalendar1.StartDate = DayPilot.Utils.Week.FirstDayOfWeek(new DateTime(year, month, day));
            DayPilotCalendar1.DataSource = dbGetEvents(DayPilotCalendar1.StartDate, DayPilotCalendar1.Days);
            DayPilotCalendar1.DataBind();
        }

        public class AgendaItem
        {
            public string Time { get; set; }
            public string Monday { get; set; }
            public string Tuesday { get; set; }

            // Propriedades para otros dias de la semana
            // ...
        }

        public class Evento
        {
            public string Fecha { get; set; }
            public string Hora { get; set; }
            public string Descripcion { get; set; }
        }

        protected void DayPilotCalendar1_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
        {
            switch (e.Command)
            {
                case "navigate":
                    DateTime start = (DateTime)e.Data["start"];
                    DayPilotCalendar1.StartDate = start;
                    DayPilotCalendar1.DataSource = dbGetEvents(DayPilotCalendar1.StartDate, DayPilotCalendar1.Days);
                    DayPilotCalendar1.DataBind();
                    // DayPilotCalendar1.Update(DayPilot.Web.Ui.Enums.CallBackUpdateType.Full);
                    break;
            }
        }

        protected void DayPilotCalendar1_EventMove(object sender, DayPilot.Web.Ui.Events.EventMoveEventArgs e)
        {
            // dbUpdateEvent(e.Value, e.NewStart, e.NewEnd);
            DayPilotCalendar1.DataSource = dbGetEvents(DayPilotCalendar1.StartDate, DayPilotCalendar1.Days);
            DayPilotCalendar1.DataBind();
            DayPilotCalendar1.Update();
        }

        protected void BtnAgendarCita_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwAlta_Inspecciones.aspx", true);
        }

    }
}