indexing
	description: "My very first test of db4o with Eiffel"
	author: "Carl Rosenberger"
	date: "$Date: 2005/08/15 15:51:07 $"
	revision: "$Revision: 1.2 $"

class
	DB4O_SAMPLE_APPLICATION

create    
	make

feature -- Initialization

	database_file : STRING
	
	init is 
		do
			database_file := "eiffel.yap"
			install_db4o_eiffel_configuration
	  	end

feature -- db4o database control

	db : OBJECT_CONTAINER

  	install_db4o_eiffel_configuration is
	  	local
			eiffel_configuration : CONFIGURATION_4E
		do
			create eiffel_configuration.make
	  	end
	  	
	open_database is
		do
			db := {DB_4O}.open_file(database_file)
		end
		
	close_database is
		local
			close_result: BOOLEAN
		do
			close_result := db.close
		end

	

feature -- Run

	make is
		do  
			init
			
			open_database
			store_two_cars
			close_database
			
			open_database
			print_all_stored_cars
			query_by_example
			soda_query_for_car_name
			close_database
			
		end
		
feature -- Tests
	
	store_two_cars is
		local
			my_car: CAR
		do
			create my_car.make_ferrari
			db.set(my_car)
			create my_car.make_bmw
			db.set(my_car)
		end
	
	print_all_stored_cars is
		local
			my_car: CAR
		do
			io.put_string ("***** All stored cars *****")
			io.put_new_line
			create my_car.make
			print_car_objectset(db.get (my_car))
		end

	query_by_example is
		local
			ferrari: CAR
		do
			io.put_string ("***** All Ferrari cars by example *****")
			io.put_new_line
			create ferrari.make_ferrari
			print_car_objectset(db.get (ferrari))
		end
		
	soda_query_for_car_name is
		local
			query : QUERY_4E
			c : CONSTRAINT
		do
			io.put_string ("***** All BMW cars S.O.D.A. syntax *****")
			io.put_new_line
			create query.make_extent(db, {CAR})
			c := query.descend("eiffel_name").constrain("BMW")
			print_car_objectset(query.execute)
		end
	
	print_car_objectset (objectset : OBJECT_SET) is
		local
			my_car: CAR
		do
			from
			until
				not objectset.has_next
			loop
				my_car ?= objectset.next
				if my_car /= Void then
					my_car.print_out
				end
			end
			
		end
		

end
