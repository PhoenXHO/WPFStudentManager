-- Active: 1734040078618@@127.0.0.1@3306@studentmanager
DROP TABLE IF EXISTS sessions;
DROP TABLE IF EXISTS students;
DROP TABLE IF EXISTS majors;
DROP TABLE IF EXISTS users;



CREATE TABLE Users (
	id INT AUTO_INCREMENT PRIMARY KEY,
	username VARCHAR(255) NOT NULL,
	password VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO users (username, password) VALUES ('admin', 'admin');



CREATE TABLE Majors (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
	Responsable VARCHAR(255) NOT NULL
);

INSERT INTO Majors (Name, Description, Responsable)
VALUES ('Computer Science', 'The study of computers and computational systems', 'John Doe'),
	   ('Mathematics', 'The study of numbers, quantity, and space', 'Jane Smith'),
	   ('Physics', 'The study of matter, energy, and the fundamental forces of nature', 'Albert Einstein'),
	   ('Biology', 'The study of living organisms and their interactions with each other and their environments', 'Charles Darwin'),
	   ('Chemistry', 'The study of the composition, structure, properties, and reactions of matter', 'Marie Curie');



CREATE TABLE Students (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    MajorId INT,
    DateOfBirth DATE,
    FOREIGN KEY (MajorId) REFERENCES Majors(Id) ON DELETE CASCADE
);

INSERT INTO Students (FirstName, LastName, Email, MajorId, DateOfBirth)
VALUES ('John', 'Doe', '...', 1, '1990-01-01'),
	   ('Jane', 'Smith', '...', 2, '1991-02-02'),
	   ('Alice', 'Johnson', '...', 3, '1992-03-03'),
	   ('Bob', 'Brown', '...', 4, '1993-04-04'),
	   ('Charlie', 'White', '...', 5, '1994-05-05'),
	   ('David', 'Black', '...', 1, '1995-06-06'),
	   ('Eve', 'Green', '...', 2, '1996-07-07'),
	   ('Frank', 'Blue', '...', 3, '1997-08-08'),
	   ('Grace', 'Red', '...', 4, '1998-09-09'),
	   ('Henry', 'Yellow', '...', 5, '1999-10-10'),
	   ('Isabel', 'Purple', '...', 1, '2000-11-11'),
	   ('Jack', 'Orange', '...', 2, '2001-12-12'),
	   ('Kelly', 'Pink', '...', 3, '2002-01-01'),
	   ('Larry', 'Brown', '...', 4, '2003-02-02'),
	   ('Molly', 'White', '...', 5, '2004-03-03');



CREATE TABLE Sessions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    User_id INT,
    Username VARCHAR(255),
    Expiry TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(Id) ON DELETE CASCADE
);