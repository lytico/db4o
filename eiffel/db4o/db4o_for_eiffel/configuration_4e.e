indexing
	description: "Installation of special translators to make db4o work with Eiffel"
	author: "Carl Rosenberger"
	date: "$Date: 2005/08/15 12:00:33 $"
	revision: "$Revision: 1.2 $"

class
	CONFIGURATION_4E

create    
	make

feature -- Initialization

	make is
		do
			install_translators
		end

feature -- Translators

   	install_translators is
	    local
	    	string_translator : STRING_TRANSLATOR_4E
   		do
			create string_translator.make(system_string_class)
			translate("any string", string_translator)
   		end

feature -- Helpers
	
    j4o_class : CLASS_ is
    	once
    		create Result.make(any_system_string.get_type)
    	end
    
    any_system_string : SYSTEM_STRING is
        once
            create Result.make_from_c_and_count ('o', 1)
        end
    
	system_string_class : CLASS_ is
		once
			Result := class_for_object (any_system_string)
		end

	class_for_object(obj: SYSTEM_OBJECT): CLASS_ is
		do
			Result := j4o_class.get_class_for_object (obj)
		end
		
	translate(obj: SYSTEM_OBJECT; translator: OBJECT_TRANSLATOR) is
		do
			{DB_4O}.configure.object_class (class_for_object (obj)).translate(translator)
		end
		
end
