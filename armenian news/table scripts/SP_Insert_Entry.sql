

-- =============================================
-- Create basic stored procedure template
-- =============================================

-- Drop stored procedure if it already exists
IF EXISTS (
  SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES 
   WHERE SPECIFIC_NAME  = N'Insert_Entry'
)
begin
   DROP procedure Insert_Entry
   print ' DRP - Insert_Entry'
end
GO

CREATE procedure Insert_Entry 
(@title varchar(200), @pubDate varchar(100),@link varchar(300), @post Text)
	
AS

IF not EXISTS (
  SELECT * 
    FROM [dbo].[Entry]
   WHERE link = @link
)
begin
INSERT INTO [dbo].[Entry]
           ([Title]
           ,[PubDate]
           ,[Link]
           ,[Post]
           ,[FetchDate])
     VALUES
          (@title,@pubDate,@link,@post,GETDATE())
  
select SCOPE_IDENTITY() as ID        
return SCOPE_IDENTITY() 
end

go

if(@@ERROR <> 0)
	raiserror('Fail CRT Insert_Entry', 18, 127) with nowait
else
	print('SUCC CRT Insert_Entry')

go	


--exec dbo.Insert_Entry 'nice mockup3!','01/01/2009','link1','post'

