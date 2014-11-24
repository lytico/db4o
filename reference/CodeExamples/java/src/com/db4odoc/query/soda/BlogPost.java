package com.db4odoc.query.soda;

import java.util.*;


class BlogPost {
    private String title;
    private String content;
    private List<String> tags =  new ArrayList<String>();
    private final List<Author> authors = new ArrayList<Author>();
    private final Map<String,Object> metaData = new HashMap<String, Object>();

    public BlogPost(String title, String content) {
        this.title = title;
        this.content = content;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public void addTags(String...tags){
        this.tags.addAll(Arrays.asList(tags));
    }
    public void addAuthors(Author...authors){
        this.authors.addAll(Arrays.asList(authors));
    }

    public List<String> getTags() {
        return tags;
    }

    public void addMetaData(String key, Object value){
        this.metaData.put(key, value);
    }

    public Map<String, Object> getMetaData() {
        return metaData;
    }

    @Override
    public String toString() {
        return "BlogPost{" +
                "title='" + title + '\'' +
                ", content='" + content + '\'' +
                '}';
    }
}
