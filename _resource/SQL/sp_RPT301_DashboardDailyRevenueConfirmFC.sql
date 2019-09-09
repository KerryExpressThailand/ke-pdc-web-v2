/*sp_RPT301_DashboardDailyRevenueConfirmFC*/
DECLARE @Month int = 4
DECLARE @Year int = 2017


SELECT	 db.profile_id AS FCID
		,db.userid AS Username
		,db.profile_name AS [Name]
		,db.BranchID
		,SUM(db.[1]) AS '1'
		,SUM(db.[2]) AS '2'
		,SUM(db.[3]) AS '3'
		,SUM(db.[4]) AS '4'
		,SUM(db.[5]) AS '5'
		,SUM(db.[6]) AS '6'
		,SUM(db.[7]) AS '7'
		,SUM(db.[8]) AS '8'
		,SUM(db.[9]) AS '9'
		,SUM(db.[10]) AS '10'
		,SUM(db.[11]) AS '11'
		,SUM(db.[12]) AS '12'
		,SUM(db.[13]) AS '13'
		,SUM(db.[14]) AS '14'
		,SUM(db.[15]) AS '15'
		,SUM(db.[16]) AS '16'
		,SUM(db.[17]) AS '17'
		,SUM(db.[18]) AS '18'
		,SUM(db.[19]) AS '19'
		,SUM(db.[20]) AS '20'
		,SUM(db.[21]) AS '21'
		,SUM(db.[22]) AS '22'
		,SUM(db.[23]) AS '23'
		,SUM(db.[24]) AS '24'
		,SUM(db.[25]) AS '25'
		,SUM(db.[26]) AS '26'
		,SUM(db.[27]) AS '27'
		,SUM(db.[28]) AS '28'
		,SUM(db.[29]) AS '29'
		,SUM(db.[30]) AS '30'
		,SUM(db.[31]) AS '31'
FROM (
	SELECT	 d.profile_id
			,d.userid
			,d.profile_name
			,d.BranchID
			,CASE WHEN [Day] = 1 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '1'
			,CASE WHEN [Day] = 2 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '2'
			,CASE WHEN [Day] = 3 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '3'
			,CASE WHEN [Day] = 4 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '4'
			,CASE WHEN [Day] = 5 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '5'
			,CASE WHEN [Day] = 6 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '6'
			,CASE WHEN [Day] = 7 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '7'
			,CASE WHEN [Day] = 8 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '8'
			,CASE WHEN [Day] = 9 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '9'
			,CASE WHEN [Day] = 10 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '10'
			,CASE WHEN [Day] = 11 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '11'
			,CASE WHEN [Day] = 12 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '12'
			,CASE WHEN [Day] = 13 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '13'
			,CASE WHEN [Day] = 14 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '14'
			,CASE WHEN [Day] = 15 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '15'
			,CASE WHEN [Day] = 16 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '16'
			,CASE WHEN [Day] = 17 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '17'
			,CASE WHEN [Day] = 18 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '18'
			,CASE WHEN [Day] = 19 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '19'
			,CASE WHEN [Day] = 20 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '20'
			,CASE WHEN [Day] = 21 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '21'
			,CASE WHEN [Day] = 22 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '22'
			,CASE WHEN [Day] = 23 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '23'
			,CASE WHEN [Day] = 24 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '24'
			,CASE WHEN [Day] = 25 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '25'
			,CASE WHEN [Day] = 26 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '26'
			,CASE WHEN [Day] = 27 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '27'
			,CASE WHEN [Day] = 28 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '28'
			,CASE WHEN [Day] = 29 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '29'
			,CASE WHEN [Day] = 30 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '30'
			,CASE WHEN [Day] = 31 THEN
				CASE WHEN d.Captured IS NULL THEN 2 ELSE d.FCConfirmed END
			ELSE 0 END AS '31'
	FROM (
		SELECT	 us.userid
				,up.profile_id
				,up.profile_name
				,dr.BranchID
				,DAY(dr.ReportDate) AS [Day]
				,dr.Captured
				,ISNULL(dr.FCConfirmed, 0) AS FCConfirmed
				--,dr.FCConfirmedDate
				--,dr.FCConfirmedBy
		FROM tb_master_user_shop AS us
		LEFT JOIN tb_master_user AS u ON us.userid = u.userid
		INNER JOIN tb_master_user_profile AS up ON up.profile_id = u.profile_id
		INNER JOIN tb_daily_revenue AS dr ON dr.BranchID = us.shop_id
		WHERE YEAR(dr.ReportDate) = @Year AND MONTH(dr.ReportDate) = @Month
	) AS d
) AS db
GROUP BY db.BranchID, db.profile_id, db.userid, db.profile_name
ORDER BY db.profile_id