/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [member_type]
    ,[member_name]
    ,[member_no]
    ,[branch_id]
    ,[identity_card_no]
    ,[tax_id]
    ,[bank_account_no]
    ,[bank_branch]
    ,[bank_name]
    ,[mobile1]
    ,[email]
    ,[shop_verified_at]
    ,[shop_verified_by]
    ,[shop_verified_staff_by]
FROM [POS].[dbo].[tb_pdc_cod_account]
--WHERE branch_id IN ('SNAR',
--'SPON',
--'SMSK',
--'SPRS',
--'SMSN',
--'SUBP',
--'SMPL',
--'SPLY',
--'SSAP',
--'SARR',
--'SPRA',
--'SMCT',
--'SLSG',
--'SMRY',
--'SMTN',
--'SMSI',
--'SMLP',
--'SDOT',
--'SMPO',
--'SMPK',
--'SMMQ',
--'SNOH',
--'SMCP',
--'SMCI',
--'SLAP',
--'SMMS',
--'SPRN',
--'SSAN',
--'SPAI',
--'SMSE')
WHERE ISNULL(is_shop_verified, 0) = 1
	AND ISNULL(is_account_verified, 0) = 0
	AND member_type = 'P'
	AND shop_verified_at < '2017-08-16 10:20'
ORDER BY shop_verified_at ASC