
use Zypher
GO

Drop table PurchaseOrderDetail
Drop table Purchaseorder
Drop Table CarpetDetail
drop table Carpet
drop table CarpetDesign
drop table CarpetColor
Drop table Customer
GO

/*
	Customer table
*/
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
GO


Create Table CarpetDesign(
ID int not null identity(1, 1), 
DesignCode varchar(10) not null default '', 
constraint PK_CarpetDesign Primary key(ID)
)  

create table CarpetColor(
ID int not null identity(1, 1), 
ColorCode varchar(10) not null default '', 
constraint PK_CarpetColor primary key(ID)
)

Create Table Carpet(
ID int not null identity(1, 1),
Notes varchar(256) null
Constraint PK_Carpet Primary key(ID)
)
go

create table CarpetDetail(
ID int not null identity(1,1), 
CarpetID int not null,
CarpetDesignID int not null, 
CarpetColorID int not null, 
constraint PK_CarpetDetail primary key(ID), 
constraint FK_Carpet foreign key(CarpetID) references Carpet(ID), 
constraint FK_Design foreign key(CarpetDesignID) references CarpetDesign(ID),
constraint FK_Color foreign key(CarpetColorID) references CarpetColor(ID)
)
GO

/*
	PurchaseOrder table
*/

Create Table PurchaseOrder(
ID int not null identity(1,1) unique, 
CustomerID int Not null, 
PurchaseDate datetime not null default GetDate(),
OrderState smallint not null default 1,
PaymentMethod smallint null default 1,
Total money default 0.0,
balance money default 0.0, 
deposit money default 0.0,
Constraint PK_Order Primary key(ID), 
Constraint FK_Customer foreign key(CustomerID) references Customer(ID)
)
GO

create table PurchaseOrderDetail(
ID int not null identity(1,1), 
PurchaseOrderID int not null, 
CarpetDetailID int not null, 
constraint PK_PurchaseOrderDetail primary key(ID), 
constraint FK_Purchaseorder foreign key(PurchaseOrderID) references PurchaseOrder(ID), 
constraint FK_CarpetDetail foreign key (CarpetDetailID) references CarpetDetail(ID)
)

GO