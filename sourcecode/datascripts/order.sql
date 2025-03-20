Create Table dOrder(
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

Create Table Carpet(
ID int not null identity 

Create Table OrderDetail



 