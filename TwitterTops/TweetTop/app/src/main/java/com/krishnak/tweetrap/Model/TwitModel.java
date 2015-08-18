package com.droid108.tweetrap.Model;

import java.util.Date;

/**
 * Created by SupportPedia on 04-04-2015.
 */
public class TwitModel {
    private String profileUrl;

    private String text;

    private String location;

    private String name;

    private String screenName;

    private long userId;

    private long fav_Count;

    private long rT_Count;

    private Date created_at;

    private long twitID;

    private long id;

    private boolean isVerified;

    public TwitModel() {
    }

    public String getProfileUrl() {
        return this.profileUrl;
    }

    public String getText() {
        return this.text;
    }

    public String getLocation() {
        return this.location;
    }

    public String getName() {
        return this.name;
    }

    public String getScreenName() {
        return this.screenName;
    }

    public long getUserId() {
        return this.userId;
    }

    public long getFav_Count() {
        return this.fav_Count;
    }

    public long getrT_Count() {
        return this.rT_Count;
    }

    public long getTwitID() {
        return this.twitID;
    }

    public long getId() {
        return this.id;
    }

    public boolean getisVerified() {
        return this.isVerified;
    }

    public Date getCreated_at() {
        return this.created_at;
    }

    public void setProfileUrl(String profileUrl) {
        this.profileUrl = profileUrl;
    }

    public void setText(String text) {
        this.text = text;
    }
    public void setLocation(String location) {
        this.location = location;
    }
    public void setrT_Count(long rT_count) {
        this.rT_Count = rT_count;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setScreenName(String screenName) {
        this.screenName = screenName;
    }

    public void setUserId(long userId) {
        this.userId = userId;
    }

    public void setFav_Count(long fav_count) {
        this.fav_Count = fav_count;
    }

    public void setTwitID(long twitID) {
        this.twitID = twitID;
    }

    public void setCreated_at(Date created_at) {
        this.created_at = created_at;
    }

    public void setId(long id) {
        this.id = id;
    }

    public void setVerified(boolean isvVerified) {
        this.isVerified = isvVerified;
    }

}