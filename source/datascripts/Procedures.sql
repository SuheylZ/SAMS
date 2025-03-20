cREATE procedure uspGetOrder(@orderid int)
AS
Begin
	Select C.title+' '+C.firstname+' '+C.lastname 'customername', 
	C.Address, C.postalcode, C.phone, C.cell, 
	PO.purchasedate,PO.total, Po.deposit, Po.balance 
	From Customer C inner join PurchaseOrder PO On Po.Customerid=C.id
	where PO.id=@orderid
End

alter procedure uspGetOrderDetail(@orderid int)
AS
Begin
	Select C.CarpetCode, CD.DesignCode, CC.ColorCode, POD.Quantity, 
	POD.NOTEs,CAST(POD.Length as varchar(10)) + ' X ' + cast(POD.breadth as varchar(10)) 'Size'
	From 
	PurchaseOrderDetail POD inner join CarpetDetail CDet ON POD.CarpetDetailid=CDet.id 
	inner join Carpet C on Cdet.CarpetID=C.id
	inner join CarpetDesign CD on CDet.CarpetDesignid=cd.id
	inner join CarpetColor CC on CDet.CarpetColorid=CC.id
	where POD.PurchaseOrderid=@orderid
End