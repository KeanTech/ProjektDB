USE [SkpOpgWeb]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 19-02-2021 00:31:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Delete User by Username
ALTER PROC [dbo].[DeleteUser]
@Username varchar(20)
AS
	DELETE FROM Projects
	WHERE Projects.ProjectLeader = @Username

	DELETE FROM Users
	WHERE Users.Login = @Username


	ALTER TABLE Users
	DROP COLUMN Salt;

	ALTER TABLE Users
	DROP COLUMN Hash;

	ALTER TABLE Users
	ADD Salt VARCHAR(MAX)

	ALTER TABLE Users
	ADD Hash VARCHAR(MAX)

	SELECT * FROM Teams;



