

Create table Customer(
ID int identity(1, 1) not null,
title char(5) not null default 'Mr.',
firstname varchar(25) not null, 
midname varchar(25) null, 
lastname varchar(25) null, 
address varchar(100) null, 
phone varchar(25) null, 
postalcode char(10) not null,
cell varchar(25) null, 
constraint PK_Customer primary key(ID)
)




 
 