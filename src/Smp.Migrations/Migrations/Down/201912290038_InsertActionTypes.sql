IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ActionTypes')
BEGIN
	DELETE FROM ActionTypes 
END