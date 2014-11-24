package com.db4odoc.exampleapp.web;


import com.db4o.ObjectSet;
import com.db4o.query.Predicate;
import com.db4odoc.exampleapp.web.db.Db4oContext;
import com.db4odoc.exampleapp.web.model.Pilot;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.view.RedirectView;

import java.util.ArrayList;


@Controller
@RequestMapping(value = "/")
@Scope(value = "request")
public class HomeController {


    @Autowired(required = true)
    private Db4oContext db4o;
    private Pilot pilot;

    // #example: List all pilots on the index-page
    @RequestMapping(value = "list.html", method = RequestMethod.GET)
    public ModelAndView get() {
        ObjectSet pilots = db4o.objectContainer().query(Pilot.class);
        return new ModelAndView("list", "pilots", new ArrayList<Pilot>(pilots));
    }
    // #end example

    
    @RequestMapping(value = "/new.html", method = RequestMethod.GET)
    public ModelAndView getNewForm() {
        ModelAndView view = new ModelAndView("new", "command", new Pilot());
        return view.addObject(new Pilot());
    }

    @RequestMapping(value = "/new.html", method = RequestMethod.POST)
    public ModelAndView createPilot(Pilot newPilot) {
        db4o.objectContainer().store(newPilot);
        return new ModelAndView(new RedirectView("list.html"));
    }

    @RequestMapping(value = "/delete{id}.html", method = RequestMethod.GET)
    public ModelAndView deletePilot(@PathVariable final String id) {
        pilot = pilotById(id);
        db4o.objectContainer().delete(pilot);
        return new ModelAndView(new RedirectView("list.html"));
    }

    @RequestMapping(value = "/edit{id}.html", method = RequestMethod.GET)
    public ModelAndView editPilot(@PathVariable final String id) {
        Pilot pilot = pilotById(id);
        ModelAndView view = new ModelAndView("edit", "command", pilot);
        return view.addObject(new Pilot());
    }

    // #example: Update the object
    @RequestMapping(value = "/edit{id}.html", method = RequestMethod.POST)
    public ModelAndView editPilot(@PathVariable final String id, Pilot pilotFromForm) {
        Pilot pilotFromDatabase = db4o.objectContainer().query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot p) {
                return p.getId().equals(id);
            }
        }).get(0);
        pilotFromDatabase.setName(pilotFromForm.getName());
        pilotFromDatabase.setPoints(pilotFromForm.getPoints());
        db4o.objectContainer().store(pilotFromDatabase);
        return new ModelAndView(new RedirectView("list.html"));
    }
    // #end example


    private Pilot pilotById(final String id) {
        return db4o.objectContainer().query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot p) {
                return p.getId().equals(id);
            }
        }).get(0);
    }


}
