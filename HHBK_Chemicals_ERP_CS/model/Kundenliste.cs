﻿using HHBK_Chemicals_ERP_CS.model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HHBK_Chemicals_ERP_CS
{
    public class Kundenliste : IKundenliste
    {
        private List<Kunde> kunden = new List<Kunde>();

        //private string myConnectionString = "server=127.0.0.1;uid=erpModel;pwd=555HHBK;database=HHBK_Chemicals;";
        private MySqlConnection conn=new MySqlConnection("server=127.0.0.1;uid=erpModel;pwd=555HHBK;database=HHBK_Chemicals;");
        private MySqlCommand mycommand;

        public Kundenliste()
        {
            this.refresh();
        }

        List<Kunde> IKundenliste.Kunden { get { this.refresh(); return kunden; }}

        void IKundenliste.alter(Kunde kunde)
        {
            if (kunde.Kundennummer != -1)
                mycommand.CommandText = Commands.change(kunde);
            else
                mycommand.CommandText = Commands.newEntity(kunde);

            MessageBox.Show(mycommand.CommandText);


            conn.Open();
            try
            {
                mycommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }

            //Über Event lösen!
            /*
            IModel a = this;
            view.Show(a.getKunden());
            */
        }

        void IKundenliste.delete(Kunde kunde)
        {
           mycommand.CommandText = Commands.delete(kunde);

           MessageBox.Show(mycommand.CommandText);


            conn.Open();
            try
            {
                mycommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }

        Kunde IKundenliste.getKunde(int kundennummer)
        {
            
            foreach(Kunde k in kunden)
            {
                if (k.Kundennummer == kundennummer)
                    return k;

            }
            return null;
        }

        void IKundenliste.save(Kunde kunde)
        {
            if (kunde.Kundennummer != -1)
                mycommand.CommandText = Commands.change(kunde);
            else
                mycommand.CommandText = Commands.newEntity(kunde);

            MessageBox.Show(mycommand.CommandText);


            conn.Open();
            try
            {
                mycommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }

        private void refresh()
        {
            Kunde kunde1 = new Kunde();
            mycommand = conn.CreateCommand();
            mycommand.CommandText = "Select * from kunde";

            conn.Open();

            MySqlDataReader reader = mycommand.ExecuteReader();

            while (reader.Read())
            {
                kunde1.Kundennummer = Convert.ToInt32(reader["kundennummer"]);
                kunde1.Name = reader["name"].ToString();
                kunde1.Vorname = reader["vorname"].ToString();
                kunde1.Strasse = reader["strasse"].ToString();
                kunde1.Hausnummer = reader["hausnummer"].ToString();
                kunde1.Ort = reader["ort"].ToString();
                kunde1.Postleitzahl = Convert.ToInt32(reader["postleitzahl"]);
                kunde1.Emailadresse = reader["emailadresse"].ToString();

                kunden.Add(kunde1);

            }
            reader.Close();
            conn.Close();
            //kunde1 = new Kunde(kundennummer, name, vorname, strasse, hausnummer, ort, postleitzahl, emailadresse);
        }

        void IKundenliste.generate()
        {
            refresh();
        }
    }
}