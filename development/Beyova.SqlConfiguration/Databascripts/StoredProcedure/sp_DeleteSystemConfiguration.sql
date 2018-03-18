CREATE PROCEDURE [dbo].[sp_DeleteSystemConfiguration](
    @Key NVARCHAR(64)
)
AS
SET NOCOUNT ON;
BEGIN
    DELETE FROM [dbo].[SystemConfiguration]
    WHERE [Key] = @Key;
END
GO


