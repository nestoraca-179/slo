﻿using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SLO.Models;
using System.Reflection;

namespace SLO.Controllers
{
    public class ContenedorController : Repository
    {
        public static Contenedor GetByID(int id)
        {
            return db.Contenedor.Single(c => c.ID == id);
        }

        public static List<Contenedor> GetAllContsByBL(int id_bl)
        {
            return db.Contenedor.Where(c => c.id_bl == id_bl).ToList();
        }

        public static void Add(DataRow row, int id_bl, string user)
        {
            Contenedor cont = new Contenedor();

            try
            {
                cont.id_bl = id_bl;
                cont.num_cont = row.Field<string>(16);
                cont.num_paq = int.Parse(Regex.Match(row.Field<string>(19), @"\d+").Value);
                cont.tip_cont = GetContType(row.Field<string>(17));
                cont.estado = 5;
                cont.eq_inter_rec1 = row.Field<string>(20);
                cont.eq_inter_rec2 = row.Field<string>(24);
                cont.eq_inter_rec3 = row.Field<string>(25);
                cont.seal_party = "CR";
                cont.peso_neto = decimal.Parse(row.Field<string>(21));
                cont.peso_bruto = decimal.Parse(row.Field<string>(23));
                cont.tamanio = int.Parse(Regex.Match(row.Field<string>(17), @"\d+").Value);
                cont.temper = string.IsNullOrEmpty(row.Field<string>(27).Trim()) ? 0 : decimal.Parse(row.Field<string>(27).Trim());
                cont.imo = string.IsNullOrEmpty(row.Field<string>(26).Replace(" ", "")) ? "" : row.Field<string>(26).Split('/')[0].Split('-')[1].Trim();
                cont.num_un = row.Field<string>(26).Split('/')[0].Split('-')[0].Trim();
                cont.ventilac = null;
                cont.descrip_mer = null;
                cont.co_us_in = user;
                cont.fe_us_in = DateTime.Now;
                cont.co_us_mo = user;
                cont.fe_us_mo = DateTime.Now;

                Contenedor c = db.Contenedor.Add(cont);
                db.SaveChanges();

                LogController.CreateLog(user, "CONTENEDOR", c.ID, "I", null);
            }
            catch (Exception ex)
            {
                IncidentController.CreateIncident(string.Format("ERROR INSERTANDO CONTENEDOR N° {0}", cont.num_cont), ex);
                throw ex;
            }
        }

        public static int Add(Contenedor cont)
        {
            int result = 0;

            try
            {
                Contenedor c = db.Contenedor.Add(cont);
                db.SaveChanges();

                LogController.CreateLog(c.co_us_in, "CONTENEDOR", c.ID, "I", null);
                result = 1;
            }
            catch (Exception ex)
            {
                IncidentController.CreateIncident(string.Format("ERROR AGREGANDO CONTENEDOR N° {0}", cont.num_cont), ex);
            }

            return result;
        }

        public static int Edit(Contenedor cont)
        {
            int result = 0;

            try
            {
                Contenedor existing = GetByID(cont.ID);
                cont.id_bl = existing.id_bl;
                cont.num_cont = existing.num_cont;
                cont.co_us_in = existing.co_us_in;
                cont.fe_us_in = existing.fe_us_in;

                string campos = GetChanges(existing, cont);

                db.Entry(existing).CurrentValues.SetValues(cont);
                db.SaveChanges();

                LogController.CreateLog(cont.co_us_mo, "CONTENEDOR", cont.ID, "M", campos);
                result = 1;
            }
            catch (Exception ex)
            {
                IncidentController.CreateIncident(string.Format("ERROR MODIFICANDO VIAJE N° {0}", cont.num_cont), ex);
            }

            return result;
        }

        public static int Delete(int ID)
        {
            int result = 0;
            Contenedor cont = GetByID(ID);

            try
            {
                Contenedor c = db.Contenedor.Remove(cont);
                db.SaveChanges();

                LogController.CreateLog(c.co_us_in, "CONTENEDOR", c.ID, "E", null);
                result = 1;
            }
            catch (Exception ex)
            {
                IncidentController.CreateIncident(string.Format("ERROR ELIMINANDO CONTENEDOR N° {0}", cont.num_cont), ex);
            }

            return result;
        }

        private static string GetContType(string type)
        {
            string new_type = "";

            switch (type)
            {
                case "20DC":
                case "20ST":
                    new_type = "2000";
                    break;
                case "20RF":
                    new_type = "2032";
                    break;
                case "20OT":
                    new_type = "2050";
                    break;
                case "20FR":
                case "20FF":
                case "22G1":
                    new_type = "2260";
                    break;
                case "20TK":
                    new_type = "2270";
                    break;
                case "40DC":
                case "40ST":
                    new_type = "4000";
                    break;
                case "40OT":
                    new_type = "4050";
                    break;
                case "40FR":
                    new_type = "4060";
                    break;
                case "40HC":
                case "45G1":
                    new_type = "4400";
                    break;
                case "40RH":
                case "40RF":
                    new_type = "4432";
                    break;
            }

            return new_type;
        }

        private static string GetChanges(Contenedor cont_v, Contenedor cont_n)
        {
            string campos = "";
            Type type = new Contenedor().GetType();

            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.Name != "fe_us_in" && prop.Name != "fe_us_mo")
                {
                    string valor1 = prop.GetValue(cont_v) == null ? "" : prop.GetValue(cont_v).ToString();
                    string valor2 = prop.GetValue(cont_n) == null ? "" : prop.GetValue(cont_n).ToString();

                    if (valor1 != valor2)
                    {
                        campos += string.Format("{0}: {1} -> {2}; ", prop.Name, valor1, valor2);
                    }
                }
            }

            return campos;
        }
    }
}