import System.IO

folders = (
	"Db4objects.Db4o",
	"Db4objects.Db4o.Tests",
	"Db4objects.Db4o",
	"Db4oUnit",
	"Db4oUnit.Extensions",
)

for folder in folders:
	for sf in ("bin", "obj"):
		path = Path.Combine(folder, sf)
		if Directory.Exists(path):
			print "Deleting ${path}"
			Directory.Delete(path, true) 
