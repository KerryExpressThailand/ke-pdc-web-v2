/****** Script for SelectTopNRows command from SSMS  ******/
SELECT	 up.profile_id AS 'Profile Id'
		,up.profile_name AS 'Profile Name'
		,up.profile_owner AS 'Profile Owner'
		,u.userid AS Username
		,u.pwd AS [Password]
		,CONVERT(VARCHAR, u.created_date, 120) AS 'Created At'
		,u.created_by AS 'Created By'
		,ISNULL(u.default_shop_id, '-') AS 'Default Shop Id'
		,ISNULL(u.username, '-') AS 'Name'
		,ISNULL(u.role_id, 0) AS 'Role Id'
		,ISNULL(u.user_group_id, 0) AS 'User Group Id'
		,ISNULL(u.profile_id, '-') AS 'Profile Id'
FROM tb_master_user AS u
INNER JOIN tb_master_user_profile AS up ON u.profile_id = up.profile_id
WHERE u.profile_id LIKE 'FC%'
