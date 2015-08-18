package com.droid108.tweetrap.Helpers;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.pm.ActivityInfo;
import android.content.res.Configuration;

import com.droid108.tweetrap.R;

import java.lang.reflect.Field;


/**
 * Created by SupportPedia on 12-11-2014.
 */
public class SPF {

    public static boolean SetSharedPreference(Context context, int key, String value) {
        SharedPreferences sharedPref = context.getSharedPreferences(context.getString(key), Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPref.edit();
        editor = editor.putString(context.getString(key), value);
        editor.commit();
        return true;
    }


    public static boolean SetSharedPreferenceLong(Context context, int key, long value) {
        SharedPreferences sharedPref = context.getSharedPreferences(context.getString(key), Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPref.edit();
        editor = editor.putLong(context.getString(key), value);
        editor.commit();
        return true;
    }
    public static String GetSharedPreference(int key, Context context) {
        SharedPreferences sharedPref = context.getSharedPreferences(context.getString(key), Context.MODE_PRIVATE);
        return sharedPref.getString(context.getString(key), "");
    }

    public static long GetSharedPreferenceAsLong(int key, Context context) {
        SharedPreferences sharedPref = context.getSharedPreferences(context.getString(key), Context.MODE_PRIVATE);
        return sharedPref.getLong(context.getString(key), 0L);
    }

    public static void Clear(Context context) {
        Field[] fields = R.string.class.getFields();
        String[] allDrawablesNames = new String[fields.length];
        for (int i = 0; i < fields.length; i++) {
            //allDrawablesNames[i] = fields[i].getName();
            String fName = fields[i].getName();
            if (fName.toLowerCase().indexOf("spf_") >= 0) {
                try {
                    int id = fields[i].getInt(R.string.class);
                    removeSP(context, id);
                } catch (IllegalAccessException e) {
                    e.printStackTrace();
                }
            }
        }
    }

    private static void removeSP(Context context, int key) {
        SharedPreferences settings = context.getSharedPreferences(context.getString(key), Context.MODE_PRIVATE);
        settings.edit().clear().commit();
    }

    public static void lockScreenOrientation(Activity context) {
        int currentOrientation = context.getResources().getConfiguration().orientation;
        if (currentOrientation == Configuration.ORIENTATION_PORTRAIT) {
            context.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
        } else {
            context.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
        }
    }

    public static void unlockScreenOrientation(Activity context) {
        context.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_SENSOR);
    }
}
