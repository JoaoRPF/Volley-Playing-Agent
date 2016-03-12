using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Assets.GameManager
{
    public class DatabaseInit
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DatabaseInit()
        {
            server = "db.tecnico.ulisboa.pt";
            database = "ist176390";
            uid = "ist176390";
            password = "qwpx1337";
            string connectionString;
            connectionString = "Server=" + server + ";" + "Database=" +
                database + ";" + "Uid=" + uid + ";" + "Pwd=" + password + ";";

            connection = new MySqlConnection(connectionString);
            Debug.Log("Connection to the Database OK!");
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Debug.Log("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Debug.Log("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.Log(ex.Message);
                return false;
            }
        }

        public void Insert(string query)
        {
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public void Update(string query)
        {
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public void Delete(string query)
        {
            //string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public List<string>[] Select(string query)
        {
            //string query = "SELECT * FROM tableinfo";

            String[] separadores = { "FROM" };
            String[] parteQueryPorFrom = query.Split(separadores, StringSplitOptions.RemoveEmptyEntries);
            String[] argumentos = new String[5];
            int i = 0;

            while (true)
            {
                if (parteQueryPorFrom[0].Contains("*"))
                {
                    argumentos[0] = "id";
                    argumentos[1] = "distanciaX";
                    argumentos[2] = "distanciaY";
                    argumentos[3] = "distParede";
                    argumentos[4] = "toques";
                    i = 5;
                    break;
                }
                if (parteQueryPorFrom[0].Contains("id"))
                {
                    argumentos[i] = "id";
                    i++;
                }
                if (parteQueryPorFrom[0].Contains("distanciaX"))
                {
                    argumentos[i] = "distanciaX";
                    i++;
                }

                if (parteQueryPorFrom[0].Contains("distanciaY"))
                {
                    argumentos[i] = "distanciaY";
                    i++;
                }

                if (parteQueryPorFrom[0].Contains("distParede"))
                {
                    argumentos[i] = "distParede";
                    i++;
                }

                if (parteQueryPorFrom[0].Contains("toques"))
                {
                    argumentos[i] = "toques";
                    i++;
                }
                break;
            }

            //Create a list to store the result
            List<string>[] list = new List<string>[i];
            for (int j = 0; j < i; j++)
            {
                list[j] = new List<string>();
            }

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    String argumento = null;
                    for (int k = 0; k < i; k++)
                    {
                        argumento = argumentos[k];
                        list[k].Add(dataReader[argumento] + "");
                    }
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public string preparaStringInsert(string tabela, string[] colunas, string[] valores)
        {
            int tamanho = colunas.Length;
            string query = "INSERT INTO " + tabela + " ";
            string todasColunas = "";
            string todosValores = "";
            for (int i = 0; i < tamanho; i++)
            {
                if (i != tamanho - 1)
                    todasColunas = todasColunas + colunas[i] + ", ";
                else
                    todasColunas = todasColunas + colunas[i];
            }
            query = query + "(" + todasColunas + ")";
            for (int j = 0; j < tamanho; j++)
            {
                if (j != tamanho - 1)
                    todosValores = todosValores + valores[j] + ", ";
                else
                    todosValores = todosValores + valores[j];
            }
            query = query + "VALUES(" + todosValores + ")";
            Debug.Log("query = " + query);
            return query;
        }
    }
}
