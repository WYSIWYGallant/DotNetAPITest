IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Person')
BEGIN
    CREATE TABLE Person (
        PersonID INT PRIMARY KEY,         -- Unique identifier for each person
        PersonName NVARCHAR(100) NOT NULL,-- Name of the person (supports Unicode characters)
        PersonAge INT NOT NULL,           -- Age of the person
        PersonType INT NOT NULL           -- Type of person (e.g., 1=Student, 2=Teacher, etc.)
    );
END;
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PersonType')
BEGIN
	CREATE TABLE PersonType (
    PersonTypeID INT PRIMARY KEY,      -- Unique identifier for each type of person
    PersonTypeDescription NVARCHAR(100) NOT NULL,  -- Description of person type (supports Unicode characters)
);
END;
