Use master
Go

Use SkpOpgWeb
Go

Create TABLE Users(
Name varchar(50) NOT NULL,
Competence varchar(250),
Login varchar(20) primary key,
Salt varbinary(20) NOT NULL,
Hash varbinary(20) NOT NULL
)
Go

Create table Projects(
ID int primary key identity(1,1), 
Status varchar(20) NOT NULL,
Title varchar(250) NOT NULL, 
Description varchar(250), 
Log varchar(250), 
StartDate DateTime NOT NULL, 
EndDate DateTime,
ProjectLeader varchar(20) FOREIGN KEY REFERENCES Users(Login) NOT NULL
)
Go

--List of members that are working on every project.
Create table Teams(
ID int NOT NULL FOREIGN KEY REFERENCES Projects(ID),
Name varchar (20) NOT NULL
)
Go


Create table Roles(
Username varchar(20) NOT NULL FOREIGN KEY REFERENCES Users(Login),
Role varchar (20) NOT NULL
)
Go

--Register user
CREATE PROC CreateUser
@Name varchar(50),
@Competence varchar(250),
@Login varchar(20),
@Salt varbinary(20), 
@Hash varbinary(20)
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

--View user by full name
CREATE PROC ViewUserByFullName
@Name varchar(20)
AS 
	SELECT Name, Competence, Login From Users
	Where Name = @Name
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

--Delete User by Username
CREATE PROC DeleteUser
@Username varchar(20)
AS
	DELETE FROM Projects
	WHERE Projects.ID = @Username
Go

--Change password on User
CREATE PROC ChangePassword
@Login varchar(20),
@Salt varbinary(20),
@Hash varbinary(20)
AS
	Update Users
	SET
	Salt = @Salt,
	Hash = @Hash
	WHERE Login = @Login
	Go
Go

--Create new project
CREATE PROC CreateProject
@Status varchar(20),
@Title varchar(250), 
@Description varchar(250),
@Log varchar(250),
@StartDate dateTime,
@EndDate dateTime,
@ProjectLeader varchar(20)
AS
	Insert into Projects 
	VALUES(@Status, @Title, @Description, @Log, @StartDate, @EndDate, @ProjectLeader)
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

--Update project by ID
CREATE PROC UpdateProject
@ID INT, 
@Status varchar(20),
@Title varchar(250), 
@Description varchar(250),
@Log varchar(250),
@StartDate dateTime,
@EndDate dateTime,
@ProjectLeader varchar(30)
AS
	Update Projects
	SET
	Status = @Status,
	Title = @Title,
	Description = @Description,
	Log = @Log,
	StartDate = @StartDate,
	EndDate = @EndDate,
	ProjectLeader = @ProjectLeader
	WHERE ID = @ID
	Go
	
Go

--Delete Project by ID
CREATE PROC DeleteProject
@ID int
AS
	DELETE FROM Projects
	WHERE ID = @ID
Go

--Add user to team.
Create proc AddUserToTeam(
@ID int,
@Name varchar(20))
AS
Insert into Teams values(@ID, @Name)
Go

--View members of team
CREATE PROC ViewTeamByID
@ID int
AS
Select Name from Teams WHERE ID = @ID
Go


SELECT * FROM Projects;

SELECT * FROM Users;

INSERT INTO Users(Name, Competence, Login, Salt, Hash) VALUES ('Kenneth Andersen', 'H2', 'kenn229k', 01230848129, 910293701972);

INSERT INTO Projects(Status, Title, Description, Log, StartDate, EndDate, ProjectLeader) VALUES ('Begyndt', 'SkpDb', 'Dette er en beskrivelse', 'Kenneth: hej med jer', GETDATE(), GETDATE(), 'kenn229k');