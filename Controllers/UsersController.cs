using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlServerApi.Data;
using SqlServerApi.Models;
using System.Data.Entity;
using System.Data.SqlClient;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public UsersController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetUsersByName([FromQuery] string batchNo)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        List<User> users = new List<User>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = @"Select   Max(Customer) As Customer,Max(Batch_No) As BatchNo,ShadeNo,datediff(dd,Max(DyProduction.PrdDate),getdate()) AS Age ,Max(Machine) As Machine from DyProduction where PC='FULLY COMPLETE' And Batch_No in (Select Batch_No from DyOverAll Where Qty!='0')  And Batch_No=@batchNo Group By DyProduction.ShadeNo";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@batchNo", batchNo);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            customer = reader["Customer"].ToString(),
                            batchNo = reader["BatchNo"].ToString(),
                            shadeNo = reader["ShadeNo"].ToString(),
                            age = reader["Age"].ToString(),
                            machine = reader["Machine"].ToString(),
                        });
                    }
                }
            }
        }

        return Ok(users);
    }

    [HttpPost("InsertUser")]
    public IActionResult InsertUser([FromBody] Labappinsert newUser)
    {
        if (string.IsNullOrEmpty(newUser.batchNo))
            return BadRequest(new { message = "ShadeNo is required." });

        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string insertQuery = @"
            INSERT INTO DyLotAppLab (BatchNo, Option1,appBy, Remarks)
            VALUES (@batchNo, @option1, @appBy, @remarks)";

            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@batchNo", newUser.batchNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@option1", newUser.option1);
                cmd.Parameters.AddWithValue("@appBy", newUser.appBy);
                cmd.Parameters.AddWithValue("@remarks", newUser.remarks ?? (object)DBNull.Value);


                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return Ok(new { message = "User inserted successfully." });
                else
                    return StatusCode(500, new { message = "Insert failed." });
            }
        }
    }
  
}
