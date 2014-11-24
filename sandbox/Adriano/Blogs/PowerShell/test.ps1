$obj = "" | select Name, Address
$obj.Name = "Adriano"
$obj.Address = "Londrina, PR"

get-pstest -DatabasePath test.odb -item $obj