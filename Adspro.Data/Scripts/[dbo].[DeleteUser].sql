CREATE PROCEDURE [dbo].[DeleteUser]
	@UserId uniqueidentifier
AS
BEGIN
	DELETE FROM [dbo].[Users] WHERE [Id] = @UserId
END