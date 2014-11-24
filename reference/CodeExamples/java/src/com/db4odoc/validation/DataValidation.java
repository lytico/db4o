package com.db4odoc.validation;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.events.*;

import javax.validation.ConstraintViolation;
import javax.validation.Validation;
import javax.validation.ValidationException;
import javax.validation.Validator;
import java.util.Set;

public class DataValidation {

    public static void main(String[] args) {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4");
        try {
            // #example: Register validation for the create and update event
            EventRegistry events = EventRegistryFactory.forObjectContainer(container);
            events.creating().addListener(new ValidationListener());
            events.updating().addListener(new ValidationListener());
            // #end example


            // #example: Storing a valid pilot
            Pilot pilot = new Pilot("Joe");
            container.store(pilot);
            // #end example


            // #example: Storing a invalid pilot throws exception
            Pilot otherPilot = new Pilot("");
            try {
                container.store(otherPilot);
            } catch (EventException e) {
                ValidationException cause = (ValidationException) e.getCause();
                System.out.println(cause.getMessage());
            }
            // #end example
        } finally {
            container.close();
        }
    }

    // #example: Validation support
    private static class ValidationListener implements EventListener4<CancellableObjectEventArgs> {
        private final Validator validator = Validation.buildDefaultValidatorFactory()
                .getValidator();

        @Override
        public void onEvent(Event4<CancellableObjectEventArgs> events,
                            CancellableObjectEventArgs eventInfo) {
            Set<ConstraintViolation<Object>> violations = validator.validate(eventInfo.object());
            if (!violations.isEmpty()) {
                throw new ValidationException(buildMessage(violations));
            }
        }

        private String buildMessage(Set<ConstraintViolation<Object>> violations) {
            final StringBuilder builder = new StringBuilder("Violations of validation-rules:\n");
            for (ConstraintViolation<Object> violation : violations) {
                builder.append(violation.getPropertyPath()).append(" ")
                        .append(violation.getMessage()).append("\n");
            }
            return builder.toString();
        }
    }
    // #end example


}
