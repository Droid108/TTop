package com.krishnak.tweettop.fragments;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.krishnak.tweettop.Adapter.TweetAdapter;
import com.krishnak.tweettop.Helpers.SPF;
import com.krishnak.tweettop.R;
import com.krishnak.tweettop.Tasks.GetJSONListener;
import com.krishnak.tweettop.Tasks.JSONClient;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Collections;

/**
 * Created by SupportPedia on 04-04-2015.
 */
public class LoveFragment extends Fragment {

    PullToRefreshListView pullToRefreshView;
    TweetAdapter madapter;
    View rootView;
    int fTypes = 0;
    int fromIds = 0;
    int lastId = 0;
    int firstId = 0;
    ArrayList<JSONObject> jsonData = null;

    public LoveFragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        rootView = inflater.inflate(R.layout.fragment_love, container, false);
        pullToRefreshView = (PullToRefreshListView) rootView.findViewById(R.id.pull_to_refresh_listview);
        pullToRefreshView.setMode(PullToRefreshBase.Mode.BOTH);
        pullToRefreshView.setOnRefreshListener(new PullToRefreshBase.OnRefreshListener2<ListView>() {
            @Override
            public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
                fTypes = 0;
                fromIds = firstId;
                callClient(fTypes, fromIds);
            }

            @Override
            public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
                fTypes = 1;
                fromIds = lastId;
                callClient(fTypes, fromIds);
            }
        });

        jsonData = new ArrayList<JSONObject>();
        String jsonString = SPF.GetSharedPreference(R.string.spf_love_tweets, getActivity().getApplicationContext());
        Type type = new TypeToken<ArrayList<JSONObject>>() {
        }.getType();
        Gson gson = new Gson();
        jsonData = gson.fromJson(jsonString, type);
        //jsonData.clear();
        if (jsonData == null)
            jsonData = new ArrayList<JSONObject>();
        else
            removeAllItems();
        madapter = new TweetAdapter(rootView.getContext(), jsonData);
        pullToRefreshView.setAdapter(madapter);
        fromIds = firstId;
        callClient(fTypes, fromIds);
        //pullToRefreshView.setRefreshing(true);
        return rootView;
    }

    private void callClient(int fType, int fromId) {
        GetJSONListener listener = new GetJSONListener() {
            @Override
            public void onRemoteCallComplete(JSONArray jsonFromNet) {
                if (jsonFromNet != null && jsonFromNet.toString().length() > 2) {
                    syncTweets(jsonFromNet, jsonData);
                    //madapter = new TweetAdapter(rootView.getContext(), jsonData);
                    //pullToRefreshView.setAdapter(madapter);
                    madapter.notifyDataSetChanged();
                }
                pullToRefreshView.onRefreshComplete();
            }

            @Override
            public void onRemoteCallStart() {

            }

            @Override
            public void onRemoteCallProgress() {

            }
        };
        JSONClient _client = new JSONClient(getActivity(), listener);
        _client.execute("http://tweetrap.elasticbeanstalk.com/api/oget?fType=" + fType + "&fromId=" + fromId);

    }

