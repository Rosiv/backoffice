﻿CREATE SERVICE ProductChanged_Service_Sender 
ON QUEUE ProductChanged_Queue_Sender 
(ProductChanged_Contract) 