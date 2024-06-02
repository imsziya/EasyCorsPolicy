# EasyCorsPolicy

Kindly add your policy in following format in Configuration such as appsettings/launchsetting etc with key as EasyCors:

"EasyCors": "{"Default":{"AllowedOrigins":"origin1,origin2,.....","AllowedHeaders":"header1,header2....","AllowedMethods":"method1,method2...","IsAllowedCredentials":false,"IsDefault":true}}"

replace your policies with dummydata like "origin1,origin2,....."  for origins/headers/method with comma (,) separated.
