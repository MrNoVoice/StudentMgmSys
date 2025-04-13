using System;
using Dapper;
using MySql.Data.MySqlClient;

namespace StudenetMgmSys
{
    class DatabaseConnection
    {
        private string connectionString = $"server=127.0.0.1;user=root;database=studentmgmsys;port=3306;password={Environment.GetEnvironmentVariable("DB_PASSWORD")};SslMode=Preferred";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public MySqlConnection OpenConnection()
        {
            var connection = GetConnection();
            connection.Open();
            return connection;
        }
    }

    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string RollNumber { get; set; }
        public string Section { get; set; }

        public Student(string Name, string RollNumber, string Class, string Section)
        {
            this.Name = Name;
            this.RollNumber = RollNumber;
            this.Class = Class;
            this.Section = Section;
        }
    }

    public class StudentRepository
    {
        private DatabaseConnection db = new DatabaseConnection();

        // Method to add a student
        public void AddStudent(Student student)
        {
            using var connection = db.OpenConnection();
            string query = "INSERT INTO students (Name, RollNumber, Class, Section) VALUES (@Name, @RollNumber, @Class, @Section)";
            connection.Execute(query, student);
            Console.WriteLine("\nStudent added!");
        }

        // Method to add a subject
        public void AddSubject(string subjectName)
        {
            using var connection = db.OpenConnection();
            string query = "INSERT INTO subjects (Name) VALUES (@Name)";
            connection.Execute(query, new { Name = subjectName });
            Console.WriteLine("\nSubject added!");
        }

        // Method to add a result
        public void AddResult(int studentID, string subjectName, int marks)
        {
            using var connection = db.OpenConnection();

            // Fetch the subject ID based on the subject name
            string getSubjectIdQuery = "SELECT ID FROM subjects WHERE Name = @Name";
            var subjectId = connection.QueryFirstOrDefault<int>(getSubjectIdQuery, new { Name = subjectName });

            // Check if subject exists
            if (subjectId != 0)
            {
                // Now, check if the student exists
                string getStudentQuery = "SELECT ID FROM students WHERE ID = @StudentID";
                var studentExists = connection.QueryFirstOrDefault<int>(getStudentQuery, new { StudentID = studentID });

                if (studentExists != 0)
                {
                    // Insert the result if both student and subject exist
                    string query = "INSERT INTO results (StudentID, SubjectID, Marks) VALUES (@StudentID, @SubjectID, @Marks)";
                    connection.Execute(query, new { StudentID = studentID, SubjectID = subjectId, Marks = marks });
                    Console.WriteLine("Result added!");
                }
                else
                {
                    Console.WriteLine("\nStudent does not exist.");
                }
            }
            else
            {
                Console.WriteLine("\nSubject does not exist.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var repo = new StudentRepository();

            Console.WriteLine("Welcome to Student Management System");

            while (true)
            {
                Console.WriteLine("\n1. Add Student");
                Console.WriteLine("\n2. Add Subject");
                Console.WriteLine("\n3. Add Result");
                Console.WriteLine("\n4. Exit");
                Console.Write("\nEnter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        // Add Student
                        Console.Write("\nEnter Student Name: ");
                        string studentName = Console.ReadLine();

                        Console.Write("\nEnter Class: ");
                        string studentClass = Console.ReadLine();

                        Console.Write("\nEnter Roll Number: ");
                        string rollNumber = Console.ReadLine();

                        Console.Write("\nEnter Section: ");
                        string section = Console.ReadLine();

                        Console.Write("\nEnter Student ID: ");
                        int studentId = int.Parse(Console.ReadLine());

                        var newStudent = new Student(studentName, rollNumber, studentClass, section);
                        repo.AddStudent(newStudent);
                        break;

                    case 2:
                        // Add Subject
                        Console.Write("\nEnter Subject Name: ");
                        string subjectName = Console.ReadLine();
                        repo.AddSubject(subjectName);
                        break;

                    case 3:
                        // Add Result
                        Console.Write("\nEnter Student ID: ");
                        int studentIdForResult = int.Parse(Console.ReadLine());

                        Console.Write("\nEnter Subject Name: ");
                        string subjectForResult = Console.ReadLine();

                        Console.Write("\nEnter Marks: ");
                        int marks = int.Parse(Console.ReadLine());

                        repo.AddResult(studentIdForResult, subjectForResult, marks);
                        break;

                    case 4:
                        // Exit
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice! Please try again.");
                        break;
                }
            }
        }
    }
}
