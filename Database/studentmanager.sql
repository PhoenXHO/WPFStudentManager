-- Active: 1734040078618@@127.0.0.1@3306@studentmanager
CREATE TABLE users (
	id INT AUTO_INCREMENT PRIMARY KEY,
	username VARCHAR(255) NOT NULL,
	password VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO users (username, password) VALUES ('admin', 'admin');

SELECT * FROM users;

CREATE TABLE Majors (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT
);

CREATE TABLE Students (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    MajorId INT,
    DateOfBirth DATE,
    FOREIGN KEY (MajorId) REFERENCES Majors(Id) ON DELETE CASCADE
);

