using ADO.Net.Exceptions;
using ADO.Net.Models;
using System.Data.SqlClient;

//ADO.Net
//ORM - EntityFrameworkCore

//ConnectionString

string connectionString = @"Server=B3-0\BS101;Database=BB101;Trusted_Connection=True";

//AddGroup();
//GetAllGroups();
//GetGroupCount();

Console.WriteLine("Search:");
string search = Console.ReadLine();

try
{
    foreach (var item in SearchSizesByName(search))
    {
        Console.WriteLine(item.SizeValue);
    }
}
catch (NotFoundException ex)
{
    Console.WriteLine(ex.Message);
}

void AddGroup()
{
    using (SqlConnection connection = new(connectionString))
    {
        connection.Open();

        string query = "INSERT INTO Groups VALUES ('BB104')";

        SqlCommand command = new(query, connection);
        int result = command.ExecuteNonQuery(); //Insert, update, delete

        Console.WriteLine($"{result} rows affected");
    }
}

void GetAllGroups()
{
    using (SqlConnection connection = new(connectionString))
    {
        connection.Open();

        string query = "SELECT * FROM Groups";

        SqlCommand command = new(query, connection);
        SqlDataReader reader = command.ExecuteReader(); //Read (SELECT)

        while (reader.Read())
        {
            Console.WriteLine($"Id: {reader["Id"]} - Name: {reader["Name"]}");
        }
    }
}

void GetGroupCount()
{
    using (SqlConnection connection = new(connectionString))
    {
        connection.Open();

        string query = "SELECT COUNT(*) FROM Groups";

        SqlCommand command = new(query, connection);
        Console.WriteLine(command.ExecuteScalar());
    }
}



List<Size> SearchSizesByName(string search)
{
    List<Size> searchSizes = new List<Size>();
    using (SqlConnection connection = new(connectionString))
    {
        connection.Open();

        string query = $"SELECT * FROM Sizes WHERE Size LIKE '%' + @search + '%'";
        //string query = $"SELECT * FROM Sizes WHERE Size LIKE '%{search}%'";

        SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@search", search);
        SqlDataReader reader = command.ExecuteReader();
       
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Size size = new()
                {
                    Id = (int)reader["Id"],
                    SizeValue = (string)reader["Size"]
                };
                searchSizes.Add(size);
                //Console.WriteLine($"Id: {reader["Id"]} - Size: {reader["Size"]}");
            }
        }
        else
        {
            throw new NotFoundException("data not found");
        }
    }

    return searchSizes;
}