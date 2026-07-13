-- Purchase Bill Database Schema
-- Run this script against a SQL Server instance to create the required database and table.
-- Matches the schema actually produced by the application's EF Core migrations.

CREATE DATABASE PurchaseBillDb;
GO

USE PurchaseBillDb;
GO

CREATE TABLE dbo.Location_Details (
    Id            INT IDENTITY(1,1) NOT NULL,
    Location_Code NVARCHAR(MAX)     NOT NULL,
    Location_Name NVARCHAR(MAX)     NOT NULL,
    CreatedAt     DATETIME2         NOT NULL,
    CONSTRAINT PK_Location_Details PRIMARY KEY CLUSTERED (Id ASC)
);
GO