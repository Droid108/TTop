package com.krishnak.tweettop.Tasks;

import org.json.JSONArray;

/**
 * Created by SupportPedia on 30-10-2014.
 */
public interface GetJSONListener {
    public void onRemoteCallComplete(JSONArray jsonFromNet);

    public void onRemoteCallStart();

    public void onRemoteCallProgress();
}