CREATE PROCEDURE [dbo].[GetUserActiveFlag]
	@UserId uniqueidentifier
AS
	SELECT [Active] FROM [dbo].[Users] WHERE [Id] = @UserId