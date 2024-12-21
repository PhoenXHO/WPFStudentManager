using MySql.Data.MySqlClient;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManager.Services
{
    public class StudentService
    {
        private readonly string _connectionString;

        // Constructor accepts the connection string to your database
        public StudentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to get all students from the database
        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Students", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = reader.GetInt32("Id"),
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            Email = reader.GetString("Email"),
                            Major = null, // Assuming Major is handled separately in the ViewModel
                            DateOfBirth = reader.GetDateTime("DateOfBirth")
                        });
                    }
                }
            }

            return students;
        }

        // Method to add a student to the database
        public void AddStudent(Student student)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO Students (FirstName, LastName, Email, MajorId, DateOfBirth) VALUES (@FirstName, @LastName, @Email, @MajorId, @DateOfBirth)", connection);

                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@MajorId", student.Major?.Id ?? 0); // Assuming 0 is the default value for no major
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);

                command.ExecuteNonQuery();
            }
        }

        // Method to delete a student by ID
        public void DeleteStudent(int studentId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM Students WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", studentId);

                command.ExecuteNonQuery();
            }
        }
    }
}
