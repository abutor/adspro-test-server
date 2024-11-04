CREATE PROCEDURE [dbo].[GetUsers]
	@Page INT = 1,
	@Limit INT = 25
AS
	SELECT [Id], [Username], [Password], [Active] FROM [dbo].[Users] ORDER BY [Username] OFFSET (@Page - 1) * @Limit ROWS FETCH NEXT @Limit ROWS ONLY;