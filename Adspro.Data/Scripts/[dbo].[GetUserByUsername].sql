CREATE PROCEDURE [dbo].[GetUserByUsername]
	@Username VARCHAR(64)
AS
	SELECT * FROM [dbo].[Users] WHERE [Username] = @Username;
