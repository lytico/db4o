
include ../include.mk

TARGET = exe
ASSEMBLY = $(TESTS)
REFERENCES = -r:$(OUTDIR)/$(CORE) -r:$(OUTDIR)/$(CS) -r:$(OUTDIR)/$(OPTIONAL) -r:$(OUTDIR)/$(CSOPTIONAL) -r:$(OUTDIR)/$(UNIT) -r:$(OUTDIR)/$(UNIT_EXT) -r:$(OUTDIR)/$(INSTR) -r:$(OUTDIR)/$(NQ) -r:$(OUTDIR)/$(CECIL) -r:$(OUTDIR)/$(FLOWANALYSIS) -r:$(OUTDIR)/$(LINQ) -r:$(OUTDIR)/$(LINQ_TESTS) -r:System.Drawing 
OPT_MCS_FLAGS = -main:Db4objects.Db4o.Tests.AllTests
