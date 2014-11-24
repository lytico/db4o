indexing 
	description: "A simple class to store in db4o"
	author: "Carl Rosenberger"
	date: "$Date: 2005/08/14 14:57:34 $"
	revision: "$Revision: 1.1 $"

class
	CAR
	
create    
	make, make_name, make_ferrari, make_bmw

feature -- Persistent

	system_name: SYSTEM_STRING
	eiffel_name: STRING

feature -- Initialisation

    make is
    	do
    	end
    		
    make_name (name : String) is
    	do
			system_name := name.to_cil
			eiffel_name := name
    	end
    		
    make_ferrari is
    		do
    			make_name("Ferrari")
    		end
    		
    make_bmw is
    		do
				make_name("BMW")
    		end
    		
feature -- Printing

	print_out is
		local
			eiffel_string : STRING
    	do
			io.put_string ("Car ")
			io.put_new_line
			io.put_string ("system_name is ")
			create eiffel_string.make_from_cil (system_name)
			io.put_string (eiffel_string)
			io.put_new_line
			io.put_string ("eiffel_name is ")
			io.put_string (eiffel_name)
			io.put_new_line
    	end

end
