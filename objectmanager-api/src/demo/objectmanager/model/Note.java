package demo.objectmanager.model;

/**
 * User: treeder
 * Date: Sep 8, 2006
 * Time: 10:33:15 AM
 */
public class Note {
    private String text;
    private boolean secret;


    public Note() {
    }

    public Note(String text, boolean secret) {

        this.text = text;
        this.secret = secret;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public boolean isSecret() {
        return secret;
    }

    public void setSecret(boolean secret) {
        this.secret = secret;
    }
}
