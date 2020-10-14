IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Albums' AND  COLUMN_NAME = 'SortName')
ALTER TABLE Albums ADD [SortName] [nvarchar](100);	

IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Bands' AND  COLUMN_NAME = 'SortName')
ALTER TABLE Bands ADD [SortName] [nvarchar](100);	
GO