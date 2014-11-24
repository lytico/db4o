indexing
	description: "Translator to store any STRING as a SYSTEM_STRING in db4o"
	author: "Carl Rosenberger"
	date: "$Date: 2005/08/15 12:00:33 $"
	revision: "$Revision: 1.2 $"

class
	STRING_TRANSLATOR_4E

inherit
	OBJECT_CONSTRUCTOR

create    
	make

feature

	system_string_class: CLASS_

    make (clazz: CLASS_) is
    	do
    		system_string_class := clazz
    	end

	stored_class: CLASS_ is
		do
			RESULT := system_string_class
		end
	
	on_instantiate (container: OBJECT_CONTAINER; stored_object: SYSTEM_OBJECT): SYSTEM_OBJECT is
		local
			sys_str : SYSTEM_STRING
			str : STRING
		do
			sys_str ?= stored_object
			str := sys_str
			RESULT := str
		end

	on_store (container: OBJECT_CONTAINER; application_object: SYSTEM_OBJECT): SYSTEM_OBJECT is
		local
			sys_str : SYSTEM_STRING
			str : STRING
		do
			str ?= application_object
			sys_str := str
			RESULT := sys_str
		end

	on_activate (container: OBJECT_CONTAINER; application_object: SYSTEM_OBJECT; stored_object: SYSTEM_OBJECT) is
		do
		end
end
