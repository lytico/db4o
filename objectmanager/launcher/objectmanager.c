/*******************************************************************************
 * Copyright (c) 2005, db4objects, Inc. and others.
 *
 * All rights reserved. This program and the accompanying materials 
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/cpl-v10.html
 * 
 * Contributors:
 *     db4objects       - Object Manager launcher
 *     IBM Corporation  - findCommand function (from Eclipse's launcher)
 *******************************************************************************/
#include <stdio.h>
#include <sys/types.h>
#include <sys/stat.h>

#define MAX_PATH_LENGTH   2000

#ifdef _WIN32

#ifdef UNICODE
#define _UNICODE
#endif
#include <windows.h>

#include <TCHAR.h>
#define dirSeparator '\\'
#define pathSeparator _T(';')

#else /* Platforms other than Windows */

#define dirSeparator '/'
#define pathSeparator ':'


// Define classpath
#ifdef __linux__

#define CLASSPATH "lib/db4o-4.5-java1.4.jar:lib/kxml-plugin.jar:lib/osgi_core.jar:lib/xmlpull_1_1_3_1.jar:lib/jakarta-oro-2.0.7.jar:lib/runtime.jar:lib/xswt.jar:lib/jface.jar:lib/objectmanager.jar:lib/linux/swt-cairo.jar:lib/linux/swt.jar:lib/linux/swt-mozilla.jar:lib/linux/swt-pi.jar"
#define LIBPATH   "lib/linux"
#define VMEXE     "java"
#define EXTENSION ""

#endif

#ifdef _WIN32

#define CLASSPATH "lib\\osgi_core.jar;lib\\runtime.jar;lib\\objectmanager.jar;lib\\db4o-4.5-java1.4.jar;lib\\jakarta-oro-2.0.7.jar;lib\\jface.jar;lib\\kxml-plugin.jar;lib\\windows\\swt-cairo.jar;lib\\windows\\swt-mozilla.jar;lib\\windows\\swt-pi.jar;lib\\windows\\swt.jar;lib\\xmlpull_1_1_3_1.jar;lib\\xswt.jar;lib\\windows\\swt.jar"
#define LIBPATH   "lib\\windows"
#define EXTENSION ".exe"

#ifdef _IKVM

#define VMEXE     "ikvm\\bin\\ikvm"

#else

#define VMEXE     "java"

#endif

#endif



#define _TCHAR char
#define _T(s) s
#define _tcschr strchr
#define _tcslen strlen
#define _stprintf sprintf
#define _tprintf printf
#define _tcsicmp strcasecmp
#define _tcscpy strcpy
#define _tgetcwd getcwd
#define _tcscat strcat
#define _tgetenv getenv
#define _tcsncpy strncpy
#define _tstat stat
#define _tcscmp strcmp
#define _tcsrchr strrchr
#define _stat stat

#endif /* _WIN32 */


/*
 * Find the absolute pathname to where a command resides.
 *
 * The string returned by the function must be freed.
 */
