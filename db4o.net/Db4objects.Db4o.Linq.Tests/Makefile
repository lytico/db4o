
include ../include.mk

TARGET = exe
ASSEMBLY = $(LINQ_TESTS)
REFERENCES = -r:$(OUTDIR)/$(CORE) -r:$(OUTDIR)/$(UNIT) -r:$(OUTDIR)/$(UNIT_EXT) -r:$(OUTDIR)/$(LINQ) -r:$(OUTDIR)/$(CECIL) -r:$(OUTDIR)/$(FLOWANALYSIS) -r:$(OUTDIR)/$(CS)
OPT_MCS_FLAGS = -main:Db4objects.Db4o.Linq.Tests.AllTests
