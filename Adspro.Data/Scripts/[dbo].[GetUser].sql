CREATE PROCEDURE [dbo].[GetUser]
	@UserId uniqueidentifier
AS
	SELECT [Id], [Username], [Password], [Active] FROM [dbo].[Users] WHERE [Id] = @UserId;