//    private JSONArray syncTweets(JSONArray inputList, JSONArray existingList) {
//        boolean idExists = false;
//        int internalId = 0;
//        int existinginternalId = 0;
//        try {
//            lastId = inputList.getJSONObject(inputList.length() - 1).getInt("Id");
//        } catch (JSONException e) {
//            e.printStackTrace();
//        }
//        if (existingList == null || existingList.length() == 0) {
//            existingList = inputList;
//            return existingList;
//        } else {
//            ArrayList<JSONObject> existingArrayList = new ArrayList<JSONObject>();
//            existingArrayList = convertJsonToAL(existingList);
//            for (int i = 0; i < inputList.length(); i++) {
//                internalId = 0;
//                try {
//                    internalId = inputList.getJSONObject(i).getInt("Id");
//                } catch (JSONException e) {
//                    e.printStackTrace();
//                }
//
//                if (internalId != 0) {
//                    if (existingList == null)
//                        existingList = new JSONArray();
//
//
//                    for (int j = 0; i < existingList.length(); i++) {
//                        try {
//                            if (existingList.getJSONObject(j).getInt("Id") == internalId) {
//                                idExists = true;
//                            }
//                        } catch (JSONException e) {
//                            e.printStackTrace();
//                        }
//                    }
//                    if (idExists) {
//                        try {
//                            int removeIndex = -1;
//                            removeIndex = existingArrayList.indexOf(inputList.getJSONObject(i));
//                            if (removeIndex != -1) {
//                                existingArrayList.remove(removeIndex);
//                                existingArrayList.add(removeIndex, inputList.getJSONObject(i));
//                            } else
//                                existingArrayList.add(inputList.getJSONObject(i));
//                        } catch (JSONException e) {
//                            e.printStackTrace();
//                        }
//                        idExists = false;
//                    }
//
//                }
//            }
//            existingList = new JSONArray(existingArrayList);
//            return existingList;
//        }
//    }

    private void syncTweets(JSONArray inputList, ArrayList<JSONObject> existingList) {
        boolean idExists = false;
        int internalId = 0;
        int existinginternalId = 0;
        int tempFirstId = 0;
        try {
            tempFirstId = inputList.getJSONObject(0).getInt("Id");
        } catch (JSONException e) {
            e.printStackTrace();
        }
        if (tempFirstId - firstId > 50) {
            jsonData.clear();
            ArrayList<JSONObject> tempObj = new ArrayList<JSONObject>();
            for (int i = 0; i < jsonData.size(); i++) {
                tempObj.add(jsonData.get(i));
            }
            jsonData.addAll(tempObj);
        }
        if (existingList == null || existingList.size() == 0) {
            jsonData.addAll(convertJsonToAL(inputList));
            //return existingList;
        } else {
            ArrayList<JSONObject> inputArrayList = new ArrayList<JSONObject>();
            inputArrayList = convertJsonToAL(inputList);
            if (fTypes == 0) {
                Collections.reverse(inputArrayList);
                for (int i = 0; i < inputArrayList.size(); i++) {
                    jsonData.add(0, inputArrayList.get(i));
                    madapter.notifyDataSetChanged();
                }
                pullToRefreshView.post(new Runnable() {
                    @Override
                    public void run() {
                        //pullToRefreshView.smoothScrollToPosition(0);
                        pullToRefreshView.getRefreshableView().smoothScrollToPosition(0);
                    }
                });
            } else
                jsonData.addAll(inputArrayList);
        }
        try {
            firstId = jsonData.get(0).getInt("Id");
            lastId = jsonData.get(jsonData.size() - 1).getInt("Id");
        } catch (JSONException e) {
            e.printStackTrace();
        }
        Gson gson = new Gson();
        String json = gson.toJson(jsonData);
        SPF.SetSharedPreference(getActivity().getApplicationContext(), R.string.spf_love_tweets, json);
    }

    private ArrayList<JSONObject> convertJsonToAL(JSONArray jsonObject) {
        ArrayList<JSONObject> listdata = new ArrayList<JSONObject>();
        if (jsonObject != null) {
            for (int i = 0; i < jsonObject.length(); i++) {
                try {
                    listdata.add(jsonObject.getJSONObject(i));
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        }
        return listdata;
    }

    private void removeAllItems() {
        ArrayList<JSONObject> tempObj = new ArrayList<JSONObject>();
        for (int i = 0; i < jsonData.size(); i++) {
            if (i < 50)
                tempObj.add(jsonData.get(i));
            else
                break;
        }
        jsonData.clear();
        jsonData.addAll(tempObj);
        try {
            firstId = jsonData.get(0).getInt("Id");
            lastId = jsonData.get(49).getInt("Id");
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }
}


