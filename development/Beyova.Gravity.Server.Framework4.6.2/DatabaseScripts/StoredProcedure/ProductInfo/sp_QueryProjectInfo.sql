IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryProductInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryProductInfo];

GO
CREATE PROCEDURE [dbo].[sp_QueryProductInfo](
    @Key UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT [Key]
        ,[Name]
        ,[Token]
        ,[PublicKey]
        ,[PrivateKey]
        ,[ExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
    FROM [dbo].[ProductInfo]
    WHERE (@Key IS NULL OR [Key] = @Key)
        AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;
END

GO