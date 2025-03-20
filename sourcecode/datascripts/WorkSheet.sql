
Select C.id 'CarpetID', C.CarpetCode 'Carpet Code', C.Notes, CN.id 'CarpetDesignID', Cn.DesignCode 'Design Code', 
CC.id 'CarpetColorID', Cc.ColorCode 'Color Code'
 from 
Carpet C inner join CarpetDetail CD on C.id=CD.carpetID 
Inner join CarpetDesign CN on CD.CarpetDesignID = CN.id
Inner join CarpetColor CC on CD.CarpetColorid=CC.id
Order by C.id 


Select C.id 'CarpetID', C.CarpetCode 'Carpet Code', C.Notes, CN.id 'CarpetDesignID', Cn.DesignCode 'Design Code', 
CC.id 'CarpetColorID', Cc.ColorCode 'Color Code'
from 
Carpet C inner join CarpetDetail CD on C.id=CD.carpetID 
Inner join CarpetDesign CN on CD.CarpetDesignID = CN.id
Inner join CarpetColor CC on CD.CarpetColorid=CC.id
where C.id = 1
Order by C.id 

Select * from CarpetColor