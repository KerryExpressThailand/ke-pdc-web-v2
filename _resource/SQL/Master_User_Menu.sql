select mu.userid,
	mu.page_id,
	mp.page_name
from tb_master_user_menu as mu with (nolock)
	inner join tb_master_menu_page as mp with (nolock)
on mu.page_id = mp.page_id
	--where mu.userid = 'bphi'
	and mu.page_id > 100