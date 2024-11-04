CREATE TABLE [dbo].[Users]
(
	[Id] uniqueidentifier primary key,
	[Username] varchar(64) not null unique,
	[Password] varchar(64) not null,
	[Active] bit default(1)
);

GO;
CREATE PROCEDURE [dbo].[GetUserByUsername]
	@Username VARCHAR(64)
AS
	SELECT * FROM [dbo].[Users] WHERE [Username] = @Username;

CREATE PROCEDURE [dbo].[GetUser]
	@UserId uniqueidentifier
AS
	SELECT [Id], [Username], [Password], [Active] FROM [dbo].[Users];
GO;

CREATE PROCEDURE [dbo].[GetUsers]
AS
	SELECT [Id] FROM [dbo].[Users]
GO;

CREATE PROCEDURE [dbo].[SetUserActivity]
	@UserId uniqueidentifier,
	@Active bit
AS
BEGIN
	UPDATE [dbo].[Users]
	SET [Active] = @Active
	WHERE [Id] = @UserId
END
GO;

CREATE PROCEDURE [dbo].[SetBatchUserActivity]
	@ActiveIds VARCHAR(MAX),
	@InActiveIds VARCHAR(MAX)
AS
BEGIN
	BEGIN TRANSACTION batch_update
	
	CREATE TABLE ##BatchUpdate ([Id] uniqueidentifier, [Active] bit) 

	INSERT INTO ##BatchUpdate ([Id], [Active])
	SELECT CAST(value AS uniqueidentifier), 1 FROM STRING_SPLIT(@ActiveIds, ',')
	UNION SELECT CAST(value AS uniqueidentifier), 0 FROM STRING_SPLIT(@InActiveIds, ',');

	UPDATE U
	SET [Active] = B.[Active]
	FROM [dbo].[Users] U
	JOIN ##BatchUpdate B ON U.[Id] = B.[Id]

	DROP TABLE ##BatchUpdate
	
	COMMIT TRANSACTION batch_update 
END
GO;

CREATE PROCEDURE [dbo].[CreateUser]
	@Id uniqueidentifier,
	@Username varchar(64),
	@Password varchar(64),
	@Active bit
AS
BEGIN
	INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [Active])
	VALUES (@Id, @Username, @Password, @Active);
	RETURN 1
END
GO;

CREATE PROCEDURE [dbo].[DeleteUser]
	@UserId uniqueidentifier
AS
BEGIN
	DELETE FROM [dbo].[Users] WHERE [Id] = @UserId
END
GO;
