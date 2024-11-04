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