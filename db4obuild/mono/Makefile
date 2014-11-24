#
# Makefile for db4o Mono RPM
#
DB4O_VERSION=@db4oversion@
DIR_HOME=.
DIR_BIN=$(DIR_HOME)/bin
DIR_SRC=$(DIR_HOME)/src
DIR_DOC=$(DIR_HOME)/doc
ifdef RPM_BUILD_ROOT
	DIR_INSTALL_BIN=$(RPM_BUILD_ROOT)/usr/lib/db4o
else
	DIR_INSTALL_BIN=/usr/lib/db4o
endif
ifdef RPM_DOC_DIR
	DIR_INSTALL_DOC=$(RPM_BUILD_ROOT)/$(RPM_DOC_DIR)/$(RPM_PACKAGE_NAME)-$(RPM_PACKAGE_VERSION)
else
	DIR_INSTALL_DOC=/usr/share/doc/db4o-$(DB4O_VERSION)
endif

MAIN_TEST=com.db4o.test.AllTests

MCS_OPTS=-warn:0 -debug+ -define:MONO

MCS=mcs

buildall:
	make -C $(DIR_SRC)
	mv $(DIR_SRC)/bin  $(DIR_BIN)
	cp $(DIR_HOME)/licenses/* $(DIR_BIN)

clean:
	rm -rf $(DIR_BIN)
	
install:
	mkdir -p $(DIR_INSTALL_BIN)
	cp -R $(DIR_BIN)/* $(DIR_INSTALL_BIN)
ifndef RPM_DOC_DIR
	mkdir -p $(DIR_INSTALL_DOC)
	cp -R $(DIR_DOC)/* $(DIR_INSTALL_DOC)
endif

.PHONY: clean buildall