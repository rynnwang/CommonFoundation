IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitBinaryOffsiteDistribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CommitBinaryOffsiteDistribution]
GO

CREATE PROCEDURE [dbo].[sp_CommitBinaryOffsiteDistribution](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128),
    @HostRegion [NVARCHAR](128)
)
AS
SET NOCOUNT ON;
BEGIN
    UPDATE [dbo].[BinaryOffsiteDistribution]
        SET [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 2
        WHERE [Identifier] = @Identifier
                AND [Container] = @Container
                AND [State] = 1
                AND [HostRegion] = @HostRegion;
END
GO


