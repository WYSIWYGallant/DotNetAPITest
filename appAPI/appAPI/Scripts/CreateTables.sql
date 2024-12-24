IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PersonType')
BEGIN
    CREATE TABLE PersonType (
        PersonTypeID INT PRIMARY KEY,      -- Unique identifier for each type of person
        PersonTypeDescription NVARCHAR(100) NOT NULL  -- Description of person type (supports Unicode characters)
    );
END;
GO
INSERT INTO PersonType (PersonTypeID, PersonTypeDescription)
VALUES
(1, 'Teacher'),
(2, 'Student')
-- Create the Person table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Person' AND xtype='U')
BEGIN
    CREATE TABLE Person (
        PersonID int IDENTITY(1,1) PRIMARY KEY,
        PersonName varchar(255) NOT NULL,
        PersonAge int NOT NULL,
        PersonType int NOT NULL,
        FOREIGN KEY (PersonType) REFERENCES PersonType(PersonTypeID)
    );
END;
GO
-- Insert default data into Person table
INSERT INTO Person (PersonName, PersonAge, PersonType)
VALUES
('Student001', 27, 2),
('Student002', 22, 2),
('Teacher003', 22, 1),
('Student004', 28, 2),
('Teacher005', 20, 1),
('Teacher006', 16, 1),
('Student007', 28, 2),
('Student008', 26, 2),
('Teacher009', 15, 1),
('Student010', 27, 2),
('Student011', 27, 2),
('Teacher012', 19, 1),
('Teacher013', 17, 1),
('Teacher014', 22, 1),
('Student015', 29, 2),
('Teacher016', 17, 1),
('Teacher017', 17, 1),
('Teacher018', 17, 1),
('Teacher019', 15, 1),
('Teacher020', 16, 1),
('Teacher021', 17, 1),
('Student022', 25, 2),
('Teacher023', 18, 1),
('Student024', 26, 2),
('Teacher025', 16, 1),
('Teacher026', 20, 1),
('Student027', 25, 2),
('Teacher028', 17, 1),
('Student029', 29, 2),
('Student030', 28, 2);

