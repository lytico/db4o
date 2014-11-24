
include ../../include.mk

OUTDIR = ../../bin

KEY_FILE = ../../db4objects.snk

precompile:
	[ -d $(OUTDIR) ] || mkdir $(OUTDIR)
	find $(WORKDIR) -name "*.cs" | egrep -v 'Resources'> $(RESPONSE_FILE)

TARGET = exe
ASSEMBLY = $(TOOL_TESTS)
REFERENCES = -r:$(OUTDIR)/$(CORE) -r:$(OUTDIR)/$(INSTR) -r:$(OUTDIR)/$(NQ) -r:$(OUTDIR)/$(CECIL) -r:$(OUTDIR)/$(FLOWANALYSIS) -r:$(OUTDIR)/$(OPTIONAL) -r:$(OUTDIR)/$(TOOL) -r:$(OUTDIR)/$(UNIT) -r:$(OUTDIR)/$(UNIT_EXT)
