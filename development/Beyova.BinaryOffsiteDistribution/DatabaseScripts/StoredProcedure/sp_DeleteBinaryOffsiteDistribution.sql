IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteBinaryOffsiteDistribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteBinaryOffsiteDistribution]
GO

CREATE PROCEDURE [dbo].[sp_DeleteBinaryOffsiteDistribution](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128)
)
AS
SET NOCOUNT ON;
BEGIN
   UPDATE [dbo].[BinaryOffsiteDistribution]
        SET 
            [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 3 --DeletePending
        WHERE [Identifier] = @Identifier
            AND (@Container IS NULL OR [Container] = @Container)
END
GO


