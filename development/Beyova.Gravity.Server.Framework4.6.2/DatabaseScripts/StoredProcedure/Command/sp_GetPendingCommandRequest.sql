IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetPendingCommandRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetPendingCommandRequest];

GO
CREATE PROCEDURE [dbo].[sp_GetPendingCommandRequest](
    @ClientKey [UNIQUEIDENTIFIER]
)
AS
BEGIN
    IF @ClientKey IS NOT NULL
    BEGIN
        SELECT [Key]
      ,[ProductKey]
      ,[Action]
      ,[Parameters]
      ,[ExpiredStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
      FROM 
	  -- [S] Starts
      (SELECT *, ROW_NUMBER() OVER(PARTITION BY [Action] ORDER BY [CreatedStamp] DESC) AS [RowNumber]
          FROM 
			-- [R] Starts
			(SELECT CRQ.[Key]
            ,CRQ.[ProductKey]
            ,CRQ.[Action]
            ,CRQ.[Parameters]
            ,CRQ.[ExpiredStamp]
            ,CRQ.[CreatedStamp]
            ,CRQ.[LastUpdatedStamp]
            ,CRQ.[State]
            ,CRR.[Key] AS [ResponseKey]
            FROM [dbo].[CommandRequest] AS CRQ
                LEFT JOIN [dbo].[CommandResult] AS CRR
                    ON CRR.[ClientKey] = @ClientKey
                    AND CRR.[RequestKey] = CRQ.[Key]
                WHERE (CRQ.[ExpiredStamp] IS NULL OR CRQ.[ExpiredStamp] > GETUTCDATE())) 
                     -- [R] Ends
                    AS R)
                -- [S] Ends
                AS S                
            WHERE [RowNumber] < 2;
    END
END

GO