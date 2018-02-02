IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetProductInfoByToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetProductInfoByToken];

GO
CREATE PROCEDURE [dbo].[sp_GetProductInfoByToken](
    @Token [NVARCHAR](512)
)
AS
BEGIN
    SELECT TOP 1 [Key]
        ,[Name]
        ,[Token]
        ,[PublicKey]
        ,[PrivateKey]
        ,[ExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
    FROM [dbo].[ProductInfo]
    WHERE [Token] = @Token
        AND ([ExpiredStamp] IS NULL OR [ExpiredStamp] > GETUTCDATE())
        AND [dbo].[fn_ObjectIsWorkable]([State]) = 1
END

GO