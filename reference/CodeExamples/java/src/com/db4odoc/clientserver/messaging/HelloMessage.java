package com.db4odoc.clientserver.messaging;


// #example: The message class
public class HelloMessage {
    private final String message;

    public HelloMessage(String message) {
        this.message = message;
    }

    @Override
    public String toString() {
        return message;
    }
}
// #end example
