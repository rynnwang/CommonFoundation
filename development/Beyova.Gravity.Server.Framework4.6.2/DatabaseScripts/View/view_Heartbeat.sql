IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_Heartbeat]') AND type in (N'V'))
DROP VIEW [dbo].[view_Heartbeat]
GO

CREATE VIEW [dbo].[view_Heartbeat]
AS

    SELECT 
       H.[Key]
      ,PC.[HostName]
      ,PC.[ServerName]
      ,PC.[IpAddress]
      ,PC.[ProductKey]
      ,H.[ClientKey]
      ,H.[ConfigurationName]
      ,H.[CpuUsage]
      ,H.[MemoryUsage]
      ,H.[TotalMemory]
      ,H.[CreatedStamp]
    FROM [dbo].[Heartbeat] AS H
    JOIN [dbo].[ProductClient] AS PC
        ON PC.[Key] = H.[ClientKey];

GO



