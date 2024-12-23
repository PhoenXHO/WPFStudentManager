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
    Email VARCHAR(255),
    MajorId INT NOT NULL,
    DateOfBirth DATE NOT NULL,
	Picture VARCHAR(255) NOT NULL,
    FOREIGN KEY (MajorId) REFERENCES Majors(Id) ON DELETE CASCADE
);

INSERT INTO Students (FirstName, LastName, Email, MajorId, DateOfBirth, Picture)
VALUES ('John', 'Doe', '...', 1, '1990-01-01', 'https://images.pexels.com/photos/10759377/pexels-photo-10759377.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Jane', 'Smith', '...', 2, '1991-02-02', 'https://images.pexels.com/photos/1972531/pexels-photo-1972531.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Alice', 'Johnson', '...', 3, '1992-03-03', 'https://images.pexels.com/photos/27084016/pexels-photo-27084016/free-photo-of-a-bearded-lizard-on-a-black-background.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Bob', 'Brown', '...', 4, '1993-04-04', 'https://images.pexels.com/photos/5967959/pexels-photo-5967959.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Charlie', 'White', '...', 5, '1994-05-05', 'https://images.pexels.com/photos/29787865/pexels-photo-29787865/free-photo-of-playful-gray-cat-in-sunlight-portrait.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('David', 'Black', '...', 1, '1995-06-06', 'https://images.pexels.com/photos/5051699/pexels-photo-5051699.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Eve', 'Green', '...', 2, '1996-07-07', 'https://images.pexels.com/photos/20743507/pexels-photo-20743507/free-photo-of-portrait-of-squirrel-with-acorn.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Frank', 'Blue', '...', 3, '1997-08-08', 'https://images.pexels.com/photos/12561241/pexels-photo-12561241.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Grace', 'Red', '...', 4, '1998-09-09', 'https://images.pexels.com/photos/13161251/pexels-photo-13161251.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Henry', 'Yellow', '...', 5, '1999-10-10', 'https://images.pexels.com/photos/5468588/pexels-photo-5468588.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Isabel', 'Purple', '...', 1, '2000-11-11', 'https://images.pexels.com/photos/19658781/pexels-photo-19658781/free-photo-of-close-up-of-a-grey-monkey.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Jack', 'Orange', '...', 2, '2001-12-12', 'https://images.pexels.com/photos/18121934/pexels-photo-18121934/free-photo-of-head-of-a-ground-squirrel.png?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Kelly', 'Pink', '...', 3, '2002-01-01', 'https://images.pexels.com/photos/20434053/pexels-photo-20434053/free-photo-of-eurasian-blue-tit-perching-on-a-branch.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Larry', 'Brown', '...', 4, '2003-02-02', 'https://images.pexels.com/photos/13299693/pexels-photo-13299693.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'),
	   ('Molly', 'White', '...', 5, '2004-03-03', 'https://images.pexels.com/photos/15545052/pexels-photo-15545052/free-photo-of-close-up-of-a-red-legged-seriema-head.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1');



CREATE TABLE Sessions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    User_id INT,
    Username VARCHAR(255),
    Expiry TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(Id) ON DELETE CASCADE
);