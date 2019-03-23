using System;
using System.Data.SqlClient;
using System.Data;
using Controllers;

namespace ConsoleApplication1.Clases
{
    public class Product
    {
        private int id;
        private decimal price;
        private int id_categ;
        private string name;
        private string description;
        private string presentation;
        private int estado;

        public int Id { get { return id; } set { id = value; } }
        public decimal Price { get { return price; } set { price = value; } }
        public int Id_categ { get { return id_categ; } set { id_categ = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string Presentation { get { return presentation; } set { presentation = value; } }
        public int Estado { get { return estado; } set { estado = value; } }


        public Product()
        {
            Estado = (int)Sistem.EnumEstados.ALTA;
        }

        public Product(int id, decimal price, int id_categ, string name, string description, string presentation, int estado)
        {
            Id = id;
            Price = price;
            Id_categ = id_categ;
            Name = name;
            Description = description;
            Presentation = presentation;
            Estado = estado;
        }

        protected bool GetProduct(int id, SqlConnection conn = null, SqlTransaction transac = null)
        {
            bool result = false;
            try
            {
                SqlCommand cmd;
                if (conn != null)
                    cmd = new SqlCommand("SP_GET_PRODUCT", conn);
                else
                    cmd = new SqlCommand("SP_GET_PRODUCT", Sistem.GetSqlConnection());
                if (transac != null)
                    cmd.Transaction = transac;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("id"))) Id = Convert.ToInt32(dr.GetInt32(dr.GetOrdinal("id")));
                    if (!dr.IsDBNull(dr.GetOrdinal("price"))) Price = Convert.ToDecimal(dr.GetInt32(dr.GetOrdinal("price")));
                    if (!dr.IsDBNull(dr.GetOrdinal("id_categ"))) Id_categ = Convert.ToInt32(dr.GetInt32(dr.GetOrdinal("id_categ")));
                    if (!dr.IsDBNull(dr.GetOrdinal("name"))) Name = Convert.ToString(dr.GetInt32(dr.GetOrdinal("name")));
                    if (!dr.IsDBNull(dr.GetOrdinal("description"))) Description = Convert.ToString(dr.GetInt32(dr.GetOrdinal("description")));
                    if (!dr.IsDBNull(dr.GetOrdinal("presentation"))) Presentation = Convert.ToString(dr.GetInt32(dr.GetOrdinal("presentation")));
                    Estado = (int)Sistem.EnumEstados.NA;
                }
                result = true;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "Product.GetProduct(int id)", true);
            }
            return result;
        }

        protected bool Put(SqlConnection conection, SqlTransaction transac = null)
        {
            bool result = false;
            if (Estado != (int)Sistem.EnumEstados.NA)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (transac != null)
                        cmd.Transaction = transac;

                    switch (Estado)
                    {
                        case (int)Sistem.EnumEstados.ALTA:
                            cmd.CommandText = "SP_PRODUT_INSERT";
                            break;
                        case (int)Sistem.EnumEstados.MODIFICACION:
                            cmd.CommandText = "SP_PRODUT_UPDATE";
                            break;
                        case (int)Sistem.EnumEstados.BAJA:
                            cmd.CommandText = "SP_PRODUT_DELETE";
                            break;
                    }

                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = Id;
                    if (Estado != (int)Sistem.EnumEstados.BAJA)
                    {
                        cmd.Parameters.Add("@price", System.Data.SqlDbType.Money).Value = Price;
                        cmd.Parameters.Add("@id_categ", System.Data.SqlDbType.Int).Value = Id_categ;
                        cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = Name;
                        cmd.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = Description;
                        cmd.Parameters.Add("@presentation", System.Data.SqlDbType.VarChar).Value = Presentation;
                    }
                    cmd.ExecuteNonQuery();
                    result = true;
                    Sistem.WriteLog(cmd.CommandText, "Product.Put(SqlConnection conection, SqlTransaction transac = null)");
                }
                catch (Exception ex)
                {
                    Sistem.WriteLog(ex, "Product.Put(SqlConnection conection, SqlTransaction transac = null)", true);
                }
            }
            else
                result = true;

            return result;
        }

    }
}
