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

CREATE TABLE tbl_User (
	Id INT NOT NULL IDENTITY(0, 1),
	Username NVARCHAR(50) NOT NULL,
	[Password] CHAR(64) NOT NULL,
	[Admin] BIT NOT NULL CONSTRAINT DF_Admin DEFAULT 0,
	CONSTRAINT PK_User PRIMARY KEY (Id)
)
CREATE TABLE tbl_Exercise (
	Id INT NOT NULL IDENTITY(0, 1),
	[Name] NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(MAX),
	UserId INT NOT NULL,
	CONSTRAINT PK_Exercise PRIMARY KEY (Id),
	CONSTRAINT FK_Exercise_User FOREIGN KEY (UserId) REFERENCES tbl_User (Id)
)
CREATE TABLE tbl_Workout (
	Id INT NOT NULL IDENTITY(0, 1),
	[Name] NVARCHAR(50),
	UserId INT NOT NULL,
	CONSTRAINT PK_Workout PRIMARY KEY (Id),
	CONSTRAINT FK_Workout_User FOREIGN KEY (UserId) REFERENCES tbl_User (Id)
)
CREATE TABLE tbl_Post (
	Id INT NOT NULL IDENTITY(0, 1),
	Title NVARCHAR(50) NOT NULL,
	[Message] NVARCHAR(MAX),
	[Date] DATE NOT NULL CONSTRAINT DF_Date DEFAULT SYSDATETIME(),
	UserId INT NOT NULL,
	CONSTRAINT PK_Post PRIMARY KEY (Id),
	CONSTRAINT FK_Post_User FOREIGN KEY (UserId) REFERENCES tbl_User (Id)
)

-- HILFSTABELLEN

CREATE TABLE tbl_Exercise_Muscle (
	Id INT NOT NULL IDENTITY(0, 1),
	ExerciseId INT NOT NULL,
	MuscleId INT NOT NULL,
	CONSTRAINT PK_Exercise_Muscle PRIMARY KEY (Id),
	CONSTRAINT FK_Exercise_Muscle FOREIGN KEY (ExerciseId) REFERENCES tbl_Exercise (Id),
	CONSTRAINT FK_Muscle_Exercise FOREIGN KEY (MuscleId) REFERENCES tbl_Muscle (Id)
)

CREATE TABLE tbl_Workout_Exercise (
	Id INT NOT NULL IDENTITY(0, 1),
	WorkoutId INT NOT NULL,
	ExerciseId INT NOT NULL,
	CONSTRAINT PK_Workout_Exercise PRIMARY KEY (Id),
	CONSTRAINT FK_Workout_Exercise FOREIGN KEY (WorkoutId) REFERENCES tbl_Workout (Id) ON DELETE CASCADE,
	CONSTRAINT FK_Exercise_Workout FOREIGN KEY (ExerciseId) REFERENCES tbl_Exercise (Id) ON DELETE CASCADE
)

CREATE TABLE tbl_Post_Workout (
	Id INT NOT NULL IDENTITY(0, 1),
	PostId INT NOT NULL,
	WorkoutId INT NOT NULL,
	CONSTRAINT PK_Post_Workout PRIMARY KEY (Id),
	CONSTRAINT FK_Post_Workout FOREIGN KEY (PostId) REFERENCES tbl_Post (Id),
	CONSTRAINT FK_Wourkout_Post FOREIGN KEY (WorkoutId) REFERENCES tbl_Workout (Id) ON DELETE CASCADE
)


-- INDEX

GO
CREATE INDEX ix_Muscle
ON tbl_Muscle ([Name])
GO


INSERT INTO tbl_Muscle VALUES('Back');
INSERT INTO tbl_Muscle VALUES('Chest');
INSERT INTO tbl_Muscle VALUES('Arms');
INSERT INTO tbl_Muscle VALUES('Shoulders');
INSERT INTO tbl_Muscle VALUES('Legs');
INSERT INTO tbl_Muscle VALUES('Core');
INSERT INTO tbl_Muscle VALUES('Full Body');
INSERT INTO tbl_User (Username, [Password], [Admin]) VALUES ('Joe', '3639EFCD08ABB273B1619E82E78C29A7DF02C1051B1820E99FC395DCAA3326B8', 1);
INSERT INTO tbl_Exercise ([Name], [Description], UserId) VALUES ('Bench Press', 'fsadf0', 0);
INSERT INTO tbl_Exercise ([Name], [Description], UserId) VALUES ('Pull Up', 'sfj', 0);
INSERT INTO tbl_Exercise_Muscle (ExerciseId, MuscleId) VALUES(0, 0);
INSERT INTO tbl_Exercise_Muscle (ExerciseId, MuscleId) VALUES(0, 1);
INSERT INTO tbl_Exercise_Muscle (ExerciseId, MuscleId) VALUES(1, 1);
INSERT INTO tbl_Workout VALUES ('Upper Body', 0);
INSERT INTO tbl_Workout VALUES ('test', 0);
INSERT INTO tbl_Workout_Exercise VALUES (0, 0);
INSERT INTO tbl_Post (Title, [Message], UserId) VALUES('Crazy Workout', 'Wow this was so much fun', 0);
INSERT INTO tbl_Post (Title, [Message], UserId) VALUES('Another Crazy Workout', 'Again so much fun', 0);
INSERT INTO tbl_Post_Workout VALUES (0, 0);
INSERT INTO tbl_Post_Workout VALUES (1, 0);