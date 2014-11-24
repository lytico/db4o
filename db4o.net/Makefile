
MAKE = make
CORE = Db4objects.Db4o
CS = Db4objects.Db4o.CS
OPTIONAL = Db4objects.Db4o.Optional
CSOPTIONAL = Db4objects.Db4o.CS.Optional
TESTS = Db4objects.Db4o.Tests
UNIT = Db4oUnit
UNIT_EXT = Db4oUnit.Extensions
INSTR = Db4objects.Db4o.Instrumentation
NQ = Db4objects.Db4o.NativeQueries
TOOL = Db4oTool/Db4oTool
TOOL_TESTS = Db4oTool/Db4oTool.Tests
LINQ = Db4objects.Db4o.Linq
LINQ_TESTS = Db4objects.Db4o.Linq.Tests
LINQ_INSTR_TESTS = Db4objects.Db4o.Linq.Instrumentation.Tests

LIBS = Libs/net-3.5

OUTDIR = ./bin

all: prebuild build postbuild

prebuild:
	[ -d $(OUTDIR) ] || mkdir $(OUTDIR)
	cp $(LIBS)/*.dll $(OUTDIR)

build: core cs optional csoptional nq linq tool tests

postbuild:

tests: unit_ext tool_tests linq_instr_tests
	cd $(TESTS) ; $(MAKE)

linq_tests:
	cd $(LINQ_TESTS) ; $(MAKE)

linq_instr_tests: linq_tests instr
	cd $(LINQ_INSTR_TESTS) ; $(MAKE)

tool_tests:
	cd $(TOOL_TESTS) ; $(MAKE)

tool:
	cd $(TOOL) ; $(MAKE)

unit_ext: unit
	cd $(UNIT_EXT) ; $(MAKE)

unit:
	cd $(UNIT) ; $(MAKE)

instr:
	cd $(INSTR) ; $(MAKE)

nq: instr
	cd $(NQ) ; $(MAKE)

linq:
	cd $(LINQ) ; $(MAKE)

core:
	cd $(CORE) ; $(MAKE)

cs:
	cd $(CS) ; $(MAKE)

csoptional: cs
	cd $(CSOPTIONAL) ; $(MAKE)

optional: linq
	cd $(OPTIONAL) ; $(MAKE)

clean:
	rm -rf $(OUTDIR)
