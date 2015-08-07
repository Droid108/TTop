package com.krishnak.tweettop.fragments;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.krishnak.tweettop.R;

/**
 * Created by SupportPedia on 07-04-2015.
 */
public class NewsFragment extends Fragment {

    public NewsFragment(){}

    @Override

    public View onCreateView(LayoutInflater inflater, ViewGroup container,Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.fragment_news, container, false);
        return rootView;
    }

}