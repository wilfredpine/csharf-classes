CREATE DATABASE OOPFrameworkDemo;
GO
USE OOPFrameworkDemo;
GO

CREATE TABLE dbo.Program (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Program_Name NVARCHAR(50) NOT NULL,
    Program_Description NVARCHAR(100) NULL
);
GO

CREATE TABLE dbo.Students (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Id_Number NVARCHAR(50) NOT NULL,
    First_Name NVARCHAR(50) NOT NULL,
    Middle_name NVARCHAR(50) NULL,
    Last_Name NVARCHAR(50) NOT NULL,
    Contact_Number NVARCHAR(30) NOT NULL,
    Birthday DATE NOT NULL,
    Program_Name NVARCHAR(50) NULL
);
GO

INSERT INTO dbo.Program (Program_Name, Program_Description)
VALUES (N'BSIT', N'Bachelor of Science in Information Technology');
GO
