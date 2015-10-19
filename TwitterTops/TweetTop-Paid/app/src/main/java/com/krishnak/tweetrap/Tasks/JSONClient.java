package com.droid108.tweetrap.Tasks;

import android.content.Context;
import android.os.AsyncTask;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONArray;
import org.json.JSONException;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;


/**
 * Created by SupportPedia on 30-10-2014.
 */
public class JSONClient extends AsyncTask<String, Void, JSONArray> {
    static Context curContext;
    GetJSONListener getJSONListener;
    public JSONClient(Context context, GetJSONListener listener) {
        this.getJSONListener = listener;
        curContext = context;

//        if (!checkInternetConnection((Activity) context)) {
//            this.cancel(true);
//        }
    }

    private static String convertStreamToString(InputStream is) {
        BufferedReader reader = new BufferedReader(new InputStreamReader(is));
        StringBuilder sb = new StringBuilder();

        String line = null;
        try {
            while ((line = reader.readLine()) != null) {
                sb.append(line + "\n");
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                is.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        return sb.toString();
    }


    public static JSONArray connect(String url) {
        HttpClient httpclient = new DefaultHttpClient();
        HttpGet httpget = new HttpGet(url);
        // Execute the request
        HttpResponse response;
        try {
            response = httpclient.execute(httpget);
            HttpEntity entity = response.getEntity();
            if (entity != null) {
                InputStream instream = entity.getContent();
                String result = convertStreamToString(instream);
                JSONArray json = new JSONArray(result);
                instream.close();
                return json;
            }
        } catch (ClientProtocolException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return null;
    }

    @Override
    protected void onProgressUpdate(Void[] values) {
        super.onProgressUpdate(values);

    }

    @Override
    public void onPreExecute() {
        //SPF.lockScreenOrientation((Activity) curContext);
        getJSONListener.onRemoteCallStart();
    }

//    Crouton thisCrouton = null;
//    public  boolean checkInternetConnection(Activity activity) {
//        Net_Connection net_connection = new Net_Connection(activity);
//        if (!net_connection.IsNetworkConnected()) {
//            thisCrouton = SpCrouton.MakeError("Not connected to internet", activity);
//            thisCrouton.show();
//            return false;
//        } else {
//            if (thisCrouton != null)
//                Crouton.hide(thisCrouton);
//            return true;
//        }
//    }
    @Override
    protected JSONArray doInBackground(String... urls) {
        if (isCancelled()) {
            return null;
        }
        getJSONListener.onRemoteCallProgress();
        return connect(urls[0]);
    }

    @Override
    protected void onPostExecute(JSONArray json) {
        getJSONListener.onRemoteCallComplete(json);
        //SPF.unlockScreenOrientation((Activity) curContext);
        //progressDialog.dismiss();
    }
}

