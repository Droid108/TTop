package com.krishnak.tweetrap.fragments;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.TextView;

import com.droid108.tweetrap.Adapter.TweetAdapter;
import com.droid108.tweetrap.Helpers.SPF;
import com.droid108.tweetrap.R;
import com.droid108.tweetrap.Tasks.GetJSONListener;
import com.droid108.tweetrap.Tasks.JSONClient;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdView;
import com.google.android.gms.ads.InterstitialAd;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Collections;

/**
 * Created by SupportPedia on 04-04-2015.
 */
public class TechnologyFragment extends Fragment {

    PullToRefreshListView pullToRefreshView;
    TweetAdapter madapter;
    View rootView;
    int fTypes = 0;
    int fromIds = 0;
    int lastId = 0;
    int firstId = 0;
    ArrayList<JSONObject> jsonData = null;
    InterstitialAd mInterstitialAd;
    boolean isRefreshinProgress = false;

    public TechnologyFragment() {
    }

    @Override
    public void onResume() {
        super.onResume();
        pullToRefreshView.setRefreshing(true);
    }

    @Override
    public void onPause() {
        super.onPause();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        rootView = inflater.inflate(R.layout.fragment_technology, container, false);

        AdView mAdView = (AdView) rootView.findViewById(R.id.adView);
        AdRequest adRequest = new AdRequest.Builder().build();
        //adRequest.isTestDevice(this);
        mAdView.loadAd(adRequest);
        pullToRefreshView = (PullToRefreshListView) rootView.findViewById(R.id.pull_to_refresh_listview_tech);
        TextView emptyVIew = (TextView) rootView.findViewById(R.id.emptyView);
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
        String jsonString = SPF.GetSharedPreference(R.string.spf_tecchnology_tweets, rootView.getContext());
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
        pullToRefreshView.setEmptyView(emptyVIew);
        fromIds = firstId;
        callClient(fTypes, fromIds);
//        pullToRefreshView.post(new Runnable() {
//            @Override
//            public void run() {
//                pullToRefreshView.setRefreshing(true);
//            }
//        });
        return rootView;
    }


    private void callClient(int fType, int fromId) {
        Log.i("call cleint", "Entered Client Method");
        GetJSONListener listener = new GetJSONListener() {
            @Override
            public void onRemoteCallComplete(JSONArray jsonFromNet) {
                if (jsonFromNet != null && jsonFromNet.toString().length() > 2) {
                    syncTweets(jsonFromNet, jsonData);
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
        if (!isRefreshinProgress) {
            JSONClient _client = new JSONClient(rootView.getContext(), listener);
            _client.execute("http://tweetrap.elasticbeanstalk.com/api/cattech?ftype=" + fType + "&fromid=" + fromId);
            isRefreshinProgress = true;
        }
    }


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
        SPF.SetSharedPreference(rootView.getContext(), R.string.spf_tecchnology_tweets, json);
        isRefreshinProgress = false;
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


