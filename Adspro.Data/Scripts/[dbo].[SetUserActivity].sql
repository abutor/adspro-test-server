CREATE PROCEDURE [dbo].[SetUserActivity]
	@UserId uniqueidentifier,
	@Active bit
AS
BEGIN
	UPDATE [dbo].[Users]
	SET [Active] = @Active
	WHERE [Id] = @UserId
END