using CompComponentsDB.Data;
using CompComponentsDB.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CompComponentsDB.Service
{
    public class ComponentService
    {
        public void Add(Component component)
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO dbo.Components (Name, Type, Supplier, Quantity, Cost, SupplyDate) " +
                    "VALUES (@Name, @Type, @Supplier, @Quantity, @Cost, @SupplyDate)", con);
                cmd.Parameters.AddWithValue("@Name", component.Name);
                cmd.Parameters.AddWithValue("@Type", component.Type);
                cmd.Parameters.AddWithValue("@Supplier", component.Supplier);
                cmd.Parameters.AddWithValue("@Quantity", component.Quantity);
                cmd.Parameters.AddWithValue("@Cost", component.Cost);
                cmd.Parameters.AddWithValue("@SupplyDate", component.SupplyDate);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Component> GetAll()
        {
            var list = new List<Component>();
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT * FROM dbo.Components ORDER BY Id", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Component
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString() ?? "",
                            Type = reader["Type"].ToString() ?? "",
                            Supplier = reader["Supplier"].ToString() ?? "",
                            Quantity = (int)reader["Quantity"],
                            Cost = (decimal)reader["Cost"],
                            SupplyDate = reader["SupplyDate"].ToString() ?? ""
                        });
                    }
                }
            }
            return list;
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("UPDATE dbo.Components SET Quantity = @Quantity WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("DELETE FROM dbo.Components WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<string> GetAllNames()
        {
            var names = new List<string>();
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT DISTINCT Name FROM dbo.Components", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        names.Add(reader.GetString(0));
                }
            }
            return names;
        }

        public List<string> GetAllSuppliers()
        {
            var suppliers = new List<string>();
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT DISTINCT Supplier FROM dbo.Components", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        suppliers.Add(reader.GetString(0));
                }
            }
            return suppliers;
        }

        public decimal GetMaxCost()
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT MAX(Cost) FROM dbo.Components", con);
                var result = cmd.ExecuteScalar();
                return result == System.DBNull.Value ? 0 : (decimal)result;
            }
        }

        public decimal GetMinCost()
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT MIN(Cost) FROM dbo.Components", con);
                var result = cmd.ExecuteScalar();
                return result == System.DBNull.Value ? 0 : (decimal)result;
            }
        }

        public decimal GetAverageCost()
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT AVG(Cost) FROM dbo.Components", con);
                var result = cmd.ExecuteScalar();
                return result == System.DBNull.Value ? 0 : (decimal)result;
            }
        }

        public int GetCountByType(string type)
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Components WHERE Type = @Type", con);
                cmd.Parameters.AddWithValue("@Type", type);
                return (int)cmd.ExecuteScalar();
            }
        }

        public Dictionary<string, int> GetCountBySupplier()
        {
            var dict = new Dictionary<string, int>();
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT Supplier, COUNT(*) FROM dbo.Components GROUP BY Supplier", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        dict[reader.GetString(0)] = reader.GetInt32(1);
                }
            }
            return dict;
        }

        public int GetCountBySupplier(string supplier)
        {
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Components WHERE Supplier = @Supplier", con);
                cmd.Parameters.AddWithValue("@Supplier", supplier);
                return (int)cmd.ExecuteScalar();
            }
        }

        public List<Component> GetComponentsBelowCost(decimal cost)
        {
            return GetComponentsByQuery("WHERE Cost < @Cost", new SqlParameter("@Cost", cost));
        }

        public List<Component> GetComponentsAboveCost(decimal cost)
        {
            return GetComponentsByQuery("WHERE Cost > @Cost", new SqlParameter("@Cost", cost));
        }

        public List<Component> GetComponentsInCostRange(decimal min, decimal max)
        {
            return GetComponentsByQuery("WHERE Cost BETWEEN @Min AND @Max",
                new SqlParameter("@Min", min), new SqlParameter("@Max", max));
        }

        public List<Component> GetComponentsBySuppliers(List<string> suppliers)
        {
            if (suppliers.Count == 0) return new List<Component>();
            var placeholders = string.Join(",", suppliers.Select((_, i) => $"@p{i}"));
            var parameters = suppliers.Select((s, i) => new SqlParameter($"@p{i}", s)).ToArray();
            return GetComponentsByQuery($"WHERE Supplier IN ({placeholders})", parameters);
        }

        private List<Component> GetComponentsByQuery(string whereClause, params SqlParameter[] parameters)
        {
            var list = new List<Component>();
            using (var con = DbConnector.GetConnection())
            {
                con.Open();
                var cmd = new SqlCommand($"SELECT * FROM dbo.Components {whereClause} ORDER BY Id", con);
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Component
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString() ?? "",
                            Type = reader["Type"].ToString() ?? "",
                            Supplier = reader["Supplier"].ToString() ?? "",
                            Quantity = (int)reader["Quantity"],
                            Cost = (decimal)reader["Cost"],
                            SupplyDate = reader["SupplyDate"].ToString() ?? ""
                        });
                    }
                }
            }
            return list;
        }
    }
}