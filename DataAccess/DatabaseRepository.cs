using Dapper;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.DataAccess
{
    public static class DatabaseRepository
    {
        private static readonly string StudentSelectBase = @"
            SELECT 
                s.Id, s.FirstName, s.LastName, s.Email, s.DateOfBirth, s.Picture, s.MajorId,
                m.Id as MajorId, m.Name, m.Description, m.Responsable
            FROM Students s
            LEFT JOIN Majors m ON s.MajorId = m.Id";

        public static async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            using var connection = DBConnection.GetConnection();
            return await connection.QueryAsync<Student, Major, Student>(
                StudentSelectBase,
                (student, major) =>
                {
                    student.Major = major;
                    return student;
                },
                splitOn: "MajorId"
            );
        }

        public static async Task<IEnumerable<Student>> GetStudentsByMajorAsync(string majorName)
        {
            using var connection = DBConnection.GetConnection();
            return await connection.QueryAsync<Student, Major, Student>(
                $"{StudentSelectBase} WHERE m.Name = @MajorName",
                (student, major) =>
                {
                    student.Major = major;
                    return student;
                },
                new { MajorName = majorName },
                splitOn: "MajorId"
            );
        }

        public static async Task<IEnumerable<Major>> GetAllMajorsAsync()
        {
            using var connection = DBConnection.GetConnection();
            return await connection.QueryAsync<Major>(
                "SELECT Id, Name, Description, Responsable FROM Majors"
            );
        }

        public static async Task<bool> AddStudentAsync(Student student)
        {
            using var connection = DBConnection.GetConnection();
            var sql = @"INSERT INTO Students (FirstName, LastName, Email, MajorId, DateOfBirth, Picture)
                       VALUES (@FirstName, @LastName, @Email, @MajorId, @DateOfBirth, @Picture);
                       SELECT LAST_INSERT_ID();";
            
            student.Id = await connection.ExecuteScalarAsync<int>(sql, student);
            return student.Id > 0;
        }

        public static async Task<bool> UpdateStudentAsync(Student student)
        {
            using var connection = DBConnection.GetConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, Email = @Email, MajorId = @MajorId, DateOfBirth = @DateOfBirth, Picture = @Picture WHERE Id = @Id",
                new
                {
                    student.FirstName,
                    student.LastName,
                    student.Email,
                    student.MajorId,
                    student.DateOfBirth,
                    student.Picture,
                    student.Id
                }
            );
            return rowsAffected > 0;
        }

        public static async Task<bool> DeleteStudentAsync(int id)
        {
            using var connection = DBConnection.GetConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM Students WHERE Id = @Id",
                new { Id = id }
            );
            return rowsAffected > 0;
        }

        public static async Task<bool> AddMajorAsync(Major major)
        {
            using var connection = DBConnection.GetConnection();
            var sql = @"INSERT INTO Majors (Name, Description, Responsable)
                       VALUES (@Name, @Description, @Responsable);
                       SELECT LAST_INSERT_ID();";
            
            major.MajorId = await connection.ExecuteScalarAsync<int>(sql, major);
            return major.MajorId > 0;
        }

        public static async Task<bool> UpdateMajorAsync(Major major)
        {
            using var connection = DBConnection.GetConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE Majors SET Name = @Name, Description = @Description, Responsable = @Responsable WHERE Id = @Id",
                major
            );
            return rowsAffected > 0;
        }

        public static async Task<bool> DeleteMajorAsync(int id)
        {
            using var connection = DBConnection.GetConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM Majors WHERE Id = @Id",
                new { Id = id }
            );
            return rowsAffected > 0;
        }
    }
}
