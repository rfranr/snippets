select object_name(object_id) as tablename, name as columnname,collation_name


-- by table_name
select count(*) , collation_name from sys.columns where collation_name is not null group by collation_name 

-- example:
-- (No column name)  collation_name
-- 6083  Modern_Spanish_CI_AS
-- 42  Modern_Spanish_CI_AI
-- 3 Latin1_General_BIN

-- 
select C.object_id, object_name(object_id) as tablename,  C.name, C.column_id, T.name  from sys.columns  C
inner join  sys.types T
on C.system_type_id = T.system_type_id
where C.collation_name = 'Modern_Spanish_CI_AI'

-- diferents objects where collation is different from the 'predomoinant'
select distinct object_name(object_id) as tablename from sys.columns  C
inner join  sys.types T
on C.system_type_id = T.system_type_id
where C.collation_name = 'Modern_Spanish_CI_AI'

-- example:
-- PERSON
-- PERSON_PASSWORD


-- then inspect objects
exec sp_help PERSON

select C.object_id, object_name(object_id) as tablename,  C.name, C.column_id, T.name  from sys.columns  C
inner join  sys.types T
on C.system_type_id = T.system_type_id
where C.collation_name = 'Latin1_General_BIN'
