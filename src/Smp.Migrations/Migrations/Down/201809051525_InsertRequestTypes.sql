IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RequestTypes')
BEGIN
	DELETE FROM RequestTypes 
END