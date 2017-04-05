select f.RequestId, f.UserId as UserId, f.Status as UserStatus, f2.UserId as FriendId, f2.Status as FriendStatus
from Friends f
join Friends f2 on f.RequestId = f2.RequestId 
	and f.UserId = '67aae632-f7e4-43c8-a662-1c3d3045fed8' 
	and f.Status != f2.Status

select f.RequestId, f.UserId as UserId, f.Status as UserStatus, f2.UserId as FriendId, f2.Status as FriendStatus
from Friends f
join Friends f2 on f.RequestId = f2.RequestId 
	and f.UserId = '01d3b4c2-aecc-45bb-80cd-dcd22a354260' 
	and f.Status != f2.Status

select *
from Friends f, FriendRequests fr
where f.RequestId = fr.RequestId
and fr.RequestId = 7