#define EXTRA 20
_TCHAR* findCommand( _TCHAR* command )
{
    _TCHAR*  cmdPath;
    int    length;
    _TCHAR*  ch;
    _TCHAR*  dir;
    _TCHAR*  path;
    struct _stat stats;

    /* If the command was an abolute pathname, use it as is. */
    if (command[0] == dirSeparator ||
       (_tcslen( command ) > 2 && command[1] == _T(':')))
    {
        length = _tcslen( command );
        cmdPath = malloc( (length + EXTRA) * sizeof(_TCHAR) ); /* add extra space for a possible ".exe" extension */
        _tcscpy( cmdPath, command );
    }

    else
    {
        /* If the command string contains a path separator */
        if (_tcschr( command, dirSeparator ) != NULL)
        {
            /* It must be relative to the current directory. */
            length = MAX_PATH_LENGTH + EXTRA + _tcslen( command );
            cmdPath = malloc( length * sizeof (_TCHAR));
            _tgetcwd( cmdPath, length );
            if (cmdPath[ _tcslen( cmdPath ) - 1 ] != dirSeparator)
            {
                length = _tcslen( cmdPath );
                cmdPath[ length ] = dirSeparator;
                cmdPath[ length+1 ] = _T('\0');
            }
            _tcscat( cmdPath, command );
        }

        /* else the command must be in the PATH somewhere */
        else
        {
            /* Get the directory PATH where executables reside. */
            path = (_TCHAR*) _tgetenv( _T("PATH") );
            length = _tcslen( path ) + _tcslen( command ) + MAX_PATH_LENGTH;
            cmdPath = malloc( length * sizeof(_TCHAR));

            /* Foreach directory in the PATH */
            dir = path;
            while (dir != NULL && *dir != _T('\0'))
            {
                ch = _tcschr( dir, pathSeparator );
                if (ch == NULL)
                {
                    _tcscpy( cmdPath, dir );
                }
                else
                {
                    length = ch - dir;
                    _tcsncpy( cmdPath, dir, length );
                    cmdPath[ length ] = _T('\0');
                    ch++;
                }
                dir = ch; /* advance for the next iteration */

                /* Determine if the executable resides in this directory. */
                if (cmdPath[0] == _T('.') &&
                   (_tcslen(cmdPath) == 1 || (_tcslen(cmdPath) == 2 && cmdPath[1] == dirSeparator)))
                {
                	_tgetcwd( cmdPath, MAX_PATH_LENGTH );
                }
                if (cmdPath[ _tcslen( cmdPath ) - 1 ] != dirSeparator)
                {
                    length = _tcslen( cmdPath );
                    cmdPath[ length ] = dirSeparator;
                    cmdPath[ length+1 ] = _T('\0');
                }
                _tcscat( cmdPath, command );

                /* If the file is not a directory and can be executed */
                if (_tstat( cmdPath, &stats ) == 0 && (stats.st_mode & S_IFREG) != 0)
                {
                    /* Stop searching */
                    dir = NULL;
                }
            }
        }
    }

#ifdef _WIN32
	/* If the command does not exist */
    if (_tstat( cmdPath, &stats ) != 0 || (stats.st_mode & S_IFREG) == 0)
    {
    	/* If the command does not end with .exe, append it an try again. */
    	length = _tcslen( cmdPath );
    	if (length > 4 && _tcsicmp( &cmdPath[ length - 4 ], _T(".exe") ) != 0)
    	    _tcscat( cmdPath, _T(".exe") );
    }
#endif

    /* Verify the resulting command actually exists. */
    if (_tstat( cmdPath, &stats ) != 0 || (stats.st_mode & S_IFREG) == 0)
    {
        free( cmdPath );
        cmdPath = NULL;
    }

    /* Return the absolute command pathname. */
    return cmdPath;
}


void usage(char **argv) {
  fprintf(stderr, "Either put Java on your path or include the path to it on the command line.\n\n");
  fprintf(stderr, "Usage: %s [%cpath%cto%cjava%s]\n\n", argv[0], dirSeparator, dirSeparator, dirSeparator, EXTENSION);
}


int main(int argc, char **argv) {
  char* vm_path;
  char command[1024*20];
  
  if (argc > 1) {
  
  	if (strcmp("--help", argv[1]) != 0 || strcmp("-?", argv[1]) != 0) {
  	  usage(argv);
  	  exit(0);
  	}
  
    vm_path = findCommand(argv[1]);
    
    // Maybe they forgot to include the java executable
    if (vm_path == NULL) {
	  command[0] = 0x0;
	  
	  char separator[2];
	  separator[0] = dirSeparator;
	  separator[1] = 0x0;
	  
	  strncpy(command, argv[1], 1024*19);
	  strcat(command, separator);
	  strcat(command, VMEXE);
	  
	  vm_path = findCommand(command);
	  
	  // Maybe they just gave us $JAVA_HOME
      if (vm_path == NULL) {
	    command[0] = 0x0;
	  
	    char separator[2];
        separator[0] = dirSeparator;
        separator[1] = 0x0;
	  
	    strncpy(command, argv[1], 1024*19);
	    strcat(command, separator);
	    strcat(command, "bin");
	    strcat(command, separator);
	    strcat(command, VMEXE);
	  
	    vm_path = findCommand(command);
      }
    }
  } else {
    vm_path = findCommand(VMEXE);
  }
  
  if (vm_path == NULL) {
  	fprintf(stderr, "Could not find Java virtual machine: %s%s\n", VMEXE, EXTENSION);
  	usage(argv);
  	exit(1);
  }
  printf("Using JVM: %s\n", vm_path);
  
  sprintf(command, "%s -Djava.library.path=%s -classpath %s com.db4o.browser.gui.standalone.StandaloneBrowser",
  	vm_path, LIBPATH, CLASSPATH);
  free(vm_path);
  
  system(command);
}

