
MONO = mono
MCS = gmcs
KEY_FILE = ../db4objects.snk
MCS_FLAGS = -debug+ -keyfile:$(KEY_FILE) -define:NET_2_0,NET_3_5,MONO,EMBEDDED
OUTDIR = ../bin
WORKDIR = .
RESPONSE_FILE = $(WORKDIR)/sources

CORE = Db4objects.Db4o.dll
CS = Db4objects.Db4o.CS.dll
OPTIONAL = Db4objects.Db4o.Optional.dll
CSOPTIONAL = Db4objects.Db4o.CS.Optional.dll
TESTS = Db4objects.Db4o.Tests.exe
UNIT = Db4oUnit.dll
UNIT_EXT = Db4oUnit.Extensions.dll
INSTR = Db4objects.Db4o.Instrumentation.dll
NQ = Db4objects.Db4o.NativeQueries.dll
TOOL = Db4oTool.exe
TOOL_TESTS = Db4oTool.Tests.exe
LINQ = Db4objects.Db4o.Linq.dll
LINQ_TESTS = Db4objects.Db4o.Linq.Tests.exe
LINQ_INSTR_TESTS = Db4objects.Db4o.Linq.Instrumentation.Tests.exe

CECIL = Mono.Cecil.dll
FLOWANALYSIS = Cecil.FlowAnalysis.dll
GETOPTIONS = Mono.GetOptions.dll
REFLECTION = Mono.Reflection.dll

build: precompile compile postcompile

precompile:
	[ -d $(OUTDIR) ] || mkdir $(OUTDIR)
	find $(WORKDIR) -name "*.cs" > $(RESPONSE_FILE)

compile:
	$(MCS) -t:$(TARGET) $(REFERENCES) -warn:0 -out:$(OUTDIR)/$(ASSEMBLY) $(MCS_FLAGS) $(OPT_MCS_FLAGS) @$(RESPONSE_FILE)

postcompile:
	rm -f $(RESPONSE_FILE)
