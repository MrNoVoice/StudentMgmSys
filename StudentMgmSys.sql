-- Create the database
CREATE DATABASE IF NOT EXISTS studentmgmsys;
USE studentmgmsys;

-- Students Table
CREATE TABLE Students (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    RollNumber VARCHAR(20) NOT NULL UNIQUE,
    Class VARCHAR(10) NOT NULL,
    Section VARCHAR(10) NOT NULL
);

-- Subjects Table
CREATE TABLE Subjects (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE
);

-- Results Table
CREATE TABLE Results (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    StudentID INT NOT NULL,
    SubjectID INT NOT NULL,
    Marks INT NOT NULL CHECK (Marks >= 0 AND Marks <= 100),
    FOREIGN KEY (StudentID) REFERENCES Students(ID) ON DELETE CASCADE,
    FOREIGN KEY (SubjectID) REFERENCES Subjects(ID) ON DELETE CASCADE
);
