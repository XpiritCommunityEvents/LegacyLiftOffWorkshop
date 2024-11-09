
IF (SELECT COUNT(*) from dbo.[Login]) = 0
BEGIN
    insert dbo.[Login] values (101,	'Owner',	'Owner123',	'Owner')
    insert dbo.[Login] values (201,	'Manager',	'Manager123',	'Manager')
END

IF (SELECT COUNT(*) from dbo.FoodPackage) = 0
BEGIN
    insert dbo.FoodPackage values (0,	'', 	0)
    insert dbo.FoodPackage values (11,	'Rice+Chicken',	1000)
    insert dbo.FoodPackage values (12,	'Rice+Beef',	1300)
    insert dbo.FoodPackage values (13,	'Rice+Shrimp',	1100)
END

IF (SELECT COUNT(*) from dbo.ServicePackage) = 0
BEGIN
    insert dbo.ServicePackage values (0,	'', 	0)
    insert dbo.ServicePackage values (21,	'Spa',	1000)
    insert dbo.ServicePackage values (22,	'Cinema',	500)
    insert dbo.ServicePackage values (23,	'Spa+Cinema',	1400)
    insert dbo.ServicePackage values (24,	'Karaoke',	900)
    insert dbo.ServicePackage values (25,	'Karaoke+Cinema',	1100)
END

IF (SELECT COUNT(*) from dbo.Room) = 0
BEGIN
  insert dbo.Room values (1,	'Single',	'Yes',	2000)
  insert dbo.Room values (2,	'Double',	'No',	3000)
  insert dbo.Room values (3,	'Family',	'Yes',	5000)
  insert dbo.Room values (4,	'Single',	'Yes',	2000)
  insert dbo.Room values (5,	'Single',	'Yes',	2000)
  insert dbo.Room values (6,	'Double',	'Yes',	3000)
  insert dbo.Room values (7,	'Double',	'Yes',	3000)
  insert dbo.Room values (8,	'Family',	'Yes',	5000)
  insert dbo.Room values (9,	'Family',	'Yes',	5000)
  insert dbo.Room values (10,	'Single',	'Yes',	2000)
  insert dbo.Room values (11,	'Single',	'No',	2000)
  insert dbo.Room values (12,	'Double',	'No',	3000)
  insert dbo.Room values (13,	'Double',	'No',	3000)  
END

IF (SELECT COUNT(*) from dbo.Booking) = 0
BEGIN
    insert dbo.Booking values (1001,	1,	'2021-05-02',	'2021-05-05',	11,	21,	'Tanha'	, 'Ctg, BD',	'01777777777',	'12345678912345678',	8000,	8000,	0)
    insert dbo.Booking values (1002,	2,	'2021-05-03',	'2021-05-05',	11,	21,	'Golpo'	, 'Dhaka',	    '01388888888',	'45629464367854245',	9000,	9000,	0)
    insert dbo.Booking values (1003,	3,	'2021-05-03',	'2021-05-04',	0,	22,	'Sajib'	, 'Dhaka', 	    '01333333333',	'64786450912476549',	5500,	2500,	3000)
    insert dbo.Booking values (1004,	4,	'2021-05-03',	'2021-05-06',	12,	23,	'Reja'	, 'CTG',	    '01222222222',	'45367834509453167',	11000,	5000,	6000)
    insert dbo.Booking values (1005,	5,	'2021-05-03',	'2021-05-05',	0,	0,	'Tisma'	, 'CTG',	    '01555555555',	'54649870234563456',	4000,	2000,	2000)
    insert dbo.Booking values (1006,	6,	'2021-05-03',	'2021-05-05',	0,	0,	'Sazia'	, 'Dhaka',	    '01444444444',	'09456812350976534',	6000,	3000,	3000)
    insert dbo.Booking values (1007,	7,	'2021-05-03',	'2021-05-05',	0,	0,	'Barman', 'Dhaka',	    '01888888888',	'54546767898909867',	6000,	3000,	3000)
    insert dbo.Booking values (1008,	8,	'2021-05-03',	'2021-05-05',	11,	0,	'Tasmia', 'CTG',	    '01666666666',	'67676767676767676',	12000,	6000,	6000)
    insert dbo.Booking values (1009,	9,	'2021-05-03',	'2021-05-05',	0,	0,	'Rehena', 'CTG',	    '01777777778',	'43538634231234456',	10000,	5000,	5000)
    insert dbo.Booking values (1010,	1,	'2021-05-03',	'2021-05-06',	13,	21,	'Alia'	, 'Dhaka',	    '01874563087',	'13684567745387612',	10300,	6000,	4300)
    insert dbo.Booking values (1011,	1,	'2021-05-03',	'2021-05-07',	13,	24,	'Lily'	, 'Thakurgaon',	'01987867565',	'34247678985430987',	13300,	7000,	6300)
END
GO