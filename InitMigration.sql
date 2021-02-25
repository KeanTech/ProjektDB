Use SkpOpgWeb
Go

------------------Create Tables
Create TABLE Users(
Name varchar(50) NOT NULL,
Competence varchar(250),
Login varchar(20) primary key,
Salt VARCHAR(MAX) NOT NULL,
Hash VARCHAR(MAX) NOT NULL
)
Go

Create table Roles(
Username varchar(20) NOT NULL FOREIGN KEY REFERENCES Users(Login),
Role varchar (20) NOT NULL
)
Go

Create table Projects(
ID int primary key identity(1,1), 
Status varchar(20) NOT NULL,
Title varchar(250) NOT NULL, 
Description varchar(250),  
StartDate DateTime NOT NULL, 
EndDate DateTime NOT NULL,
ProjectLeader varchar(20) FOREIGN KEY REFERENCES Users(Login) NOT NULL
)
Go

--Table of members that are working on every project.
Create table Teams(
ID int NOT NULL FOREIGN KEY REFERENCES Projects(ID),
Username varchar (20) NOT NULL FOREIGN KEY REFERENCES Users(Login)
) 
Go

--Table of logs that are assosiated with each project
Create Table Logs(
LogID int primary key identity(1,1),
ID int NOT NULL FOREIGN KEY REFERENCES Projects(ID),
Log varchar (250),
Username varchar(20) NOT NULL FOREIGN KEY REFERENCES Users(Login),
Date dateTime
)
Go
----------------

---------------------User Procs
--Register user
CREATE PROC CreateUser
@Name varchar(50),
@Competence varchar(250),
@Login varchar(20),
@Salt varchar(max), 
@Hash varchar(max)
AS
	Insert into Users 
	VALUES(@Name, @Competence, @Login, @Salt, @Hash)
Go

--View all Users in database
CREATE PROC ViewAllUsers
AS 
	SELECT Name, Competence, Login From Users
Go

--View User by username
CREATE PROC ViewUserByUsername
@Login varchar(20)
AS 
	SELECT Name, Competence, Login From Users
	Where Login = @Login
Go

--Delete User by Username
CREATE PROC DeleteUser
@Username varchar(20)
AS
	DELETE FROM Projects
	WHERE Projects.ID = @Username
Go

--Update User
CREATE PROC UpdateUser
@Username varchar(20),
@Hash varchar(max),
@Salt varchar(max),
@Name varchar(50),
@Competence varchar(20)
AS
	Update Users
	Set
	Hash = @Hash,
	Salt = @Salt,
	Name = @Name,
	Competence = @Competence
	WHERE Login = @Username
Go


--Change password on User
CREATE PROC ChangePassword
@Login varchar(20),
@Hash varbinary(20),
@Salt varbinary(20)
AS
	Update Users
	SET
	Salt = @Salt,
	Hash = @Hash
	WHERE Login = @Login
Go

--Get Salt from user
CREATE PROC GetSalt
@Username varchar(20)
AS
	Select Salt from Users
	Where Login = @Username
Go

--Get Hash from user
CREATE PROC GetHash
@Username varchar(20)
AS
	Select Hash from Users
	Where Login = @Username
Go

--Search for user
CREATE PROC SearchForUsers
@Search varchar(20)
AS
	SELECT Name, Competence, Login From Users
	Where 
	Login LIKE '%' + @Search + '%'
	OR
	Name LIKE '%' + @Search + '%'
Go
--------------------

-------------------Roles proc
--Add Role to User
Create Proc AddRoleToUser
@Username varchar(20),
@Role varchar(20)
AS
	Insert into Roles
	Values(@Username, @Role)
Go

--View all roles on User
Create proc ViewUsersRoles
@Username varchar(20)
As
Select Role from Roles
Where Username = @Username
Go

--Remove role from User
Create Proc RemoveRoleFromUser
@Username varchar(20),
@Role varchar(20)
As
Delete Roles
Where 
Username = @Username
AND
Role = @Role
Go

--Update role
Create proc UpdateRole
@Username varchar(20),
@OldRole varchar(20),
@NewRole varchar(20)
AS
	Update Roles
	Set 
	Role = @NewRole
	WHERE
	Username = @Username
	AND
	Role = @OldRole
Go
-------------------

--------------------Project procs
--Create new project
CREATE PROC CreateProject
@Status varchar(20),
@Title varchar(250), 
@Description varchar(250),
@StartDate dateTime,
@EndDate dateTime,
@ProjectLeader varchar(20)
AS
	Insert into Projects 
	VALUES(@Status, @Title, @Description, @StartDate, @EndDate, @ProjectLeader)

Go

--View all projects in database
CREATE PROC ViewAllProjects 
AS 
	SELECT * From Projects
Go

--View project by ID
CREATE PROC ViewProject
@ID int
AS 
	SELECT * FROM Projects 
	WHERE ID = @ID
Go

--View Active Projects
CREATE PROC ViewActiveProjects
AS
	Select * FROM Projects
	WHERE Status = 'Aktiv'
Go

--Update project by ID
CREATE PROC UpdateProject
@ID INT, 
@Status varchar(20),
@Title varchar(250), 
@Description varchar(250),
@StartDate dateTime,
@EndDate dateTime,
@ProjectLeader varchar(30)
AS
	Update Projects
	SET
	Status = @Status,
	Title = @Title,
	Description = @Description,
	StartDate = @StartDate,
	EndDate = @EndDate,
	ProjectLeader = @ProjectLeader
	WHERE ID = @ID
Go

--Delete Project by ID
CREATE PROC DeleteProject
@ID int
AS
	DELETE FROM Logs
	WHERE ID = @ID

	DELETE FROM Teams
	WHERE ID = @ID

	DELETE FROM Projects
	WHERE ID = @ID
Go
-----------------

-----------------Teams procs
--Add user to team.
Create proc AddUserToTeam(
@ID int,
@Username varchar(20))
AS

Insert into Teams
VALUES(@ID, @Username)
Go

Create Proc RemoveUserFromTeam(
@ID int,
@Username varchar(20))
AS
DELETE FROM Teams
Where 
ID = @ID 
AND
Username = @Username
Go

--View members of team
CREATE PROC GetTeam
@ID int
AS
Select Username from Teams WHERE ID = @ID
Go
------------------------

------------------------Log procs
--Add log
Create proc AddLogToProject(
@ID int, 
@Log varchar(250),
@Username varchar(20)
)
AS
Insert into Logs
VALUES(@ID, @Log, @Username, GETDATE())
GO

Create proc EditLog(
@LogID int,
@Log varchar(250),
@Username varchar(20)
)
AS
	Update Logs
	Set
	Log = @Log,
	Username = @Username,
	Date = GETDATE()
	WHERE LogID = @LogID
Go

Create proc DeleteLog(
@LogID int
)
AS
	Delete Logs 
	WHERE LogID = @LogID
Go

create proc ViewLastLogFromTeam(
@ID int
)
As
	SELECT TOP 1 Percent * 
    FROM Logs
	WHERE ID = @ID
	ORDER BY LogID Desc
Go

create proc ViewAllLogsFromTeam(
@ID int
)
AS
	Select *
	FROM Logs
	WHERE ID = @ID
	ORDER BY LogID Desc
Go

