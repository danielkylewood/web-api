USE WEBAPITEMPLATE;
GO

IF NOT EXISTS (SELECT * FROM dbo.ApiAuthentication WHERE ApiKey = '1907e999-5ee6-4daf-8076-208d4bfb19ac')
	INSERT INTO dbo.ApiAuthentication(CreatedDate, ApiKey, KeyHolder) VALUES(GETUTCDATE(), '1907e999-5ee6-4daf-8076-208d4bfb19ac', 'Test')