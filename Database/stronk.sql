USE master
GO
IF EXISTS (SELECT * FROM sysdatabases WHERE name = 'Stronk')
BEGIN
    ALTER DATABASE Stronk SET single_user WITH ROLLBACK IMMEDIATE
    DROP DATABASE Stronk
END

CREATE DATABASE Stronk
GO
USE Stronk
GO

CREATE TABLE tbl_Muscle (
	Id INT NOT NULL IDENTITY(0, 1),
	[Name] NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_Muscle PRIMARY KEY (Id)
)

CREATE TABLE tbl_Exercise (
	Id INT NOT NULL IDENTITY(0, 1),
	[Name] NVARCHAR(50) NOT NULL,
	[Description] TEXT,
	CONSTRAINT PK_Exercise PRIMARY KEY (Id)
)

CREATE TABLE tbl_User (
	Id INT NOT NULL IDENTITY(0, 1),
	Username NVARCHAR(50) NOT NULL,
	[Password] VARBINARY(130) NOT NULL,
	[Admin] BIT NOT NULL CONSTRAINT DF_Admin DEFAULT 0,
	CONSTRAINT PK_User PRIMARY KEY (Id)
)
CREATE TABLE tbl_Workout (
	Id INT NOT NULL IDENTITY(0, 1),
	[Name] NVARCHAR(50),
	CONSTRAINT PK_Workout PRIMARY KEY (Id)
)
CREATE TABLE tbl_Post (
	Id INT NOT NULL IDENTITY(0, 1),
	Title NVARCHAR(50) NOT NULL,
	[Message] TEXT,
	[Date] DATE NOT NULL CONSTRAINT DF_Date DEFAULT GETDATE(),
	Id_User INT NOT NULL,
	CONSTRAINT PK_Post PRIMARY KEY (Id),
	CONSTRAINT FK_Post_User FOREIGN KEY (Id_User) REFERENCES tbl_User (Id)
)

-- HILFSTABELLEN

CREATE TABLE tbl_Exercise_Muscle (
	Id INT NOT NULL IDENTITY(0, 1),
	Id_Exercise INT NOT NULL,
	Id_Muscle INT NOT NULL,
	CONSTRAINT PK_Exercise_Muscle PRIMARY KEY (Id),
	CONSTRAINT FK_Exercise_Muscle FOREIGN KEY (Id_Exercise) REFERENCES tbl_Exercise (Id),
	CONSTRAINT FK_Muscle_Exercise FOREIGN KEY (Id_Muscle) REFERENCES tbl_Muscle (Id)
)

CREATE TABLE tbl_Workout_Exercise (
	Id INT NOT NULL IDENTITY(0, 1),
	Id_Workout INT NOT NULL,
	Id_Exercise INT NOT NULL,
	CONSTRAINT PK_Workout_Exercise PRIMARY KEY (Id),
	CONSTRAINT FK_Workout_Exercise FOREIGN KEY (Id_Workout) REFERENCES tbl_Workout (Id),
	CONSTRAINT FK_Exercise_Workout FOREIGN KEY (Id_Exercise) REFERENCES tbl_Exercise (Id)
)

-- PROCEDUREN
GO
CREATE PROCEDURE usp_Register @username NVARCHAR(50), @password NVARCHAR(90)
AS
BEGIN
DECLARE @used AS INT = 0;
SELECT @used = COUNT(Username) FROM tbl_User WHERE Username = @username
IF LEN(@username) > 5 OR LEN(@password) > 90 OR @used > 0
	RETURN

INSERT INTO tbl_User (Username, [Password]) VALUES (@username, HASHBYTES('SHA2_512', @password))
END
GO

CREATE PROCEDURE usp_Login @username NVARCHAR(50), @password NVARCHAR(90)
AS
BEGIN
SELECT COUNT(Id) FROM tbl_User WHERE Username = @username AND HASHBYTES('SHA2_512', @password) = [Password]
END
GO

-- INDEX

CREATE INDEX ix_Muscle
ON tbl_Muscle ([Name])


INSERT INTO tbl_Muscle VALUES('Back');
INSERT INTO tbl_Muscle VALUES('Chest');
INSERT INTO tbl_Muscle VALUES('Bicep');
INSERT INTO tbl_Muscle VALUES('Tricep');
INSERT INTO tbl_Muscle VALUES('Quads');
INSERT INTO tbl_Exercise ([Name]) VALUES ('Bench Press');
INSERT INTO tbl_Exercise ([Name]) VALUES ('Pull Up');
INSERT INTO tbl_Workout VALUES ('Upper Body');
INSERT INTO tbl_Workout_Exercise VALUES (0, 0);
INSERT INTO tbl_Exercise_Muscle VALUES(0, 1);
INSERT INTO tbl_Exercise_Muscle VALUES(1, 0);
EXEC usp_Register @username = 'Joe', @password = 'Hi';
INSERT INTO tbl_Post (Title, Id_User) VALUES('Crazy Workout', 0);
SELECT * FROM tbl_Post;




-- SELECT tbl_Exercise.Name AS 'Exercise', tbl_Muscle.Name AS 'Muscle' FROM tbl_Exercise INNER JOIN (tbl_Muscle INNER JOIN tbl_Exercise_Muscle ON tbl_Muscle.Id = tbl_Exercise_Muscle.Id_Muscle) ON tbl_Exercise.Id = tbl_Exercise_Muscle.Id_Exercise;
-- SELECT tbl_Workout.Name AS 'Workout', tbl_Exercise.Name AS 'Exercise' FROM tbl_Workout INNER JOIN (tbl_Exercise INNER JOIN tbl_Workout_Exercise ON tbl_Exercise.Id = tbl_Workout_Exercise.Id_Exercise) ON tbl_Exercise.Id = tbl_Workout_Exercise.Id_Workout;