CREATE PROCEDURE [dbo].[SetBatchUserActivity]
    @ActiveIds VARCHAR(MAX),
    @InActiveIds VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION; 

        CREATE TABLE ##BatchUpdate ([Id] uniqueidentifier, [Active] bit);

        IF (@ActiveIds != '')
        BEGIN
            INSERT INTO ##BatchUpdate ([Id], [Active])
            SELECT CAST(value AS uniqueidentifier), 1 
            FROM STRING_SPLIT(@ActiveIds, ',');
        END
        
        IF (@InActiveIds != '')
        BEGIN
            INSERT INTO ##BatchUpdate ([Id], [Active])
            SELECT CAST(value AS uniqueidentifier), 0 
            FROM STRING_SPLIT(@InActiveIds, ',');
        END;

        UPDATE U
        SET [Active] = B.[Active]
        FROM [dbo].[Users] U
        JOIN ##BatchUpdate B ON U.[Id] = B.[Id];

        DROP TABLE ##BatchUpdate;

        COMMIT TRANSACTION; 
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT @ErrorMessage = ERROR_MESSAGE(),
               @ErrorSeverity = ERROR_SEVERITY(),
               @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END