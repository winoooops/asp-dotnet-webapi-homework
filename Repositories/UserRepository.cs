using System.Text;
using Microsoft.Extensions.Options;
using MyWebAPI.Configs;
using MyWebAPI.DTOs;
using MyWebAPI.Models;
using Npgsql;

namespace MyWebAPI.Repositories;

public class UserRepository
{
  private readonly string _connectionString;

  public UserRepository(IOptions<DBConfig> options)
  {
    _connectionString = options.Value.ConnectionString;
    CreateTable();
  }

  public void CreateTable()
  {
    using var conn = new NpgsqlConnection(_connectionString);    
    conn.Open();
    
    using var cmd = conn.CreateCommand();
    cmd.CommandText = @"
      CREATE TABLE IF NOT EXISTS user_table (
        id SERIAL PRIMARY KEY,
        username VARCHAR(50),
        email VARCHAR(100),
        address VARCHAR(100),
        gender INT
    );";
    cmd.ExecuteNonQuery();
    conn.Close();
  }

  public async Task<List<User>> GetAll()
  {
    using var conn = new NpgsqlConnection(_connectionString);
    await conn.OpenAsync();

    using var cmd = new NpgsqlCommand("SELECT * FROM user_table;", conn);

    using var reader = await cmd.ExecuteReaderAsync();
    var users = new List<User>();
    
    while (await reader.ReadAsync())
    {
      users.Add(new User
      {
        Id = reader.GetInt32(reader.GetOrdinal("id")),
        Username = reader.GetString(reader.GetOrdinal("username")),
        Email = reader.GetString(reader.GetOrdinal("email")),
        Address = reader.GetString(reader.GetOrdinal("address")), 
        Gender = reader.GetInt32(reader.GetOrdinal("gender")),
      });
    }

    return users;
  }

  public async Task<User?> GetUserById(int userId)
  {
    using var conn = new NpgsqlConnection(_connectionString);
    await conn.OpenAsync();

    using var cmd = new NpgsqlCommand($"Select * from user_table WHERE id={userId};", conn);

    using var reader = await cmd.ExecuteReaderAsync();
    if (await reader.ReadAsync())
    {
      return new User
      {
        Id = reader.GetInt32(reader.GetOrdinal("id")),
        Username = reader.GetString(reader.GetOrdinal("username")),
        Email = reader.GetString(reader.GetOrdinal("email")),
        Address = reader.GetString(reader.GetOrdinal("address")),
        Gender = reader.GetInt32(reader.GetOrdinal("gender")),
      };
    }

    return null;
  }

  public async Task<User?> CreateUser(UserDTO user)
  {
    await using var conn = new NpgsqlConnection(_connectionString);
    await conn.OpenAsync();

    await using var cmd =
      new NpgsqlCommand("INSERT INTO user_table (username, email, address, gender) VALUES ($1, $2, $3, $4) RETURNING *;", conn)
      {
        Parameters =
        {
          new() { Value = user.Username },
          new() { Value = user.Email },
          new() { Value = user.Address },
          new() { Value = user.Gender },
        }
      };

    await using var reader = await cmd.ExecuteReaderAsync();

    if (await reader.ReadAsync())
    {
      return new User
      {
        Id = reader.GetInt32(reader.GetOrdinal("id")),
        Username = reader.GetString(reader.GetOrdinal("username")),
        Email = reader.GetString(reader.GetOrdinal("email")),
        Address = reader.GetString(reader.GetOrdinal("address")),
        Gender = reader.GetInt32(reader.GetOrdinal("gender")),
      }; 
    }

    return null;
  }

  public async Task<User?> UpdateUser(int userId, string username, string email)
  {
    await using var conn = new NpgsqlConnection(_connectionString);
    await conn.OpenAsync();

    await using var cmd = new NpgsqlCommand($"UPDATE user_table SET username=$1, email=$2 WHERE id={userId} RETURNING *;", conn)
    {
      Parameters =
      {
        new() { Value = username },
        new() { Value = email }
      }
    };

    await using var reader = await cmd.ExecuteReaderAsync();
    

    if (await reader.ReadAsync())
    {
      return new User
      {
        Id = reader.GetInt32(reader.GetOrdinal("id")),
        Username = reader.GetString(reader.GetOrdinal("username")),
        Email = reader.GetString(reader.GetOrdinal("email")),
        Address = reader.GetString(reader.GetOrdinal("address")),
        Gender = reader.GetInt32(reader.GetOrdinal("gender")),
      };
    }

    return null;
  }

  public async Task<bool> DeleteUser(int userId)
  {
    using var conn = new NpgsqlConnection(_connectionString);
    await conn.OpenAsync();

    using var cmd = new NpgsqlCommand($"DELETE FROM user_table WHERE id={userId};", conn);
    
    int affectedRows = await cmd.ExecuteNonQueryAsync();
    return affectedRows > 0;
  }
}