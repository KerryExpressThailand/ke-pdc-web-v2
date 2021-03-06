select	 s.memberid
		,c.member_name
		,c.mobile1
		,CASE WHEN c.member_type = 'P' THEN c.identity_card_no ELSE c.tax_id END AS identity_card
		,c.bank_account_no
		,c.member_no
		,s.cod_account_id
		,count(*) as cons
from tb_Shipment s (nolock)
inner join tb_pdc_cod_account c (nolock) on s.memberid = c.member_id

where convert(varchar, s.Created_Date, 112) = '20170801' and isnull(s.cod_account_id, '') <> ''
and left(s.memberid, 1) = '1'

and isnull(c.is_shop_verified, 0) = 1
and isnull(c.is_account_verified, 0) = 0
and ISNULL(c.is_customer_confirmed, 0) = 1

group by s.memberid, c.member_name, c.mobile1, c.member_type, c.member_no, c.identity_card_no, c.tax_id, c.bank_account_no, s.cod_account_id