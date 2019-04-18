CREATE PROCEDURE [dbo].[sp_GetPendingDeleteBinaryStorages]
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 50 [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[KVMeta]
          ,NULL AS [OwnerKey]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedBy]
          ,[State]
      FROM [dbo].[BinaryStorageMetaData]
      WHERE [State] = 3; --DeletePending
END
GO


